using Microsoft.Extensions.AI;
using OllamaSharp;
using OpenAI;
using System.ClientModel;

namespace BrixCMS.Open.Services;

public interface IAiClientFactory
{
    (IChatClient Client, string Provider, string Model) BuildJsonClient();
    ChatOptions JsonChatOptions { get; }
}

public sealed class AiClientFactory : IAiClientFactory
{
    private readonly ApiKeyService _apiKeys;
    private readonly IConfiguration _cfg;
    private readonly ILogger<AiClientFactory> _logger;

    public AiClientFactory(ApiKeyService apiKeys, IConfiguration cfg, ILogger<AiClientFactory> logger)
    {
        _apiKeys = apiKeys;
        _cfg = cfg;
        _logger = logger;
    }

    public ChatOptions JsonChatOptions => new()
    {
        Temperature = 0.1f,
        ResponseFormat = ChatResponseFormat.Json,
    };

    public (IChatClient Client, string Provider, string Model) BuildJsonClient()
    {
        var resolved = _apiKeys.ResolveActiveClient();
        if (resolved.HasValue)
        {
            var (provider, endpoint, model) = resolved.Value;
            var key = _apiKeys.GetDecryptedKeySync(provider);
            if (!string.IsNullOrEmpty(key))
            {
                _logger.LogInformation("AI client: {Provider}/{Model}", provider, model);
                var oai = new OpenAIClient(
                    new ApiKeyCredential(key),
                    new OpenAIClientOptions { Endpoint = new Uri(endpoint) });
                return (oai.GetChatClient(model).AsIChatClient(), provider, model);
            }
        }

        var ollamaUrl = _cfg["Ollama:BaseUrl"] ?? "http://localhost:11434";
        var chatModel = _cfg["Ollama:ChatModel"] ?? "llama3.1:8b";
        _logger.LogWarning("AI client fallback: Ollama {Model}", chatModel);
        return ((IChatClient)new OllamaApiClient(new Uri(ollamaUrl), chatModel), "Ollama", chatModel);
    }
}
