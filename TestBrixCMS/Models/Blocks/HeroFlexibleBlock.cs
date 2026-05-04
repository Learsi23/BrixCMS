using TestBrixCMS.Data.Fields;
using TestBrixCMS.Models.Base;

namespace TestBrixCMS.Models.Blocks
{
    [BlockType(Name = "Hero Flexible", Category = "Headers", Icon = "fas fa-film",
        Description = "Hero with flexible background: video, image, or color. Multiple buttons support.")]
    public class HeroFlexibleBlock : BlockBase
    {
        // =========================================================================
        // BACKGROUND TYPE
        // =========================================================================

        [Header("Background")]
        
        [Field(Title = "Background Type", Description = "Choose background type: video, image, or color")]
        public StringField BackgroundType { get; set; } = new() { Value = "image" };

        [Field(Title = "Background Video URL", Description = "Video file URL (mp4, webm). Leave empty if not using video.")]
        public StringField BackgroundVideo { get; set; } = new();

        [Field(Title = "Video Fallback Image", Description = "Image shown if video fails or on mobile. Recommended.")]
        public ImageField VideoFallbackImage { get; set; } = new();

        [Field(Title = "Background Image", Description = "Background image for image mode.")]
        public ImageField BackgroundImage { get; set; } = new();

        [Field(Title = "Background Color", Description = "Solid background color for color mode.")]
        public ColorField BackgroundColor { get; set; } = new() { Value = "#1e293b" };

        [Field(Title = "Background Gradient", Description = "CSS gradient (e.g., linear-gradient(135deg, #667eea 0%, #764ba2 100%))")]
        public StringField BackgroundGradient { get; set; } = new();

        // =========================================================================
        // VIDEO SETTINGS
        // =========================================================================

        [Header("Video Settings")]

        [Field(Title = "Video Autoplay", Description = "Autoplay video on load.")]
        public BoolField VideoAutoplay { get; set; } = new() { Value = "true" };

        [Field(Title = "Video Muted", Description = "Mute video by default (required for autoplay).")]
        public BoolField VideoMuted { get; set; } = new() { Value = "true" };

        [Field(Title = "Video Loop", Description = "Loop video continuously.")]
        public BoolField VideoLoop { get; set; } = new() { Value = "true" };

        [Field(Title = "Video Object Fit", Description = "cover, contain, fill")]
        public StringField VideoObjectFit { get; set; } = new() { Value = "cover" };

        // =========================================================================
        // OVERLAY
        // =========================================================================

        [Header("Overlay")]

        [Field(Title = "Overlay Opacity (0-1)", Description = "Dark overlay opacity.", Placeholder = "0.5")]
        public StringField OverlayOpacity { get; set; } = new() { Value = "0.5" };

        [Field(Title = "Overlay Color", Description = "Overlay color.", Placeholder = "#000000")]
        public ColorField OverlayColor { get; set; } = new() { Value = "#000000" };

        // =========================================================================
        // CONTENT
        // =========================================================================

        [Header("Content")]

        [Field(Title = "Badge Text", Description = "Small badge above title.")]
        public StringField BadgeText { get; set; } = new();

        [Field(Title = "Badge Background Color")]
        public ColorField BadgeBgColor { get; set; } = new() { Value = "rgba(255,255,255,0.1)" };

