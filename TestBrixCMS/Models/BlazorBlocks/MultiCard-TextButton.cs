using TestBrixCMS.Data.Fields;
using TestBrixCMS.Models.Base;

namespace TestBrixCMS.Models.BlazorBlocks
{
    [BlockType(Name = "Multi Card + Button", Category = "Content", Icon = "fas fa-layer-group", Description = "Multiple content cards each with text and a button. Great for services, pricing, or feature comparisons.")]
    public class MultiCard_TextButton : BlockBase
    {
        // --- CONFIGURACIÓN DEL CONTENEDOR (CARD) ---
        [Field(Title = "Global Font Family", Placeholder = "e.g., 'Roboto'")]
        public StringField GlobalFontFamily { get; set; }

        [Field(Title = "Text Alignment", Placeholder = "left, center, right")]
        public StringField TextAlign { get; set; } = new() { Value = "left" };

        [Field(
            Title = "Enable Hover Zoom Animation?",
            Placeholder = "Type 'yes' to activate",
            Description = "Write 'yes' or 'true' to enable the scale effect on hover."
        )]
        public CheckBoxField EnableHoverScale { get; set; } = new();

        [Field(Title = "Card Background Color")]
        public ColorField BackgroundColor { get; set; } = new();

        [Field(Title = "Card Hover Background Color")]
        public ColorField BackgroundHoverColor { get; set; } = new();

        [Field(Title = "Card Background Image")]
        public ImageField BackgroundImage { get; set; } = new();

        [Field(Title = "Card Padding")]
        public StringField Padding { get; set; } = new() { Value = "2rem" };

        [Field(Title = "Card Border Radius")]
        public StringField CardBorderRadius { get; set; } = new() { Value = "0px" };

        [Field(Title = "Card Border")]
        public StringField CardBorder { get; set; }

        // --- TÍTULO ---
        [Field(Title = "Title")]
        public StringField Title { get; set; }
        [Field(Title = "Title Color")]
        public ColorField TitleColor { get; set; }
        [Field(Title = "Hover Title Color")]
        public ColorField TitleHoverColor { get; set; }
        [Field(Title = "Title Size")]
        public StringField TitleSize { get; set; }
        [Field(Title = "Title Weight")]
        public StringField TitleWeight { get; set; }

        // --- SUBTÍTULO ---
        [Field(Title = "Subtitle")]
        public StringField Subtitle { get; set; }
        [Field(Title = "Subtitle Color")]
        public ColorField SubtitleColor { get; set; }
        [Field(Title = "Hover Subtitle Color")]
        public ColorField SubtitleHoverColor { get; set; }
        [Field(Title = "Subtitle Size")]
        public StringField SubtitleSize { get; set; }
        [Field(Title = "Subtitle Weight")]
        public StringField SubtitleWeight { get; set; }

        // --- TEXTO ADICIONAL ---
        [Field(Title = "Extra Text")]
        public StringField Text { get; set; }
        [Field(Title = "Extra Text Color")]
        public ColorField TextColor { get; set; }
        [Field(Title = "Hover Text Color")]
        public ColorField TextHoverColor { get; set; }
        [Field(Title = "Extra Text Size")]
        public StringField TextSize { get; set; }
        [Field(Title = "Extra Text Weight")]
        public StringField TextWeight { get; set; }

        // --- DESCRIPCIÓN ---
        [Field(Title = "Description")]
        public TextAreaField Description { get; set; }
        [Field(Title = "Description Color")]
        public ColorField DescriptionColor { get; set; }
        [Field(Title = "Hover Description Color")]
        public ColorField DescriptionHoverColor { get; set; }
        [Field(Title = "Description Size")]
        public StringField DescriptionSize { get; set; }
        [Field(Title = "Description Weight")]
        public StringField DescriptionWeight { get; set; }

        // --- BOTÓN ---
        [Field(Title = "Button Text")]
        public StringField ButtonText { get; set; }
        [Field(Title = "Button Link")]
        public StringField ButtonUrl { get; set; }
        [Field(Title = "Button Color")]
        public ColorField ButtonColor { get; set; }
        [Field(Title = "Button Hover Color")]
        public ColorField ButtonHoverColor { get; set; }
        [Field(Title = "Button Text Color")]
        public ColorField ButtonTextColor { get; set; }
        [Field(Title = "Button Border Radius")]
        public StringField ButtonBorderRadius { get; set; }
        [Field(Title = "Button Padding")]
        public StringField ButtonPadding { get; set; }
        [Field(Title = "Button Position")]
        public StringField ButtonPosition { get; set; }
    }
}
