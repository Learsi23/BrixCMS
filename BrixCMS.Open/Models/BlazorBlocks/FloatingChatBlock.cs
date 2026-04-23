using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.BlazorBlocks
{
    [BlockType(
        Name = "Floating Chat Button",
        Category = "AI",
        Icon = "fas fa-comment-dots",
        Description = "A floating chat button (bottom-left or bottom-right) with configurable AI provider, optional PDF document restriction, and product catalogue access.")]
    public class FloatingChatBlock
    {
        [Field(Title = "Position")]
        public SelectField<string> Position { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "right", Label = "Right ▶" },
                new() { Value = "left",  Label = "◀ Left"  },
            },
            Value = "right"
        };

        [Field(Title = "Button Background Color")]
        public ColorField ButtonColor { get; set; } = new() { Value = "#2563EB" };

        [Field(Title = "Button Icon Color")]
        public ColorField ButtonTextColor { get; set; } = new() { Value = "#FFFFFF" };

        [Field(Title = "Button Icon (emoji or text)", Placeholder = "💬")]
        public StringField ButtonIcon { get; set; } = new() { Value = "💬" };

        [Field(Title = "Button Size (px)", Placeholder = "56px")]
        public StringField ButtonSize { get; set; } = new() { Value = "56px" };

        // ── AI provider ───────────────────────────────────────────────────────────
        [Field(Title = "AI Provider")]
        public SelectField<string> AiProvider { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "auto",     Label = "Auto (site default)" },
                new() { Value = "ollama",   Label = "Ollama (local)"      },
                new() { Value = "gemini",   Label = "Gemini"              },
                new() { Value = "deepseek", Label = "DeepSeek"            },
                new() { Value = "mistral",  Label = "Mistral"             },
            },
            Value = "auto"
        };

        // ── Chat behaviour ────────────────────────────────────────────────────────
        [Field(Title = "Custom System Prompt (persona only)", Placeholder = "You are a helpful assistant for this website...")]
        public StringField CustomPrompt { get; set; } = new();

        [Field(Title = "Welcome Message", Placeholder = "Hi! How can I help you today?")]
        public StringField WelcomeMessage { get; set; } = new();

        [Field(Title = "Restrict PDF Search (select documents)")]
        public PdfSelectField PdfFiles { get; set; } = new();

        // ── Branding ──────────────────────────────────────────────────────────────
        [Field(Title = "Chat Logo")]
        public ImageField Logo { get; set; } = new();

        [Field(Title = "Logo Size", Placeholder = "32px")]
        public StringField LogoSize { get; set; } = new() { Value = "32px" };

        [Field(Title = "AI Avatar Logo")]
        public ImageField AiLogo { get; set; } = new();

        [Field(Title = "AI Avatar Size", Placeholder = "24px")]
        public StringField AiLogoSize { get; set; } = new() { Value = "24px" };
    }
}
