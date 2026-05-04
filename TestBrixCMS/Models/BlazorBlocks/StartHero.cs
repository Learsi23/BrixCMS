using TestBrixCMS.Data.Fields;
using TestBrixCMS.Models.Base;

namespace TestBrixCMS.Models.BlazorBlocks
{
    // Start Hero Block Definition
    [BlockType(Name = "Start Hero", Category = "Headers", Icon = "fas fa-image", Description = "Animated hero with background image, gradient overlay, title, subtitle, and CTA button.")]
    public class StartHeroBlock : BlockBase
    {
        // --- Titles ---
        [Field(Title = "Top Title", Placeholder = "e.g., Welcome to...")]
        public StringField? TitleSida { get; set; }

        [Field(Title = "Top Title Color", Placeholder = "#000000")]
        public ColorField? TitleSidaColor { get; set; }

        [Field(Title = "Top Title Size", Placeholder = "e.g., 32px")]
        public StringField? TitleSidaSize { get; set; }

        [Field(Title = "Title sida Weight", Placeholder = "normal, bold, 600")]
        public StringField TitleSidaWeight { get; set; } = new() { Value = "normal" };

        [Field(Title = "Title sida Alignment", Placeholder = "left, center, right")]
        public StringField TitleSidaAlignment { get; set; } = new() { Value = "left" };


        [Field(Title = "Main Title", Placeholder = "e.g., Your Main Heading")]
        public StringField? Title { get; set; }

        [Field(Title = "Main Title Color", Placeholder = "#000000")]
        public ColorField? TitleColor { get; set; }

        [Field(Title = "Main Title Size", Placeholder = "e.g., 48px")]
        public StringField? TitleSize { get; set; }

        [Field(Title = "main Title Weight", Placeholder = "normal, bold, 600")]
        public StringField TitleWeight { get; set; } = new() { Value = "normal" };

        [Field(Title = "Main Title Alignment", Placeholder = "left, center, right")]
        public StringField TitleAlignment { get; set; } = new() { Value = "left" };



        [Field(Title = "Description Text", Placeholder = "Enter your paragraph here...")]
        public TextAreaField? Text { get; set; }

        [Field(Title = "Description Color", Placeholder = "#000000")]
        public ColorField? TextColor { get; set; }

        [Field(Title = "Description Size", Placeholder = "e.g., 16px")]
        public StringField? TextSize { get; set; }

        [Field(Title = "Description Weight", Placeholder = "normal, bold, 600")]
        public StringField TextWeight { get; set; } = new() { Value = "normal" };

        [Field(Title = "Description Alignment", Placeholder = "left, center, right")]
        public StringField TextAlignment { get; set; } = new() { Value = "left" };
        // ── Sección: Icono izquierdo / superior ────────────────────────
        [Header("Icono principal (imagen)")]

        [Field(Title = "Imagen icono")]
        public ImageField LeftIcon { get; set; } = new();

        [Field(Title = "Tamaño icono imagen", Placeholder = "Ej: 48px")]
        public StringField LeftIconSize { get; set; } = new();

        [Header("Icono principal (FontAwesome)")]

        [Field(Title = "Clase FontAwesome", Placeholder = "Ej: fas fa-rocket")]
        public StringField LeftIconClass { get; set; } = new();

        [Field(Title = "Color icono FA")]
        public ColorField LeftIconColor { get; set; } = new();

        [Field(Title = "Tamaño icono FA", Placeholder = "Ej: 2rem")]
        public StringField LeftIconFaSize { get; set; } = new() { Value = "2rem" };

        // ── Sección: Icono derecho (decorativo) ────────────────────────
        [Header("Icono derecho (decorativo)")]

        [Field(Title = "Imagen icono derecho")]
        public ImageField RightIcon { get; set; } = new();

        [Field(Title = "Tamaño icono derecho", Placeholder = "Ej: 32px")]
        public StringField RightIconSize { get; set; } = new();

        [Field(Title = "Clase FontAwesome derecho", Placeholder = "Ej: fas fa-arrow-right")]
        public StringField RightIconClass { get; set; } = new();

        [Field(Title = "Color icono derecho FA")]
        public ColorField RightIconColor { get; set; } = new();

