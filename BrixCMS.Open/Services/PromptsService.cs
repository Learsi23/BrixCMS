namespace BrixCMS.Open.Services;

public class PromptsService
{
    public string GenerateChatSystemPrompt(string? customPrompt = null)
    {
        const string toolInstructions = """

            ── TOOLS (MANDATORY USAGE) ──────────────────────────────────────────────────
            You have access to ONE tool. You MUST call it instead of guessing:

            1. SearchAsync — searches uploaded PDF documents for factual information.
                Call it whenever the user asks about services, policies, hours,
                menus, ingredients, or any topic that might be covered in the docs.

            Rules:
            • NEVER answer from your own training knowledge if the info could be in a tool.
            • ALWAYS call SearchAsync before discussing any specific topic covered in docs.
            • After calling SearchAsync, cite results: <citation filename='..'>quote</citation>
            • Be concise, friendly, and helpful.
            • Answer in the same language as the user.
            ─────────────────────────────────────────────────────────────────────────────
            """;

        if (!string.IsNullOrWhiteSpace(customPrompt))
            return customPrompt + toolInstructions;

        return "You are a professional assistant for this website." + toolInstructions;
    }
}
