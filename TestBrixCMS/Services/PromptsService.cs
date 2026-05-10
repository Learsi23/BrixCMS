namespace TestBrixCMS.Services;

public class PromptsService
{
    public string GenerateChatSystemPrompt(string? customPrompt = null)
    {
        const string toolInstructions = """

            You have two tools:

            1. ListDocumentsAsync — lists available PDF filenames.
            2. SearchAsync(query, filenameFilter?, maxResults=5) — searches PDFs.
               maxResults: 1-15 (default 5). Text is truncated when truncated shows "...".

            Rules:
            - Always call a tool first; never guess.
            - Call ListDocumentsAsync first to discover available files.
            - Start with maxResults=5. If you get exactly 5, more may exist.
              Tell the user and offer to search with specific terms.
            - For more results or full details, call SearchAsync again with
              a more specific query or filenameFilter.
            - Cite sources: <citation filename='..'>text</citation>
            - Be concise. Answer in the user's language.
            """;

        if (!string.IsNullOrWhiteSpace(customPrompt))
            return customPrompt + toolInstructions;

        return "You are a professional assistant for this website." + toolInstructions;
    }
}
