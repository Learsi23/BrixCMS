using TestBrixCMS.Data.Fields;
using TestBrixCMS.Models.Base;

namespace TestBrixCMS.Models.Blocks
{
    [BlockType(Name = "Text Block", Category = "Content", Icon = "fas fa-font", Description = "Rich text block with title, subtitle, and body. Full control over size, color, and alignment.")]
    public class TextBlock : BlockBase
    {
        // ==================== CONTENIDO ====================

        [Field(Title = "Title")]
        public StringField Title { get; set; }

        [Field(Title = "Title Color")]
        public ColorField TitleColor { get; set; }

        [Field(Title = "Title Size", Placeholder = "24px, 1.5rem, text-4xl")]
        public StringField TitleSize { get; set; }

        [Field(Title = "Title Weight", Placeholder = "normal, bold, 600")]
        public StringField TitleWeight { get; set; }

        [Field(Title = "Title Alignment", Placeholder = "left, center, right")]
        public StringField TitleAlignment { get; set; } = new() { Value = "left" };

        // --- SUBTITLE ---

        [Field(Title = "Subtitle")]
        public StringField Subtitle { get; set; }

        [Field(Title = "Subtitle Color")]
        public ColorField SubtitleColor { get; set; }

        [Field(Title = "Subtitle Size", Placeholder = "18px, 1.25rem, text-xl")]
        public StringField SubtitleSize { get; set; }

        [Field(Title = "Subtitle Weight", Placeholder = "normal, bold, 500")]
        public StringField SubtitleWeight { get; set; }

        [Field(Title = "Subtitle Alignment", Placeholder = "left, center, right")]
        public StringField SubtitleAlignment { get; set; } = new() { Value = "left" };

        // --- BODY ---

        [Field(Title = "Body Content")]
        public TextAreaField Body { get; set; }

        [Field(Title = "Body Color")]
        public ColorField BodyColor { get; set; }

        [Field(Title = "Body Size", Placeholder = "16px, 1rem, text-base")]
        public StringField BodySize { get; set; }

        [Field(Title = "Body Weight", Placeholder = "normal, light, 400")]
        public StringField BodyWeight { get; set; }

        [Field(Title = "Body Alignment", Placeholder = "left, center, right, justify")]
        public StringField BodyAlignment { get; set; } = new() { Value = "left" };

        // ==================== ESPACIADO SIMPLIFICADO ====================

        [Field(Title = "Padding", Placeholder = "1rem, 20px, 2rem 0")]
        public StringField Padding { get; set; } = new() { Value = "1rem" };

        [Field(Title = "Margin", Placeholder = "0, 20px, 2rem 0")]
        public StringField Margin { get; set; } = new() { Value = "0" };

        [Field(Title = "Background Color")]
        public ColorField BackgroundColor { get; set; }

        [Field(Title = "Background Image")]
        public ImageField BackgroundImage { get; set; } = new();

        // ==================== POSICIONAMIENTO ====================

        [Field(Title = "Vertical Position", Description = "Position text from top to bottom", Placeholder = "start, center, end")]
        public StringField VerticalPosition { get; set; } = new() { Value = "start" };

        // Opciones: start (arriba), center (centro vertical), end (abajo)

        [Field(Title = "Horizontal Position", Description = "Position text from left to right", Placeholder = "start, center, end")]
        public StringField HorizontalPosition { get; set; } = new() { Value = "start" };
        // Opciones: start (izquierda), center (centro horizontal), end (derecha)

        // ==================== ALTO DEL CONTENEDOR ====================

        [Field(Title = "Container Height", Description = "Set a fixed height for vertical positioning to work", Placeholder = "400px, 50vh, 100%")]
        public StringField ContainerHeight { get; set; } = new() { Value = "auto" };
        // Para que el posicionamiento vertical funcione, necesitas un alto definido
        // Ejemplos: "400px", "50vh", "100%", "min-height: 400px"
    }
}
