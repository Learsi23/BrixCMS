using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using System.Linq;
using System.Linq.Expressions;

namespace BrixCMS.Open.Services;

public class SemanticSearch(
    VectorStoreCollection<string, IngestedChunk> vectorCollection,
    VectorStoreCollection<string, IngestedDocument> documentsCollection)
{
    public async Task<IReadOnlyList<IngestedChunk>> SearchAsync(string text, string? documentIdFilter, int maxResults)
    {
        // Support single filename OR comma-separated list ("manual.pdf, faq.pdf")
        var filters = (documentIdFilter ?? "")
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(f => f.Length > 0)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        Expression<Func<IngestedChunk, bool>>? filterExpr = filters.Count switch
        {
            0 => null,
            1 => record => record.DocumentId == documentIdFilter,
            _ => null   // multi-file: fetch unfiltered then filter in memory below
        };

        var nearest = vectorCollection.SearchAsync(text, filters.Count > 1 ? maxResults * 3 : maxResults,
            new VectorSearchOptions<IngestedChunk> { Filter = filterExpr });

        var results = await nearest.Select(r => r.Record).ToListAsync();

        // Apply multi-file filter in memory when more than one file is specified
        if (filters.Count > 1)
            results = results.Where(r => filters.Contains(r.DocumentId)).Take(maxResults).ToList();

        return results;
    }

    /// <summary>Returns the distinct DocumentIds of all ingested documents.</summary>
    public async Task<List<string>> GetAllDocumentIdsAsync()
    {
        var docs = await documentsCollection.GetAsync(_ => true, 500).ToListAsync();
        return docs
            .Select(d => d.DocumentId)
            .Distinct()
            .OrderBy(x => x)
            .ToList();
    }

    public async Task DeleteDocumentAsync(string documentId)
    {
        var chunksToDelete = await vectorCollection.GetAsync(_ => true, 1000).ToListAsync();
        var filteredChunks = chunksToDelete.Where(r => r.DocumentId == documentId).ToList();
        
        if (filteredChunks.Any())
        {
            await vectorCollection.DeleteAsync(filteredChunks.Select(r => r.Key));
        }

        var docToDelete = await documentsCollection.GetAsync(_ => true, 100).ToListAsync();
        var filteredDoc = docToDelete.FirstOrDefault(d => d.DocumentId == documentId);
        if (filteredDoc != null)
        {
            await documentsCollection.DeleteAsync(filteredDoc.Key);
        }
    }
}
