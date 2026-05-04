using TestBrixCMS.Models.Base;
using TestBrixCMS.Data.Fields;

namespace TestBrixCMS.Models.ColumnBlocks
{
    [BlockType(Name = "Columns", Category = "Layout", Icon = "fas fa-columns",
        Description = "Multi-column layout container. Add any blocks inside each column for flexible page layouts.")]
    public class ColumnBlock : BlockGroupBase
    {
        // Background base color
        [Field(Title = "Background Color", Description = "Background color of the section.")]
        public ColorField BackgroundColor { get; set; } = new();

        // Background image
        [Field(Title = "Background Image", Description = "Background image for the section.")]
        public ImageField BackgroundImage { get; set; } = new();

        // Overlay opacity (0 - 1)
        [Field(Title = "Overlay Opacity (0-1)", Description = "Opacity of the background overlay.", Placeholder = "e.g., 0.5")]
        public StringField BackgroundOverlayOpacity { get; set; } = new() { Value = "0" };

        // Overlay color
        [Field(Title = "Overlay Color", Description = "Color of the background overlay.")]
        public ColorField BackgroundOverlayColor { get; set; } = new() { Value = "#000000" };

        // Gap between columns
        [Field(Title = "Gap Between Columns", Description = "Spacing between columns (Tailwind gap classes).")]
        public StringField Gap { get; set; } = new() { Value = "gap-6" };

        // Enable parallax effect
        [Field(Title = "Enable Parallax", Description = "Background image will move slower on scroll.")]
        public BoolField EnableParallax { get; set; } = new() { Value = "false" };

        // NEW: Column layout type
        [Field(Title = "Column Layout", Description = "How columns are distributed.")]
        public StringField ColumnLayout { get; set; } = new() { Value = "auto" };
        // Options: "auto" (automatic based on count), "centered" (single column centered), "full" (single column full width)
    }
}
