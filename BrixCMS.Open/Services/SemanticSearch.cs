using Microsoft.Extensions.VectorData;
using System.Linq.Expressions;

namespace BrixCMS.Open.Services;

public class SemanticSearch(
    VectorStoreCollection<string, IngestedChunk> chunksCollection,
    VectorStoreCollection<string, IngestedDocument> documentsCollection)
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
            1 => record => record.DocumentId == documentIdFilter,
            _ => null
        };

        var options = new VectorSearchOptions<IngestedChunk> { Filter = filterExpr };

        var results = new List<IngestedChunk>();
        var searchCount = filters.Count > 1 ? maxResults * 3 : maxResults;
        await foreach (var result in chunksCollection.SearchAsync(text, searchCount, options))
            results.Add(result.Record);

        if (filters.Count > 1)
            results = results.Where(r => filters.Contains(r.DocumentId)).Take(maxResults).ToList();

        return results;
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
        var chunks = new List<IngestedChunk>();
        var dummy = new ReadOnlyMemory<float>(new float[384]);
        await foreach (var result in chunksCollection.SearchAsync(dummy, 10000))
            chunks.Add(result.Record);

        var toDelete = chunks.Where(r => r.DocumentId == documentId).Select(r => r.Key).ToList();
        if (toDelete.Any())
            await chunksCollection.DeleteAsync(toDelete);

        var docs = new List<IngestedDocument>();
        await foreach (var result in documentsCollection.SearchAsync(dummy, 1000))
            docs.Add(result.Record);

        var doc = docs.FirstOrDefault(d => d.DocumentId == documentId);
        if (doc != null)
            await documentsCollection.DeleteAsync(doc.Key);
    }
}
