using TestBrixCMS.Models.Base;
using TestBrixCMS.Data.Fields;

namespace TestBrixCMS.Models.Blocks
{
    [BlockType(Name = "Timeline / Steps", Category = "Content", Icon = "fas fa-stream",
        Description = "Timeline of steps or milestones. Ideal for 'How it works', company history, or process.")]
    public class TimelineBlock : BlockGroupBase
    {
        [Header("Texts")]
        [Field(Title = "Title", Description = "Main title for the timeline section.")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Title Color", Description = "Color of the title text.")]
        public ColorField TitleColor { get; set; } = new();

        [Field(Title = "Subtitle", Description = "Subtitle or description for the timeline section.")]
        public StringField Subtitle { get; set; } = new();

        [Header("Style")]
        [Field(Title = "Background Color", Description = "Background color of the timeline block.")]
        public ColorField BackgroundColor { get; set; } = new();

        [Field(Title = "Connector Color", Description = "Color of the timeline connector line.")]
        public ColorField ConnectorColor { get; set; } = new() { Value = "#3b82f6" };

        [Field(Title = "Layout", Description = "Layout of the timeline (vertical or horizontal).", Placeholder = "vertical | horizontal")]
        public StringField Layout { get; set; } = new() { Value = "vertical" };

        [Field(Title = "Text Alignment", Description = "Alignment of the text in the timeline.", Placeholder = "left | center")]
        public StringField TextAlign { get; set; } = new() { Value = "left" };
    }
}
