using TestBrixCMS.Models.Base;
using TestBrixCMS.Data.Fields;

namespace TestBrixCMS.Models.Blocks
{
    [BlockType(Name = "Dropdown / FAQ", Category = "Content", Icon = "fas fa-chevron-down", Description = "Collapsible accordion sections. Perfect for FAQs, specifications, or grouped content.")]
    public class DropdownBlock : BlockBase
    {
        [Field(Title = "Título del Bloque", Placeholder = "Escribe el título (opcional)")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Color del Título")]
        public ColorField TitleColor { get; set; } = new();

        [Field(Title = "Tamaño del Título", Placeholder = "Ej: 24px, 2rem, text-3xl")]
        public StringField TitleSize { get; set; } = new();

        [Field(Title = "Color de Fondo del Bloque")]
        public ColorField BackgroundColor { get; set; } = new();

        [Field(Title = "Gradiente CSS (opcional)",
               Placeholder = "Ej: linear-gradient(45deg, #ff0000, #00ff00)",
               Description = "Si se especifica, reemplaza el color de fondo")]
        public StringField BackgroundGradient { get; set; } = new();

        [Field(Title = "Pregunta del Dropdown", Placeholder = "Escribe la pregunta")]
        public StringField Question { get; set; } = new();

        [Field(Title = "Color de la Pregunta")]
        public ColorField QuestionColor { get; set; } = new();

        [Field(Title = "Tamaño de la Pregunta", Placeholder = "Ej: 18px, 1.2rem")]
        public StringField QuestionSize { get; set; } = new();

        [Field(Title = "Respuesta (HTML)", Placeholder = "Escribe el contenido")]
        public StringField Answer { get; set; } = new(); // Usamos StringField con Html.Raw

        [Field(Title = "Color de la Respuesta")]
        public ColorField AnswerColor { get; set; } = new();

        [Field(Title = "Tamaño de la Respuesta", Placeholder = "Ej: 16px, 1rem")]
        public StringField AnswerSize { get; set; } = new();

        [Field(Title = "Color de Fondo del Dropdown")]
        public ColorField DropdownBackgroundColor { get; set; } = new();

        [Field(Title = "¿Abierto por defecto?")]
        public BoolField OpenByDefault { get; set; } = new();
    }
}
