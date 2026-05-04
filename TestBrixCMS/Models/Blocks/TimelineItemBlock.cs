using TestBrixCMS.Models.Base;
using TestBrixCMS.Data.Fields;

namespace TestBrixCMS.Models.Blocks
{
    [BlockType(Name = "Timeline Item", Category = "Content", Icon = "fas fa-circle",
        Description = "A step or milestone in the timeline. Use within a Timeline block.")]
    public class TimelineItemBlock : BlockBase
    {
        [Field(Title = "Step Label / Number", Description = "Label or number for the step, e.g., '01' or 'Step 1'.", Placeholder = "e.g., 01 or Step 1")]
        public StringField StepLabel { get; set; } = new();

        [Field(Title = "Icon (FontAwesome)", Description = "FontAwesome icon class for the step.", Placeholder = "fas fa-rocket")]
        public StringField Icon { get; set; } = new();

        [Field(Title = "Date / Period", Description = "Date or period for the step, e.g., 'January 2024'.", Placeholder = "e.g., January 2024")]
        public StringField Date { get; set; } = new();

        [Field(Title = "Step Title", Description = "Title of the step.")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Description", Description = "Description of the step.")]
        public TextAreaField Description { get; set; } = new() { Rows = 3 };

        [Field(Title = "Accent Color", Description = "Accent color for the step.")]
        public ColorField AccentColor { get; set; } = new() { Value = "#3b82f6" };
    }
}
