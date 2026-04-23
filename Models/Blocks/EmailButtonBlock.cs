using BrixCMS.Open.Models.Base;
using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.Blocks.Email
{
    [BlockType(Name = "Email Button", Category = "Ecommerce", Icon = "fas fa-envelope-open-text", Description = "Button that triggers an email action (e.g. inquiry, quote request). Sends an email via your SMTP configuration.")]
    public class EmailButtonBlock : BlockBase
    {
        [Field(Title = "Button Text", Placeholder = "Ej: Contáctanos")]
        public StringField Text { get; set; } = new();

        [Field(Title = "Email Address", Placeholder = "Ej: info@ejemplo.com")]
        public StringField EmailAddress { get; set; } = new();

        [Field(Title = "Email Subject", Placeholder = "Ej: Consulta desde web")]
        public StringField Subject { get; set; } = new();

        [Field(Title = "Email Body", Placeholder = "Ej: Hola, me gustaría...")]
        public TextAreaField Body { get; set; } = new();

        [Field(Title = "Button Color")]
        public ColorField BackgroundColor { get; set; } = new() { Value = "#3b82f6" };

        [Field(Title = "Button Hover Color")]
        public ColorField HoverColor { get; set; } = new() { Value = "#2563eb" };

        [Field(Title = "Text Color")]
        public ColorField TextColor { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Border Radius", Placeholder = "Ej: 8px, 9999px")]
        public StringField BorderRadius { get; set; } = new() { Value = "8px" };

        [Field(Title = "Border", Placeholder = "Ej: 2px solid #000")]
        public StringField Border { get; set; } = new();

        [Field(Title = "Width", Placeholder = "Ej: 200px, 100%")]
        public StringField Width { get; set; } = new() { Value = "auto" };

        [Field(Title = "Padding", Placeholder = "Ej: 12px 24px")]
        public StringField Padding { get; set; } = new() { Value = "12px 24px" };

        [Field(Title = "Button Position",
               Description = "Alineación del botón dentro del contenedor")]
        public SelectField<string> Position { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "left", Label = "Izquierda", Icon = "fas fa-align-left" },
                new() { Value = "center", Label = "Centro", Icon = "fas fa-align-center" },
                new() { Value = "right", Label = "Derecha", Icon = "fas fa-align-right" }
            },
            Value = "center"
        };
    }
}
