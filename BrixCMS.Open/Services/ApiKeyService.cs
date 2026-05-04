using BrixCMS.Open.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BrixCMS.Open.Services;

public record AiProviderConfig(string Provider, string Model);

public class ApiKeyService
{
    private readonly BrixDbContext _db;

    public ApiKeyService(BrixDbContext db)
    {
        _db = db;
    }

    // ── Active provider preference (stored in SiteConfig key "ai-provider") ───
    public AiProviderConfig? GetActiveProviderConfig()
    {
        var cfg = _db.SiteConfig.FirstOrDefault(c => c.Key == "ai-provider");
        if (cfg?.JsonData == null) return null;
        try
        {
            return JsonSerializer.Deserialize<AiProviderConfig>(cfg.JsonData,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch { return null; }
    }

    public async Task SetActiveProviderAsync(string provider, string model)
    {
        var json = JsonSerializer.Serialize(new AiProviderConfig(provider, model));
        var cfg  = await _db.SiteConfig.FirstOrDefaultAsync(c => c.Key == "ai-provider");

        if (cfg != null)
        {
            cfg.JsonData = json;
        }
        else
        {
            _db.SiteConfig.Add(new SiteConfig { Id = 0, Key = "ai-provider", JsonData = json });
        }
        await _db.SaveChangesAsync();
    }

    // ── Ollama URL (stored in SiteConfig key "ollama-url") ───────────────────
    public string GetOllamaUrl()
    {
        var cfg = _db.SiteConfig.FirstOrDefault(c => c.Key == "ollama-url");
        if (cfg?.JsonData == null) return "http://localhost:11434";
        try { return JsonSerializer.Deserialize<string>(cfg.JsonData) ?? "http://localhost:11434"; }
        catch { return "http://localhost:11434"; }
    }

    public async Task SaveOllamaUrlAsync(string url)
    {
        var cfg = await _db.SiteConfig.FirstOrDefaultAsync(c => c.Key == "ollama-url");
        var json = JsonSerializer.Serialize(url.Trim());
        if (cfg != null)
            cfg.JsonData = json;
        else
            _db.SiteConfig.Add(new SiteConfig { Id = 0, Key = "ollama-url", JsonData = json });
        await _db.SaveChangesAsync();
    }

    // ── Resolve Ollama config ─────────────────────────────────────────────────
    public (string provider, string endpoint, string model)? ResolveActiveClient()
    {
        var active = GetActiveProviderConfig();
        var model  = active?.Model ?? "llama3.2:3b";
        return ("ollama", GetOllamaUrl(), model);
    }
}
