using BrixCMS.Open.Data.Fields;
using BrixCMS.Open.Models.Base;

namespace BrixCMS.Open.Models.Blocks
{
    [BlockType(Name = "Before / After Image", Category = "Media", Icon = "fas fa-columns", Description = "Image comparison slider. Perfect for showing transformations, before/after results, or product comparisons.")]
    public class BeforeAfterBlock : BlockBase
    {
        [Field(Title = "Before Image", Description = "The 'before' image (left side)")]
        public ImageField BeforeImage { get; set; } = new();

        [Field(Title = "After Image", Description = "The 'after' image (right side)")]
        public ImageField AfterImage { get; set; } = new();

        [Field(Title = "Before Label", Placeholder = "e.g. Before, Original")]
        public StringField BeforeLabel { get; set; } = new() { Value = "Before" };

        [Field(Title = "After Label", Placeholder = "e.g. After, Result")]
        public StringField AfterLabel { get; set; } = new() { Value = "After" };

        [Field(Title = "Label Text Color")]
        public ColorField LabelColor { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Label Background")]
        public ColorField LabelBackground { get; set; } = new() { Value = "#000000" };

        [Field(Title = "Slider Orientation")]
        public SelectField<string> Orientation { get; set; } = new()
        {
            Value = "horizontal",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "horizontal", Label = "Horizontal" },
                new() { Value = "vertical", Label = "Vertical" }
            }
        };

        [Field(Title = "Starting Position (%)", Placeholder = "0-100")]
        public StringField StartingPosition { get; set; } = new() { Value = "50" };

        [Field(Title = "Max Width", Placeholder = "e.g. 800px, 100%")]
        public StringField Width { get; set; } = new() { Value = "100%" };

        [Field(Title = "Height", Placeholder = "e.g. 500px, auto")]
        public StringField Height { get; set; } = new() { Value = "500px" };

        [Field(Title = "Border Radius", Placeholder = "0, 8px, 1rem")]
        public StringField BorderRadius { get; set; } = new() { Value = "0" };

        [Field(Title = "Section ID (anchor)")]
        public StringField SectionId { get; set; } = new();

        [Field(Title = "Vertical Padding", Placeholder = "2rem")]
        public StringField PaddingY { get; set; } = new() { Value = "2rem" };
    }
}