        [Field(Title = "Subtitle", Placeholder = "e.g., A short subtitle")]
        public StringField? SubTitle { get; set; }

        [Field(Title = "Subtitle Color", Placeholder = "#000000")]
        public ColorField? SubTitleColor { get; set; }

        [Field(Title = "Subtitle Size", Placeholder = "e.g., 24px")]
        public StringField? SubTitleSize { get; set; }

        [Field(Title = "SubTitle Weight", Placeholder = "normal, bold, 600")]
        public StringField SubTitletWeight { get; set; } = new() { Value = "normal" };

        [Field(Title = "SubTitle Alignment", Placeholder = "left, center, right")]
        public StringField SubTitleAlignment { get; set; } = new() { Value = "left" };



        // --- Background ---
        [Field(Title = "Background Color", Placeholder = "#ffffff")]
        public ColorField? BackgroundColor { get; set; }

        [Field(Title = "Background Image")]
        public ImageField? BackgroundImage { get; set; }

        [Field(Title = "Background Opacity %", Placeholder = "100 = full, 50 = half, 0 = hidden")]
        public NumberField? BackgroundOpacity { get; set; }

        [Field(Title = "Background Blur (px)", Placeholder = "0 = clear, 5 = soft, 10 = heavy")]
        public NumberField? BackgroundBlur { get; set; }

        // --- Border ---
        [Field(Title = "Border", Placeholder = "e.g., 1px solid #000000")]
        public StringField? Border { get; set; }

        [Field(Title = "Border Radius", Placeholder = "e.g., 12px or 50%")]
        public StringField? BorderRadius { get; set; }

        // --- First Image ---
        [Field(Title = "First Image")]
        public ImageField? FirstImage { get; set; }

        [Field(Title = "First Image Size", Placeholder = "e.g., 50% or 200px")]
        public StringField? FirstImageSize { get; set; }

        [Field(Title = "First Image - Top Offset", Placeholder = "e.g., 0px or 20%. Leave empty to ignore.")]
        public StringField? FirstImageOffsetTop { get; set; }

        [Field(Title = "First Image - Bottom Offset", Placeholder = "e.g., 0px. Leave empty to ignore.")]
        public StringField? FirstImageOffsetBottom { get; set; }

        [Field(Title = "First Image - Left Offset", Placeholder = "e.g., 0px or 10%. Leave empty to ignore.")]
        public StringField? FirstImageOffsetLeft { get; set; }

        [Field(Title = "First Image - Right Offset", Placeholder = "e.g., 0px or -50px (outside). Leave empty to ignore.")]
        public StringField? FirstImageOffsetRight { get; set; }

        [Field(Title = "First Image Layer", Placeholder = "behind = behind text, front = above text")]
        public StringField? FirstImageLayer { get; set; }

        [Field(Title = "Place First Image Next to Text")]
        public BoolField? FirstImageNextToText { get; set; }

        // --- Second Image ---
        [Field(Title = "Second Image")]
        public ImageField? SecondImage { get; set; }

        [Field(Title = "Second Image Size", Placeholder = "e.g., 30% or 150px")]
        public StringField? SecondImageSize { get; set; }

        [Field(Title = "Second Image - Top Offset", Placeholder = "e.g., 0px or 20%. Leave empty to ignore.")]
        public StringField? SecondImageOffsetTop { get; set; }

        [Field(Title = "Second Image - Bottom Offset", Placeholder = "e.g., 0px. Leave empty to ignore.")]
        public StringField? SecondImageOffsetBottom { get; set; }

        [Field(Title = "Second Image - Left Offset", Placeholder = "e.g., 0px or 10%. Leave empty to ignore.")]
        public StringField? SecondImageOffsetLeft { get; set; }

        [Field(Title = "Second Image - Right Offset", Placeholder = "e.g., 0px or -50px (outside). Leave empty to ignore.")]
        public StringField? SecondImageOffsetRight { get; set; }

        [Field(Title = "Second Image Layer", Placeholder = "behind = behind text, front = above text")]
        public StringField? SecondImageLayer { get; set; }

        [Field(Title = "Place Second Image Next to Text")]
        public BoolField? SecondImageNextToText { get; set; }
    }
}
