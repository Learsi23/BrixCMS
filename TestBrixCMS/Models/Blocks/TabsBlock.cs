using TestBrixCMS.Models.Base;
using TestBrixCMS.Data.Fields;

namespace TestBrixCMS.Models.Blocks
{
    [BlockType(Name = "Tabs", Category = "Interactive", Icon = "fas fa-folder-open",
        Description = "Content organized in tabs. Ideal for Description / Specifications / Reviews.")]
    public class TabsBlock : BlockGroupBase
    {
        [Header("Style")]
        [Field(Title = "Background Color", Description = "Background color of the tabs block.")]
        public ColorField BackgroundColor { get; set; } = new();

        [Field(Title = "Active Tab Color", Description = "Background color of the active tab.")]
        public ColorField TabActiveColor { get; set; } = new() { Value = "#3b82f6" };

        [Field(Title = "Active Tab Text Color", Description = "Text color of the active tab.")]
        public ColorField TabActiveTextColor { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Inactive Tab Text Color", Description = "Text color of inactive tabs.")]
        public ColorField TabInactiveTextColor { get; set; } = new() { Value = "#6b7280" };
    }
}
