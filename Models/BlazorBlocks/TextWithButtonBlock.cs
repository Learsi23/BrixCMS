using BrixCMS.Open.Data.Fields;
using BrixCMS.Open.Models.Base;

namespace BrixCMS.Open.Models.BlazorBlocks
{
    [BlockType(Name = "Text + Button", Category = "Content", Icon = "fas fa-align-left", Description = "Text content paired with a CTA button. Clean layout for announcements, offers, or feature highlights.")]
    public class TextWithButtonBlock : BlockBase
    {
        // --- CONFIGURACIÓN GLOBAL DEL BLOQUE ---
        [Field(Title = "Global Font Family", Placeholder = "e.g., 'Roboto', 'Open Sans'")]
        public StringField GlobalFontFamily { get; set; }

        [Field(Title = "Text Alignment", Placeholder = "left, center, right")]
        public StringField TextAlign { get; set; } = new() { Value = "left" };

        [Field(
            Title = "Enable Hover Zoom Animation?",
            Placeholder = "Type 'yes' to activate",
            Description = "Write 'yes' or 'true' to enable the scale effect on hover."
        )]
        public CheckBoxField EnableHoverScale { get; set; } = new();

        // --- TÍTULO ---
        [Field(Title = "Title", Placeholder = "Enter the title")]
        public StringField Title { get; set; }

        [Field(Title = "Title Color")]
        public ColorField TitleColor { get; set; }

        [Field(Title = "Title Size", Placeholder = "e.g., '24px'")]
        public StringField TitleSize { get; set; }

        [Field(Title = "Title Weight", Placeholder = "e.g., 'bold', '700', '300'")]
        public StringField TitleWeight { get; set; }

        // --- SUBTÍTULO ---
        [Field(Title = "Subtitle", Placeholder = "Enter the subtitle")]
        public StringField Subtitle { get; set; }

        [Field(Title = "Subtitle Color")]
        public ColorField SubtitleColor { get; set; }

        [Field(Title = "Subtitle Size", Placeholder = "e.g., '20px'")]
        public StringField SubtitleSize { get; set; }

        [Field(Title = "Subtitle Weight", Placeholder = "e.g., 'normal', '400'")]
        public StringField SubtitleWeight { get; set; }

        // --- DESCRIPCIÓN ---
        [Field(Title = "Description", Placeholder = "Enter the description")]
        public TextAreaField Description { get; set; }

        [Field(Title = "Description Color")]
        public ColorField DescriptionColor { get; set; }

        [Field(Title = "Description Size", Placeholder = "e.g., '16px'")]
        public StringField DescriptionSize { get; set; }

        [Field(Title = "Description Weight", Placeholder = "e.g., '400'")]
        public StringField DescriptionWeight { get; set; }

        // --- BOTÓN ---
        [Field(Title = "Button Text", Placeholder = "Enter button text")]
        public StringField ButtonText { get; set; }

        [Field(Title = "Button Link", Placeholder = "Enter button link")]
        public StringField ButtonUrl { get; set; }

        [Field(Title = "Button Color")]
        public ColorField ButtonColor { get; set; }

        [Field(Title = "Button Hover Color")]
        public ColorField ButtonHoverColor { get; set; }

        [Field(Title = "Button Border Radius", Placeholder = "e.g., '10px'")]
        public StringField ButtonBorderRadius { get; set; }

        [Field(Title = "Button Border", Placeholder = "e.g., '2px solid #000'")]
        public StringField ButtonBorder { get; set; }

        [Field(Title = "Button Width")]
        public StringField ButtonWidth { get; set; }

        [Field(Title = "Button Padding", Placeholder = "e.g., '10px'")]
        public StringField ButtonPadding { get; set; }

        [Field(Title = "Button Text Color")]
        public ColorField ButtonTextColor { get; set; }


        [Field(Title = "Button Position", Placeholder = "left, center, right")]
        public StringField ButtonPosition { get; set; }
    }
}
