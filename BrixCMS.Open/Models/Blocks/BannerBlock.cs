using BrixCMS.Open.Models.Base;
using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.Blocks
{
    [BlockType(Name = "Banner / Announcement", Category = "Layout", Icon = "fas fa-bullhorn",
        Description = "Announcement or notification bar: free shipping, active discount, new feature. Can be closed.")]
    public class BannerBlock : BlockBase
    {
        [Header("Content")]
        [Field(Title = "Icon (Emoji or FontAwesome)", Description = "Icon to display, e.g., emoji (🎉) or FontAwesome class (fas fa-truck).", Placeholder = "🎉 or fas fa-truck")]
        public StringField Icon { get; set; } = new() { Value = "🎉" };

        [Field(Title = "Announcement Text", Description = "Text for the announcement, e.g., 'Free shipping on orders over €50'.", Placeholder = "e.g., Free shipping on orders over €50")]
        public StringField Text { get; set; } = new();

        [Field(Title = "Link Text", Description = "Text for the link, e.g., 'Learn more'.", Placeholder = "e.g., Learn more")]
        public StringField LinkText { get; set; } = new();

        [Field(Title = "Link URL", Description = "URL for the link.")]
        public StringField LinkUrl { get; set; } = new();

        [Header("Style")]
        [Field(Title = "Background Color", Description = "Background color of the banner.")]
        public ColorField BackgroundColor { get; set; } = new() { Value = "#1f2937" };

        [Field(Title = "Text Color", Description = "Color of the announcement text.")]
        public ColorField TextColor { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Closeable", Description = "If true, the banner can be closed by the user.")]
        public BoolField Closeable { get; set; } = new() { Value = "true" };
    }
}