        [Field(Title = "Badge Text Color")]
        public ColorField BadgeTextColor { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Title", Description = "Main title text.")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Title Color")]
        public ColorField TitleColor { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Title Size", Description = "Font size (e.g., 3rem, 48px).", Placeholder = "3rem")]
        public StringField TitleSize { get; set; } = new() { Value = "3rem" };

        [Field(Title = "Title Weight", Description = "Font weight (normal, bold, 400, 600, 700, 800, 900).")]
        public StringField TitleWeight { get; set; } = new() { Value = "800" };

        [Field(Title = "Title Alignment", Description = "Text alignment (left, center, right).")]
        public StringField TitleAlignment { get; set; } = new() { Value = "center" };

        [Field(Title = "Title Max Width", Description = "Max width (e.g., 800px, 4xl).")]
        public StringField TitleMaxWidth { get; set; } = new();

        [Field(Title = "Subtitle", Description = "Subtitle text below title.")]
        public StringField Subtitle { get; set; } = new();

        [Field(Title = "Subtitle Color")]
        public ColorField SubtitleColor { get; set; } = new() { Value = "#e2e8f0" };

        [Field(Title = "Subtitle Size", Description = "Font size.", Placeholder = "1.25rem")]
        public StringField SubtitleSize { get; set; } = new() { Value = "1.25rem" };

        [Field(Title = "Subtitle Weight")]
        public StringField SubtitleWeight { get; set; } = new() { Value = "normal" };

        [Field(Title = "Subtitle Alignment")]
        public StringField SubtitleAlignment { get; set; } = new() { Value = "center" };

        [Field(Title = "Description", Description = "Description text below subtitle.")]
        public StringField Description { get; set; } = new();

        [Field(Title = "Description Color")]
        public ColorField DescriptionColor { get; set; } = new() { Value = "#cbd5e1" };

        [Field(Title = "Description Size", Description = "Font size.", Placeholder = "1rem")]
        public StringField DescriptionSize { get; set; } = new() { Value = "1rem" };

        [Field(Title = "Description Weight")]
        public StringField DescriptionWeight { get; set; } = new() { Value = "normal" };

        [Field(Title = "Description Alignment")]
        public StringField DescriptionAlignment { get; set; } = new() { Value = "center" };

        [Field(Title = "Description Max Width")]
        public StringField DescriptionMaxWidth { get; set; } = new();

        // =========================================================================
        // LAYOUT
        // =========================================================================

        [Header("Layout")]

        [Field(Title = "Hero Height", Description = "Height (e.g., 500px, 60vh, full-screen).", Placeholder = "600px")]
        public StringField Height { get; set; } = new() { Value = "600px" };

        [Field(Title = "Content Alignment", Description = "Horizontal alignment.", Placeholder = "center")]
        public StringField ContentAlign { get; set; } = new() { Value = "center" };

        [Field(Title = "Vertical Alignment", Description = "top, center, bottom")]
        public StringField VerticalAlign { get; set; } = new() { Value = "center" };

        [Field(Title = "Content Max Width")]
        public StringField ContentMaxWidth { get; set; } = new() { Value = "4xl" };

        [Field(Title = "Padding Y", Description = "Vertical padding.", Placeholder = "3rem")]
        public StringField PaddingY { get; set; } = new() { Value = "3rem" };

        // =========================================================================
        // BUTTONS (Multiple)
        // =========================================================================

        [Header("Button 1")]

        [Field(Title = "Button 1 Text")]
        public StringField Button1Text { get; set; } = new();

        [Field(Title = "Button 1 URL")]
        public StringField Button1Url { get; set; } = new();

        [Field(Title = "Button 1 Style", Description = "primary, secondary, outline")]
        public StringField Button1Style { get; set; } = new() { Value = "primary" };

        [Field(Title = "Button 1 Color")]
        public ColorField Button1Color { get; set; } = new() { Value = "#3b82f6" };

        [Field(Title = "Button 1 Text Color")]
        public ColorField Button1TextColor { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Button 1 Border Radius")]
        public StringField Button1BorderRadius { get; set; } = new() { Value = "rounded-xl" };

        [Field(Title = "Button 1 Padding", Description = "Button padding (e.g., px-6 py-3).")]
        public StringField Button1Padding { get; set; } = new() { Value = "px-6 py-3" };

        [Header("Button 2")]

        [Field(Title = "Button 2 Text")]
        public StringField Button2Text { get; set; } = new();

        [Field(Title = "Button 2 URL")]
        public StringField Button2Url { get; set; } = new();

        [Field(Title = "Button 2 Style", Description = "primary, secondary, outline")]
        public StringField Button2Style { get; set; } = new() { Value = "outline" };

        [Field(Title = "Button 2 Color")]
        public ColorField Button2Color { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Button 2 Text Color")]
        public ColorField Button2TextColor { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Button 2 Border Color")]
        public ColorField Button2BorderColor { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Button 2 Border Radius")]
        public StringField Button2BorderRadius { get; set; } = new() { Value = "rounded-xl" };

        [Field(Title = "Button 2 Padding")]
        public StringField Button2Padding { get; set; } = new() { Value = "px-6 py-3" };

        [Header("Button 3")]

        [Field(Title = "Button 3 Text")]
        public StringField Button3Text { get; set; } = new();

        [Field(Title = "Button 3 URL")]
        public StringField Button3Url { get; set; } = new();

        [Field(Title = "Button 3 Style", Description = "primary, secondary, outline")]
        public StringField Button3Style { get; set; } = new() { Value = "outline" };

        [Field(Title = "Button 3 Color")]
        public ColorField Button3Color { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Button 3 Text Color")]
        public ColorField Button3TextColor { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Button 3 Border Color")]
        public ColorField Button3BorderColor { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Button 3 Border Radius")]
        public StringField Button3BorderRadius { get; set; } = new() { Value = "rounded-xl" };

        [Field(Title = "Button 3 Padding")]
        public StringField Button3Padding { get; set; } = new() { Value = "px-6 py-3" };

        // =========================================================================
        // BUTTONS ALIGNMENT
        // =========================================================================

        [Header("Buttons")]

        [Field(Title = "Buttons Gap", Description = "Space between buttons.", Placeholder = "gap-4")]
        public StringField ButtonsGap { get; set; } = new() { Value = "gap-4" };

        [Field(Title = "Buttons Alignment", Description = "Alignment of all buttons.")]
        public StringField ButtonsAlignment { get; set; } = new() { Value = "center" };
    }
}
