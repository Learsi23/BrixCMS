using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis.PageSegmenter;
using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;

namespace BrixCMS.Open.Services.Ingestion;

public class DataIngestor(
    ILogger<DataIngestor> logger,
    VectorStoreCollection<string, IngestedChunk> chunksCollection,
    VectorStoreCollection<string, IngestedDocument> documentsCollection,
    IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
{
    public static async Task IngestDataAsync(IServiceProvider services, IIngestionSource source)
    {
        using var scope = services.CreateScope();
        var ingestor = scope.ServiceProvider.GetRequiredService<DataIngestor>();
        await ingestor.IngestDataAsync(source);
    }

    public async Task IngestDataAsync(IIngestionSource source)
    {
        await chunksCollection.EnsureCollectionExistsAsync();
        await documentsCollection.EnsureCollectionExistsAsync();

        var sourceId = source.SourceId;
        var existingDocs = await GetAllDocsAsync(documentsCollection);
        var documentsForSource = existingDocs.Where(doc => doc.SourceId == sourceId).ToList();

        var deletedDocuments = await source.GetDeletedDocumentsAsync(documentsForSource);
        foreach (var deletedDocument in deletedDocuments)
        {
            logger.LogInformation("Removing ingested data for {documentId}", deletedDocument.DocumentId);
            await DeleteChunksForDocumentIdAsync(chunksCollection, deletedDocument.DocumentId);
            await documentsCollection.DeleteAsync(deletedDocument.Key);
        }

        var modifiedDocuments = await source.GetNewOrModifiedDocumentsAsync(documentsForSource);
        foreach (var modifiedDocument in modifiedDocuments)
        {
            logger.LogInformation("Processing {documentId}", modifiedDocument.DocumentId);
            await DeleteChunksForDocumentIdAsync(chunksCollection, modifiedDocument.DocumentId);
            await documentsCollection.UpsertAsync(modifiedDocument);

            var newRecords = await source.CreateChunksForDocumentAsync(modifiedDocument);
            foreach (var chunk in newRecords)
                await chunksCollection.UpsertAsync(chunk);
        }

        logger.LogInformation("Ingestion is up-to-date");
    }

    public async Task IngestPdfAsync(string filePath)
    {
        await chunksCollection.EnsureCollectionExistsAsync();
        await documentsCollection.EnsureCollectionExistsAsync();

        var documentId = Path.GetFileName(filePath);
        var documentVersion = File.GetLastWriteTimeUtc(filePath).ToString("o");

        var existingDocs = await GetAllDocsAsync(documentsCollection);
        var existingDoc = existingDocs.FirstOrDefault(d => d.DocumentId == documentId);
        if (existingDoc != null)
        {
            await DeleteChunksForDocumentIdAsync(chunksCollection, documentId);
            await documentsCollection.DeleteAsync(existingDoc.Key);
        }

        var document = new IngestedDocument
        {
            Key = Guid.CreateVersion7().ToString(),
            SourceId = "ManualUpload",
            DocumentId = documentId,
            DocumentVersion = documentVersion
        };

        await documentsCollection.UpsertAsync(document);

        using var pdf = PdfDocument.Open(filePath);
        var paragraphs = pdf.GetPages().SelectMany(GetPageParagraphs).ToList();

        foreach (var p in paragraphs)
        {
            var embedding = await embeddingGenerator.GenerateAsync(p.Text);
            await chunksCollection.UpsertAsync(new IngestedChunk
            {
                Key = Guid.CreateVersion7().ToString(),
                DocumentId = documentId,
                PageNumber = p.PageNumber,
                Text = p.Text,
                Vector = embedding.Vector
            });
        }

        logger.LogInformation("📄 PDF '{DocumentId}' ingested with {ChunkCount} chunks", documentId, paragraphs.Count);
    }

    private static async Task<List<IngestedDocument>> GetAllDocsAsync(VectorStoreCollection<string, IngestedDocument> collection)
    {
        var docs = new List<IngestedDocument>();
        var dummy = new ReadOnlyMemory<float>(new float[2]);
        await foreach (var result in collection.SearchAsync(dummy, 1000))
            docs.Add(result.Record);
        return docs;
    }

    private static async Task DeleteChunksForDocumentIdAsync(VectorStoreCollection<string, IngestedChunk> collection, string documentId)
    {
        var chunks = new List<IngestedChunk>();
        var dummy = new ReadOnlyMemory<float>(new float[384]);
        await foreach (var result in collection.SearchAsync(dummy, 10000))
            chunks.Add(result.Record);

        var toDelete = chunks.Where(r => r.DocumentId == documentId).Select(r => r.Key).ToList();
        if (toDelete.Any())
            await collection.DeleteAsync(toDelete);
    }

    private static IEnumerable<(int PageNumber, int IndexOnPage, string Text)> GetPageParagraphs(Page pdfPage)
    {
        var words = NearestNeighbourWordExtractor.Instance.GetWords(pdfPage.Letters);
        var textBlocks = DocstrumBoundingBoxes.Instance.GetBlocks(words);

        var allChunks = new List<(int PageNumber, int IndexOnPage, string Text)>();
        int chunkIndex = 0;

        foreach (var block in textBlocks)
        {
            var cleanedText = block.Text.ReplaceLineEndings(" ").Trim();
            if (string.IsNullOrWhiteSpace(cleanedText)) continue;

            foreach (var subParagraph in SplitIntoChunks(cleanedText, 80))
                allChunks.Add((pdfPage.Number, chunkIndex++, subParagraph));
        }

        return allChunks;
    }

    private static List<string> SplitIntoChunks(string text, int maxWords, int maxChars = 500)
    {
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (words.Length <= maxWords && text.Length <= maxChars) return [text];

        // Handle case where single "word" (e.g. compact JSON with no spaces) exceeds char limit
        if (words.Length == 1)
        {
            var chunks = new List<string>();
            for (int i = 0; i < text.Length; i += maxChars)
            {
                var len = Math.Min(maxChars, text.Length - i);
                chunks.Add(text.Substring(i, len));
            }
            return chunks;
        }

        var result = new List<string>();
        var sb = new System.Text.StringBuilder();
        int count = 0;
        foreach (var word in words)
        {
            if ((count > 0 && count >= maxWords) || (sb.Length + word.Length + 1 > maxChars && count > 0))
            {
                result.Add(sb.ToString().TrimEnd());
                sb.Clear();
                count = 0;
            }
            sb.Append(word).Append(' ');
            count++;
        }
        if (sb.Length > 0) result.Add(sb.ToString().TrimEnd());
        return result;
    }
}
