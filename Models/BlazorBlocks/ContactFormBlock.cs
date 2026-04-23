using BrixCMS.Open.Data.Fields;
using BrixCMS.Open.Models.Base;

namespace BrixCMS.Open.Models.BlazorBlocks
{
    [BlockType(Name = "Contact Form", Category = "Content", Icon = "fas fa-envelope", Description = "Interactive contact form with email delivery. Fully customizable fields and confirmation message.")]
    public class ContactFormBlock : BlockBase
    {
        [Field(Title = "Título del Formulario")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Email de Destino", Placeholder = "admin@tuweb.com")]
        public StringField RecipientEmail { get; set; } = new();

        //Button

        [Field]
        public StringField Text { get; set; }

        [Field(Title = "Link", Placeholder = "Destination URL")]
        public StringField Url { get; set; }

        [Field]
        public ColorField Color { get; set; }

        [Field(Title = "Button Hover Color")]
        public ColorField HoverColor { get; set; }

        [Field(Title = "Border Radius", Placeholder = "fas fa-font")]
        public StringField BorderRadius { get; set; }

        [Field(Title = "Button Border", Placeholder = "e.g., '2px solid #000'")]
        public StringField Border { get; set; }


        [Field(Title = "Width")]
        public StringField Width { get; set; }

        [Field(Title = "Padding", Placeholder = "fas fa-font")]
        public StringField Padding { get; set; }

        [Field(Title = "Text Color", Placeholder = "Color for the button text")]
        public ColorField TextColor { get; set; }

        [Field(Title = "Button Position", Placeholder = "Position of the button (e.g., 'left', 'center', 'right')")]
        public StringField ButtonPosition { get; set; }
    }
}
