using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.BlazorBlocks
{
    [BlockType(Name = "AI Assistant", Category = "AI", Icon = "fas fa-robot",
        Description = "Modern embedded AI chatbot with glassmorphism design, animated gradients, and fully customizable colors.")]
    public class ChatBlock
    {
        // ── Block Appearance ────────────────────────────────────────────────────
        [Field(Title = "Block Background Color")]
        public ColorField BackgroundColor { get; set; } = new() { Value = "#FFFFFF" };

        [Field(Title = "Block Border Color")]
        public ColorField BorderColor { get; set; } = new() { Value = "#E2E8F0" };

        [Field(Title = "Block Border Radius", Placeholder = "16px")]
        public StringField BorderRadius { get; set; } = new() { Value = "16px" };

        // ── Header ──────────────────────────────────────────────────────────────
        [Field(Title = "Title")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Header Background Color")]
        public ColorField HeaderBgColor { get; set; } = new() { Value = "#6366F1" };

        [Field(Title = "Header Gradient End Color")]
        public ColorField HeaderGradientColor { get; set; } = new() { Value = "#8B5CF6" };

        [Field(Title = "Header Text Color")]
        public ColorField HeaderTextColor { get; set; } = new() { Value = "#FFFFFF" };

        [Field(Title = "Title Size", Placeholder = "18px")]
        public StringField TitleSize { get; set; } = new() { Value = "18px" };

        // ── Logos ───────────────────────────────────────────────────────────────
        [Field(Title = "Header Logo")]
        public ImageField Logo { get; set; } = new();

        [Field(Title = "Logo Size", Placeholder = "28px")]
        public StringField LogoSize { get; set; } = new() { Value = "28px" };

        [Field(Title = "AI Avatar Logo")]
        public ImageField Ai_Logo { get; set; } = new();

        [Field(Title = "AI Avatar Size", Placeholder = "28px")]
        public StringField Ai_LogoSize { get; set; } = new() { Value = "28px" };

        // ── Chat Area ───────────────────────────────────────────────────────────
        [Field(Title = "Chat Background Color")]
        public ColorField ChatBgColor { get; set; } = new() { Value = "#F8FAFC" };

        // ── Messages ────────────────────────────────────────────────────────────
        [Field(Title = "User Message Bubble Color")]
        public ColorField UserBubbleColor { get; set; } = new() { Value = "#6366F1" };

        [Field(Title = "User Message Text Color")]
        public ColorField UserTextColor { get; set; } = new() { Value = "#FFFFFF" };

        [Field(Title = "AI Message Bubble Color")]
        public ColorField AiBubbleColor { get; set; } = new() { Value = "#FFFFFF" };

        [Field(Title = "AI Message Text Color")]
        public ColorField AiTextColor { get; set; } = new() { Value = "#1E293B" };

        [Field(Title = "AI Bubble Border Color")]
        public ColorField AiBubbleBorderColor { get; set; } = new() { Value = "#E2E8F0" };

        // ── Input Area ──────────────────────────────────────────────────────────
        [Field(Title = "Input Background Color")]
        public ColorField InputBgColor { get; set; } = new() { Value = "#FFFFFF" };

        [Field(Title = "Input Text Color")]
        public ColorField InputTextColor { get; set; } = new() { Value = "#0F172A" };

        [Field(Title = "Input Border Color")]
        public ColorField InputBorderColor { get; set; } = new() { Value = "#CBD5E1" };

        [Field(Title = "Input Placeholder Color")]
        public ColorField InputPlaceholderColor { get; set; } = new() { Value = "#94A3B8" };

        [Field(Title = "Send Button Color")]
        public ColorField SendButtonColor { get; set; } = new() { Value = "#6366F1" };

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
        [Field(Title = "Custom System Prompt (persona only)", Placeholder = "Dile a la IA cómo actuar...")]
        public StringField CustomPrompt { get; set; } = new();

        [Field(Title = "Welcome Message", Placeholder = "¡Hola! ¿En qué puedo ayudarte?")]
        public StringField WelcomeMessage { get; set; } = new();

        [Field(Title = "Restrict PDF Search (select documents)")]
        public PdfSelectField PdfFiles { get; set; } = new();
    }
}
