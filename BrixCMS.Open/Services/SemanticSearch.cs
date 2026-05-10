using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using System.Linq.Expressions;

namespace BrixCMS.Open.Services;

public class SemanticSearch(
    VectorStoreCollection<string, IngestedChunk> chunksCollection,
    VectorStoreCollection<string, IngestedDocument> documentsCollection,
    IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
{
    public async Task<IReadOnlyList<IngestedChunk>> SearchAsync(string text, string? documentIdFilter, int maxResults)
    {
        var filters = (documentIdFilter ?? "")
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(f => f.Length > 0)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        Expression<Func<IngestedChunk, bool>>? filterExpr = filters.Count switch
        {
            0 => null,
            1 => record => record.DocumentId == filters.First(),
            _ => null
        };

        var options = new VectorSearchOptions<IngestedChunk> { Filter = filterExpr };

        // When search phrase is very short/empty, skip semantic search and return chunks
        // in document/page order for comprehensive listing
        if (string.IsNullOrWhiteSpace(text) || text.Length < 3)
        {
            var results = new List<IngestedChunk>();
            var dummy = new ReadOnlyMemory<float>(new float[384]);
            var searchCount = filters.Count > 1 ? maxResults * 3 : maxResults + 50;
            await foreach (var result in chunksCollection.SearchAsync(dummy, searchCount, options))
                results.Add(result.Record);

            if (filters.Count > 1)
                results = results.Where(r => filters.Contains(r.DocumentId)).Take(maxResults).ToList();

            return results.OrderBy(r => r.DocumentId).ThenBy(r => r.PageNumber).Take(maxResults).ToList();
        }

        var embedding = await embeddingGenerator.GenerateAsync(text);
        var queryVector = embedding.Vector;

        var rankedResults = new List<IngestedChunk>();
        var searchCount2 = filters.Count > 1 ? maxResults * 3 : maxResults;
        await foreach (var result in chunksCollection.SearchAsync(queryVector, searchCount2, options))
            rankedResults.Add(result.Record);

        if (filters.Count > 1)
            rankedResults = rankedResults.Where(r => filters.Contains(r.DocumentId)).Take(maxResults).ToList();

        return rankedResults;
    }

    public async Task<List<string>> GetAllDocumentIdsAsync()
    {
        var docs = new List<IngestedDocument>();
        var dummy = new ReadOnlyMemory<float>(new float[2]);
        await foreach (var result in documentsCollection.SearchAsync(dummy, 1000))
            docs.Add(result.Record);

        return docs.Select(d => d.DocumentId).Distinct().OrderBy(x => x).ToList();
    }

    public async Task DeleteDocumentAsync(string documentId)
    {
        var chunkIds = new List<string>();
        var dummy = new ReadOnlyMemory<float>(new float[384]);
        await foreach (var result in chunksCollection.SearchAsync(dummy, 10000))
        {
            if (result.Record.DocumentId == documentId)
                chunkIds.Add(result.Record.Key);
        }

        if (chunkIds.Count != 0)
            await chunksCollection.DeleteAsync(chunkIds);

        var docDummy = new ReadOnlyMemory<float>(new float[2]);
        await foreach (var result in documentsCollection.SearchAsync(docDummy, 1000))
        {
            if (result.Record.DocumentId == documentId)
            {
                await documentsCollection.DeleteAsync(result.Record.Key);
                break;
            }
        }
    }
}
