using BrixCMS.Open.Data.Fields;
using BrixCMS.Open.Models.Base;

namespace BrixCMS.Open.Models.Blocks
{
    [BlockType(Name = "Hero Section", Category = "Headers", Icon = "fas fa-play-circle",
        Description = "Full-width hero with title, subtitle, and background image. Perfect for page introductions.")]
    public class HeroBlock : BlockBase
    {
        // =========================================================================
        // BACKGROUND SETTINGS
        // =========================================================================

        [Header("Background")]

        [Field(Title = "Background Color")]
        public ColorField BackgroundColor { get; set; } = new() { Value = "#0A0A0B" };

        [Field(Title = "Background Image")]
        public ImageField Background { get; set; } = new();

        // NEW: Gradient support (linear-gradient, radial-gradient, conic-gradient)
        [Field(Title = "Background Gradient", Description = "CSS gradient (e.g., linear-gradient(135deg, #667eea 0%, #764ba2 100%))")]
        public StringField BackgroundGradient { get; set; } = new();

        [Field(Title = "Enable Parallax", Description = "Background image will move slower on scroll.")]
        public BoolField EnableParallax { get; set; } = new() { Value = "false" };

        // NEW: Background blur effect
        [Field(Title = "Background Blur (px)", Description = "Blur amount on background image (e.g., 2, 5, 10).")]
        public StringField BackgroundBlur { get; set; } = new() { Value = "0" };

        // NEW: Background brightness filter
        [Field(Title = "Background Brightness", Description = "Brightness filter (0.5 = dark, 1 = normal, 1.5 = bright).")]
        public StringField BackgroundBrightness { get; set; } = new() { Value = "1" };

        // NEW: Background size
        [Field(Title = "Background Size", Description = "cover, contain, or auto", Placeholder = "cover")]
        public StringField BackgroundSize { get; set; } = new() { Value = "cover" };

        // NEW: Background position
        [Field(Title = "Background Position", Description = "center, top, bottom, left, right", Placeholder = "center")]
        public StringField BackgroundPosition { get; set; } = new() { Value = "center" };

        // NEW: Background repeat
        [Field(Title = "Background Repeat", Description = "no-repeat, repeat, repeat-x, repeat-y", Placeholder = "no-repeat")]
        public StringField BackgroundRepeat { get; set; } = new() { Value = "no-repeat" };

        // =========================================================================
        // OVERLAY SETTINGS
        // =========================================================================

        [Header("Overlay")]

        [Field(Title = "Overlay Color", Description = "Color of the background overlay.", Placeholder = "#000000")]
        public ColorField OverlayColor { get; set; } = new() { Value = "#000000" };

        [Field(Title = "Overlay Opacity (0–1)", Description = "Opacity of the overlay.", Placeholder = "0.3")]
        public StringField OverlayOpacity { get; set; } = new() { Value = "0.3" };

        // NEW: Second overlay for gradient effect
        [Field(Title = "Overlay Gradient", Description = "Second overlay gradient (e.g., linear-gradient(to top, black, transparent))")]
        public TextAreaField OverlayGradient { get; set; } = new();

        // NEW: Overlay blend mode
        [Field(Title = "Overlay Blend Mode", Description = "mix-blend-mode: multiply, overlay, screen, etc.")]
        public StringField OverlayBlendMode { get; set; } = new() { Value = "normal" };

        // =========================================================================
        // GRID PATTERN OVERLAY (from your view)
        // =========================================================================

        [Header("Grid Pattern")]

        // NEW: Show grid pattern overlay
        [Field(Title = "Show Grid Pattern", Description = "Display a subtle grid pattern overlay.")]
        public BoolField ShowGridPattern { get; set; } = new() { Value = "true" };

        [Field(Title = "Grid Pattern Color", Description = "Color of the grid lines.")]
        public ColorField GridPatternColor { get; set; } = new() { Value = "#2A2A30" };

        [Field(Title = "Grid Pattern Opacity", Description = "Opacity of the grid pattern (0-1).")]
        public StringField GridPatternOpacity { get; set; } = new() { Value = "0.3" };

        [Field(Title = "Grid Pattern Size", Description = "Size of grid cells (e.g., 60px, 40px).")]
        public StringField GridPatternSize { get; set; } = new() { Value = "60px" };

        // =========================================================================
        // CONTENT SETTINGS
        // =========================================================================

        [Header("Content")]

