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
        var allDocs = await documentsCollection.GetAsync(_ => true, 1000).ToListAsync();
        var documentsForSource = allDocs.Where(doc => doc.SourceId == sourceId).ToList();

        var deletedDocuments = await source.GetDeletedDocumentsAsync(documentsForSource);
        foreach (var deletedDocument in deletedDocuments)
        {
            logger.LogInformation("Removing ingested data for {documentId}", deletedDocument.DocumentId);
            await DeleteChunksForDocumentAsync(deletedDocument);
            await documentsCollection.DeleteAsync(deletedDocument.Key);
        }

        var modifiedDocuments = await source.GetNewOrModifiedDocumentsAsync(documentsForSource);
        foreach (var modifiedDocument in modifiedDocuments)
        {
            logger.LogInformation("Processing {documentId}", modifiedDocument.DocumentId);
            await DeleteChunksForDocumentAsync(modifiedDocument);

            await documentsCollection.UpsertAsync(modifiedDocument);

            var newRecords = await source.CreateChunksForDocumentAsync(modifiedDocument);
            await chunksCollection.UpsertAsync(newRecords);
        }

        logger.LogInformation("Ingestion is up-to-date");

        async Task DeleteChunksForDocumentAsync(IngestedDocument document)
        {
            var documentId = document.DocumentId;
            var allChunks = await chunksCollection.GetAsync(_ => true, 1000).ToListAsync();
            var chunksToDelete = allChunks.Where(r => r.DocumentId == documentId).ToList();
            if (chunksToDelete.Any())
            {
                await chunksCollection.DeleteAsync(chunksToDelete.Select(r => r.Key));
            }
        }
    }

    public async Task IngestPdfAsync(string filePath)
    {
        await chunksCollection.EnsureCollectionExistsAsync();
        await documentsCollection.EnsureCollectionExistsAsync();

        var documentId = Path.GetFileName(filePath);
        var documentVersion = File.GetLastWriteTimeUtc(filePath).ToString("o");

        var allDocs = await documentsCollection.GetAsync(_ => true, 100).ToListAsync();
        var existingDoc = allDocs.FirstOrDefault(d => d.DocumentId == documentId);
        if (existingDoc != null)
        {
            await DeleteChunksForDocumentIdAsync(documentId);
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
        var chunks = new List<IngestedChunk>();

        foreach (var p in paragraphs)
        {
            var embedding = await embeddingGenerator.GenerateAsync(p.Text);
            chunks.Add(new IngestedChunk
            {
                Key = Guid.CreateVersion7().ToString(),
                DocumentId = documentId,
                PageNumber = p.PageNumber,
                Text = p.Text,
                Vector = embedding.Vector
            });
        }

        if (chunks.Any())
        {
            await chunksCollection.UpsertAsync(chunks);
        }

        logger.LogInformation("? PDF '{DocumentId}' ingested with {ChunkCount} chunks", documentId, chunks.Count);
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

            foreach (var subParagraph in SplitIntoChunks(cleanedText, 200))
            {
                allChunks.Add((pdfPage.Number, chunkIndex++, subParagraph));
            }
        }

        return allChunks;
    }

    private static List<string> SplitIntoChunks(string text, int maxWords)
    {
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (words.Length <= maxWords) return [text];

        var chunks = new List<string>();
        var sb = new System.Text.StringBuilder();
        int count = 0;
        foreach (var word in words)
        {
            if (count > 0 && count >= maxWords)
            {
                chunks.Add(sb.ToString().TrimEnd());
                sb.Clear();
                count = 0;
            }
            sb.Append(word).Append(' ');
            count++;
        }
        if (sb.Length > 0) chunks.Add(sb.ToString().TrimEnd());
        return chunks;
    }

    private async Task DeleteChunksForDocumentIdAsync(string documentId)
    {
        var allChunks = await chunksCollection.GetAsync(_ => true, 1000).ToListAsync();
        var chunksToDelete = allChunks.Where(r => r.DocumentId == documentId).ToList();
        if (chunksToDelete.Any())
        {
            await chunksCollection.DeleteAsync(chunksToDelete.Select(r => r.Key));
        }
    }
}
