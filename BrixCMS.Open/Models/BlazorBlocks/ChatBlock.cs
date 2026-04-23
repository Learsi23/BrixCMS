using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.BlazorBlocks
{
    [BlockType(Name = "AI Assistant", Category = "AI", Icon = "fas fa-robot",
        Description = "Embeds an interactive AI chatbot. Supports per-block AI provider selection and optional PDF document restriction.")]
    public class ChatBlock
    {
        [Field(Title = "Color de Fondo del Bloque")]
        public ColorField BackgroundColor { get; set; } = new();

        [Field(Title = "Título del Chat", Placeholder = "Ej: Asistente Inteligente")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Color del Título")]
        public ColorField TitleColor { get; set; } = new();

        [Field(Title = "Tamaño del Título", Placeholder = "Ej: 24px")]
        public StringField TitleSize { get; set; } = new();

        [Field(Title = "Logo del Chat")]
        public ImageField Logo { get; set; } = new();

        [Field(Title = "Tamaño del Logo", Placeholder = "Ej: 24px")]
        public StringField LogoSize { get; set; } = new();

        [Field(Title = "Ai_Logo del Chat")]
        public ImageField Ai_Logo { get; set; } = new();

        [Field(Title = "Tamaño del Ai_Logo", Placeholder = "Ej: 24px")]
        public StringField Ai_LogoSize { get; set; } = new();

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
        [Field(Title = "Instrucciones IA (System Prompt — persona only)", Placeholder = "Dile a la IA cómo actuar...")]
        public StringField CustomPrompt { get; set; } = new();

        [Field(Title = "Mensaje de Bienvenida", Placeholder = "Ej: ¡Hola! ¿En qué puedo ayudarte?")]
        public StringField WelcomeMessage { get; set; } = new();

        [Field(Title = "Restrict PDF Search (select documents)")]
        public PdfSelectField PdfFiles { get; set; } = new();
    }
}
