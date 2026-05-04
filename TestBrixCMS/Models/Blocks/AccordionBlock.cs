using TestBrixCMS.Models.Base;
using TestBrixCMS.Data.Fields;

namespace TestBrixCMS.Models.Blocks
{
    [BlockType(Name = "Accordion / FAQ", Category = "Interactive", Icon = "fas fa-list-ul",
        Description = "Multi-item collapsible accordion. Perfect for FAQs with multiple questions in one block. Use Accordion Item inside an this block")]
    public class AccordionBlock : BlockGroupBase
    {
        [Header("Style")]
        [Field(Title = "Block Title", Description = "Title displayed at the top of the accordion block.")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Title Color", Description = "Color of the block title text.")]
        public ColorField TitleColor { get; set; } = new();

        [Field(Title = "Background Color", Description = "Background color of the accordion block.")]
        public ColorField BackgroundColor { get; set; } = new();

        [Field(Title = "Active Color (Border/Icon)", Description = "Color used for the active state border and icon.")]
        public ColorField ActiveColor { get; set; } = new() { Value = "#3b82f6" };

        [Field(Title = "Allow Multiple Open", Description = "If true, allows multiple accordion items to be open at the same time.")]
        public BoolField AllowMultiple { get; set; } = new() { Value = "false" };
    }
}
