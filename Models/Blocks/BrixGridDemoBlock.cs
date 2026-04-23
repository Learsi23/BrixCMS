using BrixCMS.Open.Data.Fields;
using BrixCMS.Open.Models.Base;

namespace BrixCMS.Open.Models.Blocks
{
    [BlockType(Name = "Brix Grid Demo", Category = "Content", Icon = "fas fa-th",
        Description = "Visual demonstration of the modular block system. Shows a grid of draggable blocks.")]
    public class BrixGridDemoBlock : BlockBase
    {
        [Header("Header")]
        [Field(Title = "Section Title")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Title Color")]
        public ColorField TitleColor { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Title Size", Placeholder = "E.g.: 2rem")]
        public StringField TitleSize { get; set; } = new() { Value = "2rem" };

        [Field(Title = "Subtitle")]
        public StringField Subtitle { get; set; } = new();

        [Field(Title = "Subtitle Color")]
        public ColorField SubtitleColor { get; set; } = new() { Value = "#9ca3af" };

        [Header("Layout")]
        [Field(Title = "Layout", Placeholder = "grid-text (grid left, text right)")]
        public StringField Layout { get; set; } = new() { Value = "grid-text" };

        [Header("Grid Configuration")]
        [Field(Title = "Grid Columns", Placeholder = "E.g.: 4")]
        public StringField GridColumns { get; set; } = new() { Value = "4" };

        [Field(Title = "Grid Rows", Placeholder = "E.g.: 3")]
        public StringField GridRows { get; set; } = new() { Value = "3" };

        [Field(Title = "Gap", Placeholder = "E.g.: 8px")]
        public StringField Gap { get; set; } = new() { Value = "8px" };

        [Header("Block 1 (Top Full Width)")]
        [Field(Title = "Block 1 - Label", Placeholder = "E.g.: Hero Banner")]
        public StringField Block1Label { get; set; } = new();

        [Field(Title = "Block 1 - Span Columns", Placeholder = "E.g.: 4 for full width")]
        public StringField Block1Span { get; set; } = new() { Value = "4" };

        [Field(Title = "Block 1 - Highlight", Description = "If true, this block is highlighted")]
        public BoolField Block1Highlight { get; set; } = new() { Value = "true" };

        [Header("Block 2 (Middle Left - Tall)")]
        [Field(Title = "Block 2 - Label", Placeholder = "E.g.: Product Grid")]
        public StringField Block2Label { get; set; } = new();

        [Field(Title = "Block 2 - Span Columns", Placeholder = "E.g.: 2")]
        public StringField Block2Span { get; set; } = new() { Value = "2" };

        [Field(Title = "Block 2 - Span Rows", Placeholder = "E.g.: 2")]
        public StringField Block2RowSpan { get; set; } = new() { Value = "2" };

        [Header("Block 3")]
        [Field(Title = "Block 3 - Label", Placeholder = "E.g.: Image")]
        public StringField Block3Label { get; set; } = new();

        [Header("Block 4")]
        [Field(Title = "Block 4 - Label", Placeholder = "E.g.: Text")]
        public StringField Block4Label { get; set; } = new();

        [Header("Block 5")]
        [Field(Title = "Block 5 - Label", Placeholder = "E.g.: CTA")]
        public StringField Block5Label { get; set; } = new();

        [Header("Block 6")]
        [Field(Title = "Block 6 - Label", Placeholder = "E.g.: Video")]
        public StringField Block6Label { get; set; } = new();

        [Header("Block 7 (Bottom)")]
        [Field(Title = "Block 7 - Label", Placeholder = "E.g.: Reviews")]
        public StringField Block7Label { get; set; } = new();

        [Field(Title = "Block 7 - Span Columns")]
        public StringField Block7Span { get; set; } = new() { Value = "3" };

        [Header("Block 8 (Footer)")]
        [Field(Title = "Block 8 - Label", Placeholder = "E.g.: Footer")]
        public StringField Block8Label { get; set; } = new();

        [Field(Title = "Block 8 - Span Columns")]
        public StringField Block8Span { get; set; } = new() { Value = "4" };

        [Header("Feature List (Right Side)")]
        [Field(Title = "Show Feature List")]
        public BoolField ShowFeatures { get; set; } = new() { Value = "true" };

        [Field(Title = "Feature 1")]
        public StringField Feature1 { get; set; } = new();

        [Field(Title = "Feature 2")]
        public StringField Feature2 { get; set; } = new();

        [Field(Title = "Feature 3")]
        public StringField Feature3 { get; set; } = new();

        [Header("Style")]
        [Field(Title = "Accent Color")]
        public ColorField AccentColor { get; set; } = new() { Value = "#5B6EF5" };

        [Field(Title = "Background Color")]
        public ColorField BackgroundColor { get; set; } = new();
    }
}
