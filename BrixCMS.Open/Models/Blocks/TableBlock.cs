using BrixCMS.Open.Data.Fields;
using BrixCMS.Open.Models.Base;

namespace BrixCMS.Open.Models.Blocks
{
    [BlockType(Name = "Table", Category = "Content", Icon = "fas fa-table", Description = "Simple data table. Different from comparison table - optimized for displaying structured data.")]
    public class TableBlock : BlockBase
    {
        [Field(Title = "Table Content",
               Description = "First line = column headers. Each next line = one row. Separate columns with the pipe character |",
               Placeholder = "Product | Price | Status\nApple | €1.50 | Available\nBanana | €0.80 | Available")]
        public TextAreaField TableData { get; set; } = new()
        {
            Value = "Product | Price | Status\nItem 1 | €10.00 | Available\nItem 2 | €20.00 | Available",
        };

        [Field(Title = "Table Caption", Placeholder = "Optional table title")]
        public StringField Caption { get; set; }

        [Field(Title = "Table Style")]
        public SelectField<string> Style { get; set; } = new()
        {
            Value = "striped",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "default", Label = "Default" },
                new() { Value = "striped", Label = "Striped" },
                new() { Value = "bordered", Label = "Bordered" },
                new() { Value = "minimal", Label = "Minimal" }
            }
        };

        [Field(Title = "Header Background")]
        public ColorField HeaderBackground { get; set; } = new() { Value = "#f3f4f6" };

        [Field(Title = "Header Text Color")]
        public ColorField HeaderColor { get; set; } = new() { Value = "#000000" };

        [Field(Title = "Row Background")]
        public ColorField RowBackground { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Alternate Row Background")]
        public ColorField RowAlternateBackground { get; set; } = new() { Value = "#f9fafb" };

        [Field(Title = "Border Color")]
        public ColorField BorderColor { get; set; } = new() { Value = "#e5e7eb" };

        [Field(Title = "Cell Padding", Placeholder = "12px")]
        public StringField CellPadding { get; set; } = new() { Value = "12px" };

        [Field(Title = "Text Alignment")]
        public SelectField<string> TextAlign { get; set; } = new()
        {
            Value = "left",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "left", Label = "Left" },
                new() { Value = "center", Label = "Center" },
                new() { Value = "right", Label = "Right" }
            }
        };

        [Field(Title = "Scroll on Mobile")]
        public SelectField<string> MobileScroll { get; set; } = new()
        {
            Value = "true",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "true", Label = "Yes (horizontal scroll)" },
                new() { Value = "false", Label = "No" }
            }
        };

        [Field(Title = "Section ID (anchor)")]
        public StringField SectionId { get; set; } = new();
    }
}
