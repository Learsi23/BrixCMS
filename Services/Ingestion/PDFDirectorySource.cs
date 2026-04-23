using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis.PageSegmenter;
using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;

namespace BrixCMS.Open.Services.Ingestion;

public class PDFDirectorySource(
    string sourceDirectory,
    IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator) : IIngestionSource
{
    // Generar un ID único para el archivo basado en su nombre
    public static string SourceFileId(string path) => Path.GetFileName(path);

    // Generar una versión basada en la fecha de última modificación
    public static string SourceFileVersion(string path) => File.GetLastWriteTimeUtc(path).ToString("o");

    public string SourceId => $"{nameof(PDFDirectorySource)}:{sourceDirectory}";

    public Task<IEnumerable<IngestedDocument>> GetNewOrModifiedDocumentsAsync(IReadOnlyList<IngestedDocument> existingDocuments)
    {
        var results = new List<IngestedDocument>();

        if (!Directory.Exists(sourceDirectory))
        {
            return Task.FromResult(Enumerable.Empty<IngestedDocument>());
        }

        var sourceFiles = Directory.GetFiles(sourceDirectory, "*.pdf");

        // Agrupar existentes por ID para comparar versiones
        var existingDocumentsById = existingDocuments
            .GroupBy(d => d.DocumentId)
            .ToDictionary(
                g => g.Key,
                g => g.OrderByDescending(d => d.DocumentVersion).First()
            );

        foreach (var sourceFile in sourceFiles)
        {
            var sourceFileId = SourceFileId(sourceFile);
            var sourceFileVersion = SourceFileVersion(sourceFile);

            var existingDocumentVersion = existingDocumentsById.TryGetValue(sourceFileId, out var existingDocument)
                ? existingDocument.DocumentVersion
                : null;

            if (existingDocumentVersion != sourceFileVersion)
            {
                results.Add(new IngestedDocument
                {
                    Key = Guid.CreateVersion7().ToString(),
                    SourceId = SourceId,
                    DocumentId = sourceFileId,
                    DocumentVersion = sourceFileVersion
                });
            }
        }

        return Task.FromResult((IEnumerable<IngestedDocument>)results);
    }

    public Task<IEnumerable<IngestedDocument>> GetDeletedDocumentsAsync(IReadOnlyList<IngestedDocument> existingDocuments)
    {
        var currentFiles = Directory.GetFiles(sourceDirectory, "*.pdf");
        var currentFileIds = currentFiles.Select(SourceFileId).ToHashSet();

        var deletedDocuments = existingDocuments.Where(d => !currentFileIds.Contains(d.DocumentId));
        return Task.FromResult(deletedDocuments);
    }

    public async Task<IEnumerable<IngestedChunk>> CreateChunksForDocumentAsync(IngestedDocument document)
    {
        var filePath = Path.Combine(sourceDirectory, document.DocumentId);

        // Abrir el documento PDF
        using var pdf = PdfDocument.Open(filePath);

        // Extraer y fragmentar el texto de todas las páginas
        var paragraphs = pdf.GetPages().SelectMany(GetPageParagraphs).ToList();

        var chunks = new List<IngestedChunk>();

        foreach (var p in paragraphs)
        {
            // ? GENERAR VECTOR: Esto evita el error de SQLite (NULL)
            // El texto ya viene fragmentado por GetPageParagraphs (max 200 chars)
            var embedding = await embeddingGenerator.GenerateAsync(p.Text);

            chunks.Add(new IngestedChunk
            {
                Key = Guid.CreateVersion7().ToString(),
                DocumentId = document.DocumentId,
                PageNumber = p.PageNumber,
                Text = p.Text,
                Vector = embedding.Vector // Asignamos el vector generado por Ollama/AI Service
            });
        }

        return chunks;
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

            // Si el bloque es muy grande, el TextChunker lo dividirá en trozos de 200 tokens
            // Si el bloque es pequeño, lo dejará igual.
#pragma warning disable SKEXP0050
            var subParagraphs = TextChunker.SplitPlainTextParagraphs([cleanedText], 200);

            foreach (var subParagraph in subParagraphs)
            {
                allChunks.Add((pdfPage.Number, chunkIndex++, subParagraph));
            }
#pragma warning restore SKEXP0050
        }

        return allChunks;
    }
}
