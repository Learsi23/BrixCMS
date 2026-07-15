using BrixCMS.Open.Models.Base;
using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.Blocks
{
    [BlockType(Name = "Tab Item", Category = "Interactive", Icon = "fas fa-file-alt",
        Description = "A tab with a title and HTML content. Use within the Tabs block.", Child = true)]
    public class TabItemBlock : BlockBase
    {
        [Field(Title = "Tab Title", Description = "Title of the tab.")]
        public StringField TabLabel { get; set; } = new();

        [Field(Title = "Icon (FontAwesome)", Description = "FontAwesome icon class, e.g., 'fas fa-star'.", Placeholder = "fas fa-star")]
        public StringField TabIcon { get; set; } = new();

        [Field(Title = "Content (HTML allowed)", Description = "HTML content for the tab.")]
        public TextAreaField Content { get; set; } = new() { Rows = 8 };
    }
}
