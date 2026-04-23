using BrixCMS.Open.Data.Fields;
using BrixCMS.Open.Models.Base;

namespace BrixCMS.Open.Models.BlazorBlocks
{
    [BlockType(Name = "Image + Text", Category = "Layout", Icon = "fas fa-columns", Description = "Side-by-side image and text layout. Supports left/right image position, rounded corners, and custom spacing.")]
    public class FlexibleImageTextBlock : BlockBase
    {
        // ==================== BACKGROUND ====================
        [Field(Title = "Background Color", Description = "Background color of the block")]
        public ColorField BackgroundColor { get; set; } = new();

        [Field(Title = "Background Image", Description = "Background image (replaces color)")]
        public ImageField BackgroundImage { get; set; } = new();

        [Field(Title = "Background Opacity", Placeholder = "0.1 - 1", Description = "Background opacity (0 = transparent, 1 = solid)")]
        public StringField BackgroundOpacity { get; set; } = new() { Value = "1" };

        [Field(Title = "Padding Vertical", Placeholder = "py-8, py-12, py-16", Description = "Vertical spacing (Tailwind)")]
        public StringField PaddingVertical { get; set; } = new() { Value = "py-12" };

        [Field(Title = "Padding Horizontal", Placeholder = "px-4, px-6, px-8", Description = "Horizontal spacing (Tailwind)")]
        public StringField PaddingHorizontal { get; set; } = new() { Value = "px-6" };