        [Field(Title = "Badge Text", Description = "Small badge above the title.", Placeholder = "Ex: Nu i public beta")]
        public StringField BadgeText { get; set; } = new();

        // NEW: Badge color
        [Field(Title = "Badge Background Color", Description = "Background color of the badge.")]
        public ColorField BadgeBgColor { get; set; } = new() { Value = "rgba(91,110,245,0.1)" };

        // NEW: Badge border color
        [Field(Title = "Badge Border Color", Description = "Border color of the badge.")]
        public ColorField BadgeBorderColor { get; set; } = new() { Value = "rgba(91,110,245,0.3)" };

        // NEW: Badge text color
        [Field(Title = "Badge Text Color", Description = "Text color of the badge.")]
        public ColorField BadgeTextColor { get; set; } = new() { Value = "#A5B4FC" };

        // NEW: Badge dot color
        [Field(Title = "Badge Dot Color", Description = "Color of the status dot in badge.")]
        public ColorField BadgeDotColor { get; set; } = new() { Value = "#22C55E" };

        [Field(Title = "Title", Description = "Main title of the hero section.", Placeholder = "Enter the title")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Title Color")]
        public ColorField TitleColor { get; set; } = new() { Value = "#F0F0F5" };

        [Field(Title = "Title Size", Description = "Size of the title.", Placeholder = "3rem")]
        public StringField TitleSize { get; set; } = new() { Value = "3rem" };

        [Field(Title = "Title Weight", Description = "Font weight (normal, bold, 400, 600, 700, 800, 900).")]
        public StringField TitleWeight { get; set; } = new() { Value = "900" };

        // NEW: Title line height
        [Field(Title = "Title Line Height", Description = "Line height of the title.", Placeholder = "1.2")]
        public StringField TitleLineHeight { get; set; } = new() { Value = "1.2" };

        // NEW: Title max width
        [Field(Title = "Title Max Width", Description = "Maximum width of the title (e.g., 800px, 4xl).")]
        public StringField TitleMaxWidth { get; set; } = new();

        [Field(Title = "Subtitle", Description = "Subtitle text.")]
        public StringField Subtitle { get; set; } = new();

        [Field(Title = "Subtitle Color")]
        public ColorField SubtitleColor { get; set; } = new() { Value = "#e2e8f0" };

        [Field(Title = "Subtitle Size", Description = "Size of the subtitle.", Placeholder = "1.2rem")]
        public StringField SubtitleSize { get; set; } = new() { Value = "1.2rem" };

        [Field(Title = "Subtitle Weight", Description = "Font weight of the subtitle.")]
        public StringField SubtitleWeight { get; set; } = new() { Value = "normal" };

        // NEW: Subtitle max width
        [Field(Title = "Subtitle Max Width", Description = "Maximum width of the subtitle (e.g., 2xl, 600px).")]
        public StringField SubtitleMaxWidth { get; set; } = new() { Value = "2xl" };

        [Field(Title = "Description", Description = "Description text.")]
        public StringField Description { get; set; } = new();

        [Field(Title = "Description Color")]
        public ColorField DescriptionColor { get; set; } = new() { Value = "rgba(255,255,255,0.75)" };

        // NEW: Description max width
        [Field(Title = "Description Max Width", Description = "Maximum width of the description.")]
        public StringField DescriptionMaxWidth { get; set; } = new() { Value = "xl" };

        // =========================================================================
        // LAYOUT & SPACING
        // =========================================================================

        [Header("Height & Layout")]

        [Field(Title = "Hero Height", Description = "full-screen | half-screen | compact | custom value (e.g., 500px, 80vh)")]
        public StringField Height { get; set; } = new() { Value = "half-screen" };

        [Field(Title = "Content Alignment", Description = "left | center | right")]
        public StringField TextAlign { get; set; } = new() { Value = "center" };

        // NEW: Vertical alignment
        [Field(Title = "Vertical Alignment", Description = "top | center | bottom")]
        public StringField VerticalAlign { get; set; } = new() { Value = "center" };

        // NEW: Content max width
        [Field(Title = "Content Max Width", Description = "Maximum width of the content container (e.g., 4xl, 6xl, 1280px).")]
        public StringField ContentMaxWidth { get; set; } = new() { Value = "4xl" };

        // NEW: Padding Y
        [Field(Title = "Padding Y", Description = "Vertical padding override.", Placeholder = "6rem")]
        public StringField PaddingY { get; set; } = new() { Value = "6rem" };

        // NEW: Padding X
        [Field(Title = "Padding X", Description = "Horizontal padding.", Placeholder = "1.5rem")]
        public StringField PaddingX { get; set; } = new() { Value = "1.5rem" };

        // NEW: Margin top
        [Field(Title = "Margin Top", Description = "Top margin override.")]
        public StringField MarginTop { get; set; } = new();

        // NEW: Margin bottom
        [Field(Title = "Margin Bottom", Description = "Bottom margin override.")]
        public StringField MarginBottom { get; set; } = new();

        // =========================================================================
        // PRIMARY BUTTON
        // =========================================================================

        [Header("Primary Button")]

        [Field(Title = "Button Text")]
        public StringField ButtonText { get; set; } = new();

        [Field(Title = "Button URL")]
        public StringField ButtonUrl { get; set; } = new();

        [Field(Title = "Button Color")]
        public ColorField ButtonColor { get; set; } = new() { Value = "#3b82f6" };

        [Field(Title = "Button Text Color")]
        public ColorField ButtonTextColor { get; set; } = new() { Value = "#ffffff" };

        // NEW: Button border radius
        [Field(Title = "Button Border Radius", Description = "Border radius (e.g., rounded-xl, rounded-full, 12px).")]
        public StringField ButtonBorderRadius { get; set; } = new() { Value = "rounded-xl" };

        // NEW: Button padding
        [Field(Title = "Button Padding", Description = "Padding (e.g., px-8 py-3, 1rem 2rem).")]
        public StringField ButtonPadding { get; set; } = new() { Value = "px-8 py-3" };

        // NEW: Button hover effect
        [Field(Title = "Button Hover Opacity", Description = "Opacity on hover (0-1).")]
        public StringField ButtonHoverOpacity { get; set; } = new() { Value = "0.85" };

        // =========================================================================
        // SECONDARY BUTTON
        // =========================================================================

        [Header("Secondary Button")]

        [Field(Title = "Button 2 Text", Placeholder = "Ex: ▶ Se demo (2 min)")]
        public StringField Button2Text { get; set; } = new();

        [Field(Title = "Button 2 URL")]
        public StringField Button2Url { get; set; } = new();

        [Field(Title = "Button 2 Color")]
        public ColorField Button2Color { get; set; } = new() { Value = "transparent" };

        [Field(Title = "Button 2 Text Color")]
        public ColorField Button2TextColor { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Button 2 Border Color")]
        public ColorField Button2BorderColor { get; set; } = new() { Value = "#2A2A30" };

        // NEW: Button 2 border width
        [Field(Title = "Button 2 Border Width", Description = "Border width (e.g., 1px, 2px).")]
        public StringField Button2BorderWidth { get; set; } = new() { Value = "1px" };

        // NEW: Button 2 border radius
        [Field(Title = "Button 2 Border Radius", Description = "Border radius for secondary button.")]
        public StringField Button2BorderRadius { get; set; } = new() { Value = "rounded-xl" };

        // NEW: Button 2 hover opacity
        [Field(Title = "Button 2 Hover Opacity", Description = "Opacity on hover for secondary button.")]
        public StringField Button2HoverOpacity { get; set; } = new() { Value = "0.85" };

        // =========================================================================
        // SOCIAL PROOF
        // =========================================================================

        [Header("Social Proof")]

        [Field(Title = "Show Social Proof", Description = "Display social proof section below buttons.")]
        public BoolField ShowSocialProof { get; set; } = new() { Value = "false" };

        // NEW: Social proof items (comma separated)
        [Field(Title = "Social Proof Items", Description = "Comma-separated list of social proof texts.")]
        public StringField SocialProofItems { get; set; } = new() { Value = "Ingen kreditkortsinformation krävs,Gratis i 14 dagar,Avsluta när som helst" };

        // NEW: Social proof text color
        [Field(Title = "Social Proof Color", Description = "Text color for social proof.")]
        public ColorField SocialProofColor { get; set; } = new() { Value = "#56566A" };

        // NEW: Social proof icon color
        [Field(Title = "Social Proof Icon Color", Description = "Checkmark icon color.")]
        public ColorField SocialProofIconColor { get; set; } = new() { Value = "#22C55E" };

        // NEW: Social proof separator color
        [Field(Title = "Social Proof Separator Color", Description = "Vertical separator color.")]
        public ColorField SocialProofSeparatorColor { get; set; } = new() { Value = "#2A2A30" };

        // =========================================================================
        // STYLING & APPEARANCE
        // =========================================================================

        [Header("Styling")]

        // NEW: Border radius for the entire section
        [Field(Title = "Border Radius", Description = "Border radius for the hero section.")]
        public StringField BorderRadius { get; set; } = new() { Value = "rounded-none" };

        // NEW: Shadow
        [Field(Title = "Shadow", Description = "Box shadow for the hero section.")]
        public StringField Shadow { get; set; } = new() { Value = "shadow-none" };

        // NEW: Border
        [Field(Title = "Border", Description = "Border style (e.g., 1px solid #e5e7eb).")]
        public StringField Border { get; set; } = new();

        // NEW: Z-index
        [Field(Title = "Z-Index", Description = "Z-index value (e.g., 10, 20, 50).")]
        public StringField ZIndex { get; set; } = new() { Value = "auto" };

        // NEW: Section ID (anchor)
        [Field(Title = "Section ID (Anchor)", Description = "ID for section anchoring (e.g., 'hero' for #hero).")]
        public StringField SectionId { get; set; } = new();

        // =========================================================================
        // VISIBILITY & RESPONSIVE
        // =========================================================================

        [Header("Visibility")]

        // NEW: Hide on mobile
        [Field(Title = "Hide on Mobile", Description = "Hide this section on mobile devices.")]
        public BoolField HideOnMobile { get; set; } = new() { Value = "false" };

        // NEW: Hide on tablet
        [Field(Title = "Hide on Tablet", Description = "Hide this section on tablet devices.")]
        public BoolField HideOnTablet { get; set; } = new() { Value = "false" };

        // NEW: Hide on desktop
        [Field(Title = "Hide on Desktop", Description = "Hide this section on desktop devices.")]
        public BoolField HideOnDesktop { get; set; } = new() { Value = "false" };

        // =========================================================================
        // ANIMATION
        // =========================================================================

        [Header("Animation")]

        // NEW: Animate on scroll (inherited from BlockBase, but adding more options)
        [Field(Title = "Animation Type", Description = "fade-up, fade-in, slide-in-left, slide-in-right, zoom-in, none")]
        public StringField AnimationType { get; set; } = new() { Value = "fade-up" };

        // NEW: Animation delay (ms)
        [Field(Title = "Animation Delay (ms)", Description = "Delay before animation starts (e.g., 200, 500).")]
        public StringField AnimationDelay { get; set; } = new() { Value = "0" };

        // NEW: Animation duration (ms)
        [Field(Title = "Animation Duration (ms)", Description = "Animation duration (e.g., 600, 1000).")]
        public StringField AnimationDuration { get; set; } = new() { Value = "600" };

        // =========================================================================
        // ADVANCED / CUSTOMIZATION
        // =========================================================================

        [Header("Advanced")]

        // NEW: Custom CSS classes
        [Field(Title = "Custom CSS Classes", Description = "Additional CSS classes for the hero section.")]
        public StringField CustomClasses { get; set; } = new();

        // NEW: Custom inline styles
        [Field(Title = "Custom Styles", Description = "Additional inline CSS styles.")]
        public TextAreaField CustomStyles { get; set; } = new();

        // NEW: Data attributes for JS
        [Field(Title = "Data Attributes", Description = "JSON object of data-* attributes.")]
        public TextAreaField DataAttributes { get; set; } = new();

        // =========================================================================
        // ALIGNMENT CONTROLS (Individual element alignment)
        // =========================================================================

        [Header("Element Alignment")]

        [Field(Title = "Badge Alignment", Description = "left | center | right", Placeholder = "center")]
        public StringField BadgeAlignment { get; set; } = new() { Value = "center" };

        [Field(Title = "Title Alignment", Description = "left | center | right", Placeholder = "center")]
        public StringField TitleAlignment { get; set; } = new() { Value = "center" };

        [Field(Title = "Subtitle Alignment", Description = "left | center | right", Placeholder = "center")]
        public StringField SubtitleAlignment { get; set; } = new() { Value = "center" };

        [Field(Title = "Description Alignment", Description = "left | center | right", Placeholder = "center")]
        public StringField DescriptionAlignment { get; set; } = new() { Value = "center" };

        [Field(Title = "Buttons Alignment", Description = "left | center | right", Placeholder = "center")]
        public StringField ButtonsAlignment { get; set; } = new() { Value = "center" };

        [Field(Title = "Social Proof Alignment", Description = "left | center | right", Placeholder = "center")]
        public StringField SocialProofAlignment { get; set; } = new() { Value = "center" };
    }
}
