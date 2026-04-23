using BrixCMS.Open.Models.Base;
using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.ColumnBlocks
{
    [BlockType(Name = "Columns", Category = "Layout", Icon = "fas fa-columns",
        Description = "Multi-column layout container. Add any blocks inside each column for flexible page layouts.")]
    public class FullColumnBlock : BlockGroupBase
    {
        // =========================================================================
        // BACKGROUND SETTINGS
        // =========================================================================

        [Field(Title = "Background Color", Description = "Background color of the section.")]
        public ColorField BackgroundColor { get; set; } = new();

        [Field(Title = "Background Image", Description = "Background image for the section.")]
        public ImageField BackgroundImage { get; set; } = new();

        [Field(Title = "Overlay Opacity (0-1)", Description = "Opacity of the background overlay.", Placeholder = "e.g., 0.5")]
        public StringField BackgroundOverlayOpacity { get; set; } = new() { Value = "0" };

        [Field(Title = "Overlay Color", Description = "Color of the background overlay.")]
        public ColorField BackgroundOverlayColor { get; set; } = new() { Value = "#000000" };

        // NEW: Background blur effect
        [Field(Title = "Background Blur (px)", Description = "Blur amount on background image (e.g., 2, 5, 10).", Placeholder = "0")]
        public StringField BackgroundBlur { get; set; } = new() { Value = "0" };

        // NEW: Background brightness filter
        [Field(Title = "Background Brightness", Description = "Brightness filter (0.5 = dark, 1 = normal, 1.5 = bright).", Placeholder = "1")]
        public StringField BackgroundBrightness { get; set; } = new() { Value = "1" };

        // NEW: Enable parallax effect
        [Field(Title = "Enable Parallax", Description = "Background image will move slower on scroll.")]
        public BoolField EnableParallax { get; set; } = new() { Value = "false" };

        // =========================================================================
        // COLUMN LAYOUT SETTINGS
        // =========================================================================

        [Field(Title = "Column Layout", Description = "How columns are distributed. Options: auto, centered, full")]
        public StringField ColumnLayout { get; set; } = new() { Value = "auto" };

        [Header("Layout")]

        [Field(Title = "Max Columns (Desktop)", Description = "Maximum number of columns for desktop (1-6).", Placeholder = "3")]
        public StringField MaxColumns { get; set; } = new() { Value = "3" };

        [Field(Title = "Tablet Columns (md)", Description = "Number of columns for tablet (1-4).", Placeholder = "2")]
        public StringField TabletColumns { get; set; } = new() { Value = "2" };

        // NEW: Mobile columns
        [Field(Title = "Mobile Columns (sm)", Description = "Number of columns for mobile (1-2).", Placeholder = "1")]
        public StringField MobileColumns { get; set; } = new() { Value = "1" };

        [Field(Title = "Gap Between Columns", Description = "Spacing between columns (Tailwind gap classes).")]
        public StringField Gap { get; set; } = new() { Value = "gap-6" };

        // NEW: Responsive gaps
        [Field(Title = "Gap Mobile", Description = "Gap on mobile devices (Tailwind class).")]
        public StringField GapMobile { get; set; } = new() { Value = "gap-4" };

        [Field(Title = "Gap Desktop", Description = "Gap on desktop devices (Tailwind class).")]
        public StringField GapDesktop { get; set; } = new() { Value = "gap-8" };

        [Field(Title = "Vertical Padding", Description = "Vertical padding for the grid.", Placeholder = "3rem")]
        public StringField PaddingY { get; set; } = new() { Value = "3rem" };

        [Field(Title = "Horizontal Padding", Description = "Horizontal padding for the grid.", Placeholder = "1.5rem")]
        public StringField PaddingX { get; set; } = new() { Value = "1.5rem" };

        // NEW: Individual padding controls
        [Field(Title = "Padding Top", Description = "Top padding override.", Placeholder = "e.g., 2rem")]
        public StringField PaddingTop { get; set; } = new();

        [Field(Title = "Padding Bottom", Description = "Bottom padding override.", Placeholder = "e.g., 2rem")]
        public StringField PaddingBottom { get; set; } = new();

        // NEW: Individual margin controls
        [Field(Title = "Margin Top", Description = "Top margin override.", Placeholder = "e.g., 2rem")]
        public StringField MarginTop { get; set; } = new();

        [Field(Title = "Margin Bottom", Description = "Bottom margin override.", Placeholder = "e.g., 2rem")]
        public StringField MarginBottom { get; set; } = new();

        [Field(Title = "Items Alignment", Description = "Alignment of items in the grid (start, center, end, stretch).")]
        public StringField ItemsAlign { get; set; } = new() { Value = "stretch" };

        [Field(Title = "Section ID (Anchor)", Description = "ID for section anchoring (e.g., 'services' for #services).")]
        public StringField SectionId { get; set; } = new();

        // NEW: Minimum section height
        [Field(Title = "Min Height", Description = "Minimum section height (e.g., 100vh, 500px, auto).")]
        public StringField MinHeight { get; set; } = new() { Value = "auto" };

        // NEW: Reverse order on mobile
        [Field(Title = "Reverse on Mobile", Description = "Reverse column order on mobile devices.")]
        public BoolField ReverseOnMobile { get; set; } = new() { Value = "false" };

        // NEW: Container queries support
        [Field(Title = "Use Container Queries", Description = "Enable modern container-based responsive behavior.")]
        public BoolField UseContainerQueries { get; set; } = new() { Value = "false" };

        // =========================================================================
        // STYLING SETTINGS
        // =========================================================================

        [Header("Styling")]

        // NEW: Border radius
        [Field(Title = "Border Radius", Description = "Border radius (Tailwind: rounded-none, rounded-sm, rounded, rounded-md, rounded-lg, rounded-xl, rounded-2xl, rounded-full)")]
        public StringField BorderRadius { get; set; } = new() { Value = "rounded-none" };

        // NEW: Box shadow
        [Field(Title = "Shadow", Description = "Box shadow (Tailwind: shadow-none, shadow-sm, shadow-md, shadow-lg, shadow-xl, shadow-2xl, shadow-inner)")]
        public StringField Shadow { get; set; } = new() { Value = "shadow-none" };

        // NEW: Border style
        [Field(Title = "Border", Description = "Border style (e.g., 1px solid #e5e7eb or border-gray-200).")]
        public StringField Border { get; set; } = new();

        // NEW: Z-index
        [Field(Title = "Z-Index", Description = "Z-index value (e.g., 10, 20, 50, auto).")]
        public StringField ZIndex { get; set; } = new() { Value = "auto" };

        // =========================================================================
        // SECTION HEADER
        // =========================================================================

        [Header("Section Header")]

        [Field(Title = "Header (Above Title)", Description = "Header text above the main title.")]
        public StringField TitleSida { get; set; } = new();

        [Field(Title = "Header Color", Description = "Color of the header text.")]
        public ColorField TitleSidaColor { get; set; } = new();

        [Field(Title = "Header Size", Description = "Size of the header text.", Placeholder = "1rem")]
        public StringField TitleSidaSize { get; set; } = new();

        [Field(Title = "Header Alignment", Description = "Alignment of the header text (left, center, right).")]
        public StringField TitleSidaAlign { get; set; } = new() { Value = "left" };

        // =========================================================================
        // MAIN TITLE
        // =========================================================================

        [Header("Main Title")]

        [Field(Title = "Title", Description = "Main title for the grid section.")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Title Color", Description = "Color of the main title text.")]
        public ColorField TitleColor { get; set; } = new();

        [Field(Title = "Title Size", Description = "Size of the main title text.", Placeholder = "2rem")]
        public StringField TitleSize { get; set; } = new();

        [Field(Title = "Title Border", Description = "Border of the main title text.", Placeholder = "1px solid #000")]
        public StringField TitleBorder { get; set; } = new();

        [Field(Title = "Title Weight", Description = "Font weight (normal, bold, 400, 600, 700).")]
        public StringField TitleWeight { get; set; }

        [Field(Title = "Title Alignment", Description = "Text alignment (left, center, right).")]
        public StringField TitleAlignment { get; set; } = new() { Value = "left" };

        // =========================================================================
        // SUBTITLE
        // =========================================================================

        [Header("Subtitle")]

        [Field(Title = "Subtitle", Description = "Subtitle for the grid section.")]
        public StringField SubTitle { get; set; } = new();

        [Field(Title = "Subtitle Color", Description = "Color of the subtitle text.")]
        public ColorField SubTitleColor { get; set; } = new();

        [Field(Title = "Subtitle Size", Description = "Size of the subtitle text.", Placeholder = "1.1rem")]
        public StringField SubTitleSize { get; set; } = new();

        [Field(Title = "Subtitle Border", Description = "Border of the subtitle text.", Placeholder = "1px solid #000")]
        public StringField SubTitleBorder { get; set; } = new();

        [Field(Title = "Subtitle Weight", Description = "Font weight (normal, bold, 400, 600, 700).")]
        public StringField SubTitleWeight { get; set; }

        [Field(Title = "Subtitle Alignment", Description = "Text alignment (left, center, right).")]
        public StringField SubTitleAlignment { get; set; } = new() { Value = "left" };

        // =========================================================================
        // TEXT (Inline)
        // =========================================================================

        [Header("Text")]

        [Field(Title = "Text", Description = "Inline text for the grid section.")]
        public StringField Text { get; set; } = new();

        [Field(Title = "Text Color", Description = "Color of the text.")]
        public ColorField TextColor { get; set; } = new();

        [Field(Title = "Text Size", Description = "Size of the text.", Placeholder = "1rem")]
        public StringField TextSize { get; set; } = new();

        [Field(Title = "Text Border", Description = "Border of the text.", Placeholder = "1px solid #000")]
        public StringField TextBorder { get; set; } = new();

        [Field(Title = "Text Weight", Description = "Font weight (normal, bold, 400, 600, 700).")]
        public StringField TextWeight { get; set; }

        [Field(Title = "Text Alignment", Description = "Text alignment (left, center, right).")]
        public StringField TextAlignment { get; set; } = new() { Value = "left" };

        // =========================================================================
        // DESCRIPTION (Rich Text / HTML)
        // =========================================================================

        [Header("Description")]

        [Field(Title = "Description (HTML)", Description = "Rich text description below subtitle.")]
        public TextAreaField Description { get; set; } = new();

        [Field(Title = "Description Color", Description = "Color of the description text.")]
        public ColorField DescriptionColor { get; set; } = new();

        [Field(Title = "Description Border", Description = "Border of the description text.", Placeholder = "1px solid #000")]
        public StringField DescriptionBorder { get; set; } = new();

        [Field(Title = "Description Weight", Description = "Font weight (normal, bold, 400, 600, 700).")]
        public StringField DescriptionWeight { get; set; }

        [Field(Title = "Description Alignment", Description = "Text alignment (left, center, right).")]
        public StringField DescriptionAlignment { get; set; } = new() { Value = "left" };

        [Field(Title = "Header Text Alignment", Description = "Alignment of all header texts (left, center, right).")]
        public StringField HeaderTextAlign { get; set; } = new() { Value = "left" };

        // =========================================================================
        // VISIBILITY & RESPONSIVE
        // =========================================================================

        [Header("Visibility")]

        // NEW: Hide on specific devices
        [Field(Title = "Hide on Mobile", Description = "Hide this section on mobile devices (sm and below).")]
        public BoolField HideOnMobile { get; set; } = new() { Value = "false" };

        [Field(Title = "Hide on Tablet", Description = "Hide this section on tablet devices (md).")]
        public BoolField HideOnTablet { get; set; } = new() { Value = "false" };

        [Field(Title = "Hide on Desktop", Description = "Hide this section on desktop devices (lg and above).")]
        public BoolField HideOnDesktop { get; set; } = new() { Value = "false" };

        // =========================================================================
        // ADVANCED / CUSTOMIZATION
        // =========================================================================

        [Header("Advanced")]

        // NEW: Custom CSS classes
        [Field(Title = "Custom CSS Classes", Description = "Additional CSS classes for the section wrapper.")]
        public StringField CustomClasses { get; set; } = new();

        // NEW: Custom inline styles
        [Field(Title = "Custom Styles", Description = "Additional inline CSS styles (e.g., 'background: linear-gradient(...);').")]
        public TextAreaField CustomStyles { get; set; } = new();

        // NEW: Data attributes for JS
        [Field(Title = "Data Attributes", Description = "JSON object of data-* attributes (e.g., {\"data-gsap\":\"fade\",\"data-speed\":\"1\"}).")]
        public TextAreaField DataAttributes { get; set; } = new();

        // NEW: Enable sticky positioning
        [Field(Title = "Sticky Section", Description = "Make the entire section sticky on scroll.")]
        public BoolField StickySection { get; set; } = new() { Value = "false" };

        // NEW: Sticky top offset
        [Field(Title = "Sticky Top Offset", Description = "Top offset for sticky section (e.g., 0, 20px, 5rem).")]
        public StringField StickyTopOffset { get; set; } = new() { Value = "0" };
    }
}
