using BrixCMS.Open.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BrixCMS.Open.Services;

public record ApiKeyInfo(string Provider, bool HasKey, DateTime? UpdatedAt);
public record AiProviderConfig(string Provider, string Model);

public class ApiKeyService
{
    private readonly BrixDbContext _db;
    private readonly EncryptionService _encryption;

    private static readonly Dictionary<string, (string Endpoint, string DefaultModel)> Providers = new()
    {
        ["gemini"]   = ("https://generativelanguage.googleapis.com/v1beta/openai/", "gemini-2.5-flash-lite"),
        ["deepseek"] = ("https://api.deepseek.com/v1",   "deepseek-chat"),
        ["mistral"]  = ("https://api.mistral.ai/v1",     "mistral-small-latest"),
    };

    public ApiKeyService(BrixDbContext db, EncryptionService encryption)
    {
        _db         = db;
        _encryption = encryption;
    }

    public async Task<List<ApiKeyInfo>> GetAllAsync()
    {
        var keys = await _db.ApiKeys.ToListAsync();
        return keys.Select(k => new ApiKeyInfo(k.Provider, true, k.UpdatedAt)).ToList();
    }

    public async Task SaveKeyAsync(string provider, string plaintextKey)
    {
        var (encryptedKey, iv, authTag) = _encryption.Encrypt(plaintextKey);
        var existing = await _db.ApiKeys.FirstOrDefaultAsync(k => k.Provider == provider);

        if (existing != null)
        {
            existing.EncryptedKey = encryptedKey;
            existing.Iv           = iv;
            existing.AuthTag      = authTag;
            existing.UpdatedAt    = DateTime.UtcNow;
        }
        else
        {
            _db.ApiKeys.Add(new ApiKey
            {
                Provider     = provider,
                EncryptedKey = encryptedKey,
                Iv           = iv,
                AuthTag      = authTag,
                CreatedAt    = DateTime.UtcNow,
                UpdatedAt    = DateTime.UtcNow,
            });
        }
        await _db.SaveChangesAsync();
    }

    public async Task DeleteKeyAsync(string provider)
    {
        var key = await _db.ApiKeys.FirstOrDefaultAsync(k => k.Provider == provider);
        if (key == null) return;
        _db.ApiKeys.Remove(key);
        await _db.SaveChangesAsync();
    }

    public string? GetDecryptedKeySync(string provider)
    {
        var key = _db.ApiKeys.FirstOrDefault(k => k.Provider == provider);
        if (key == null) return null;
        try { return _encryption.Decrypt(key.EncryptedKey, key.Iv, key.AuthTag); }
        catch { return null; }
    }

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

    public (string provider, string endpoint, string model)? ResolveActiveClient()
    {
        var active = GetActiveProviderConfig();

        if (active?.Provider == "ollama")
            return ("ollama", GetOllamaUrl(), active.Model);

        if (active != null && Providers.TryGetValue(active.Provider, out var info))
        {
            var key = GetDecryptedKeySync(active.Provider);
            if (key != null) return (active.Provider, info.Endpoint, active.Model);
        }

        foreach (var (provider, (endpoint, defaultModel)) in Providers)
        {
            var key = GetDecryptedKeySync(provider);
            if (key != null) return (provider, endpoint, defaultModel);
        }

        return null;
    }

    public string MaskKey(string plaintextKey) => _encryption.MaskKey(plaintextKey);
    public static IReadOnlyDictionary<string, (string Endpoint, string DefaultModel)> KnownProviders => Providers;
}
