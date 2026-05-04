using TestBrixCMS.Data.Fields;

namespace TestBrixCMS.Models.BlazorBlocks
{
    [BlockType(
        Name = "Floating Chat Button",
        Category = "AI",
        Icon = "fas fa-comment-dots",
        Description = "A modern floating chat button with glassmorphism panel, animated gradients, and fully customizable colors.")]
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

        // ── Button Colors ───────────────────────────────────────────────────────
        [Field(Title = "Button Primary Color")]
        public ColorField ButtonColor { get; set; } = new() { Value = "#6366F1" };

        [Field(Title = "Button Gradient End Color")]
        public ColorField ButtonGradientColor { get; set; } = new() { Value = "#8B5CF6" };

        [Field(Title = "Button Icon Color")]
        public ColorField ButtonTextColor { get; set; } = new() { Value = "#FFFFFF" };

        [Field(Title = "Button Icon (emoji or text)", Placeholder = "💬")]
        public StringField ButtonIcon { get; set; } = new() { Value = "💬" };

        [Field(Title = "Button Size (px)", Placeholder = "60px")]
        public StringField ButtonSize { get; set; } = new() { Value = "60px" };

        [Field(Title = "Button Glow Effect")]
        public BoolField ButtonGlow { get; set; } = new() { Value = "true" };

        // ── Panel Colors ────────────────────────────────────────────────────────
        [Field(Title = "Panel Background Color")]
        public ColorField PanelBgColor { get; set; } = new() { Value = "#0F172A" };

        [Field(Title = "Panel Header Background Start")]
        public ColorField HeaderBgColor { get; set; } = new() { Value = "#1E293B" };

        [Field(Title = "Panel Header Gradient End")]
        public ColorField HeaderGradientColor { get; set; } = new() { Value = "#6366F1" };

        [Field(Title = "Header Text Color")]
        public ColorField HeaderTextColor { get; set; } = new() { Value = "#F8FAFC" };

        [Field(Title = "Chat Background Color")]
        public ColorField ChatBgColor { get; set; } = new() { Value = "#0F172A" };

        // ── Message Colors ──────────────────────────────────────────────────────
        [Field(Title = "User Message Bubble Color")]
        public ColorField UserBubbleColor { get; set; } = new() { Value = "#6366F1" };

        [Field(Title = "User Message Text Color")]
        public ColorField UserTextColor { get; set; } = new() { Value = "#FFFFFF" };

        [Field(Title = "AI Message Bubble Color")]
        public ColorField AiBubbleColor { get; set; } = new() { Value = "#1E293B" };

        [Field(Title = "AI Message Text Color")]
        public ColorField AiTextColor { get; set; } = new() { Value = "#E2E8F0" };

        // ── Input Colors ────────────────────────────────────────────────────────
        [Field(Title = "Input Background Color")]
        public ColorField InputBgColor { get; set; } = new() { Value = "#1E293B" };

        [Field(Title = "Input Text Color")]
        public ColorField InputTextColor { get; set; } = new() { Value = "#F8FAFC" };

        [Field(Title = "Input Border Color")]
        public ColorField InputBorderColor { get; set; } = new() { Value = "#334155" };

        [Field(Title = "Send Button Color")]
        public ColorField SendButtonColor { get; set; } = new() { Value = "#6366F1" };

        // ── AI provider ───────────────────────────────────────────────────────────
        [Field(Title = "AI Provider")]
        public SelectField<string> AiProvider { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "auto",   Label = "Auto (site default)" },
                new() { Value = "ollama", Label = "Ollama (local)"      },
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

        [Field(Title = "Logo Size", Placeholder = "28px")]
        public StringField LogoSize { get; set; } = new() { Value = "28px" };

        [Field(Title = "AI Avatar Logo")]
        public ImageField AiLogo { get; set; } = new();

        [Field(Title = "AI Avatar Size", Placeholder = "28px")]
        public StringField AiLogoSize { get; set; } = new() { Value = "28px" };
    }
}