        // ==================== LAYOUT ====================
        [Field(Title = "Layout", Description = "Content organization")]
        public SelectField<string> Layout { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "image-left", Label = "Image Left", Icon = "fas fa-image" },
                new() { Value = "image-right", Label = "Image Right", Icon = "fas fa-image" },
                new() { Value = "image-top", Label = "Image Top", Icon = "fas fa-arrow-up" },
                new() { Value = "image-bottom", Label = "Image Bottom", Icon = "fas fa-arrow-down" },
                new() { Value = "two-columns", Label = "Two Columns", Icon = "fas fa-columns" }
            },
            Value = "image-left"
        };

        [Field(Title = "Vertical Alignment", Description = "Vertical alignment of content")]
        public SelectField<string> VerticalAlignment { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "start", Label = "Top", Icon = "fas fa-arrow-up" },
                new() { Value = "center", Label = "Center", Icon = "fas fa-align-center" },
                new() { Value = "end", Label = "Bottom", Icon = "fas fa-arrow-down" }
            },
            Value = "center"
        };

        [Field(Title = "Gap", Placeholder = "gap-4, gap-6, gap-8", Description = "Space between image and text")]
        public StringField Gap { get; set; } = new() { Value = "gap-8" };

        // ==================== TITLE ====================
        [Field(Title = "Title", Placeholder = "Enter title HTML")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Title Color")]
        public ColorField TitleColor { get; set; } = new();

        [Field(Title = "Title Size", Placeholder = "text-2xl, text-3xl, 32px")]
        public StringField TitleSize { get; set; } = new() { Value = "text-3xl" };

        [Field(Title = "Title Weight", Description = "Font weight")]
        public SelectField<string> TitleWeight { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "font-normal", Label = "Normal" },
                new() { Value = "font-medium", Label = "Medium" },
                new() { Value = "font-semibold", Label = "Semibold" },
                new() { Value = "font-bold", Label = "Bold" }
            },
            Value = "font-bold"
        };

        [Field(Title = "Title Margin Bottom", Placeholder = "mb-2, mb-4, mb-6", Description = "Bottom margin of title")]
        public StringField TitleMarginBottom { get; set; } = new() { Value = "mb-4" };

        // ==================== SUBTITLE ====================
        [Field(Title = "SubTitle", Placeholder = "Enter subtitle HTML")]
        public StringField SubTitle { get; set; } = new();

        [Field(Title = "SubTitle Color")]
        public ColorField SubTitleColor { get; set; } = new();

        [Field(Title = "SubTitle Size", Placeholder = "text-xl, text-2xl, 24px")]
        public StringField SubTitleSize { get; set; } = new() { Value = "text-xl" };

        [Field(Title = "SubTitle Weight", Description = "Font weight")]
        public SelectField<string> SubTitleWeight { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "font-normal", Label = "Normal" },
                new() { Value = "font-medium", Label = "Medium" },
                new() { Value = "font-semibold", Label = "Semibold" }
            },
            Value = "font-semibold"
        };

        [Field(Title = "SubTitle Margin Bottom", Placeholder = "mb-2, mb-4, mb-6")]
        public StringField SubTitleMarginBottom { get; set; } = new() { Value = "mb-3" };

        // ==================== IMAGE ====================
        [Field(Title = "Image")]
        public ImageField Image { get; set; } = new();

        // DEFAULT: 40% width - smaller images
        [Field(Title = "Image Width", Placeholder = "w-full, w-2/5, 300px, 40%", Description = "Width of the image - default is 40%")]
        public StringField ImageWidth { get; set; } = new() { Value = "w-2/5" };  // 40% instead of 50%

        // DEFAULT: Max width limited to 300px to keep images small
        [Field(Title = "Image Max Width", Placeholder = "max-w-md, max-w-sm, 300px", Description = "Maximum width of the image - default max 300px")]
        public StringField ImageMaxWidth { get; set; } = new() { Value = "max-w-xs" };  // 320px max (Tailwind xs = 320px)

        [Field(Title = "Image Border Radius", Description = "Rounded corners")]
        public SelectField<string> ImageBorderRadius { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "rounded-none", Label = "None" },
                new() { Value = "rounded", Label = "Small" },
                new() { Value = "rounded-lg", Label = "Medium" },
                new() { Value = "rounded-xl", Label = "Large" },
                new() { Value = "rounded-full", Label = "Circle" }
            },
            Value = "rounded-lg"
        };

        [Field(Title = "Image Shadow", Description = "Image shadow")]
        public SelectField<string> ImageShadow { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "shadow-none", Label = "None" },
                new() { Value = "shadow-sm", Label = "Small" },
                new() { Value = "shadow", Label = "Normal" },
                new() { Value = "shadow-lg", Label = "Large" },
                new() { Value = "shadow-xl", Label = "Extra large" }
            },
            Value = "shadow-lg"
        };

        [Field(Title = "Image Object Fit", Description = "How the image fits its container")]
        public SelectField<string> ImageObjectFit { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "object-contain", Label = "Contain (maintains aspect ratio)" },
                new() { Value = "object-cover", Label = "Cover (fills space)" },
                new() { Value = "object-fill", Label = "Fill (distorts)" },
                new() { Value = "object-scale-down", Label = "Scale down" }
            },
            Value = "object-contain"
        };

        // ==================== TEXT ====================
        [Field(Title = "Text", Placeholder = "Enter Text HTML")]
        public TextAreaField Text { get; set; } = new();

        [Field(Title = "Text Color")]
        public ColorField TextColor { get; set; } = new();

        [Field(Title = "Text Size", Placeholder = "text-base, text-lg, 16px")]
        public StringField TextSize { get; set; } = new() { Value = "text-base" };

        [Field(Title = "Text Line Height", Placeholder = "leading-relaxed, leading-loose, 1.6")]
        public StringField TextLineHeight { get; set; } = new() { Value = "leading-relaxed" };

        // ==================== BUTTON ====================
        [Field(Title = "Button Text", Placeholder = "Click here")]
        public StringField ButtonText { get; set; } = new();

        [Field(Title = "Button Link", Placeholder = "/page, https://...")]
        public UrlField ButtonLink { get; set; } = new();

        [Field(Title = "Button Style")]
        public SelectField<string> ButtonStyle { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "primary", Label = "Primary" },
                new() { Value = "secondary", Label = "Secondary" },
                new() { Value = "outline", Label = "Outline" }
            },
            Value = "primary"
        };
    }
}
