using TestBrixCMS.Data.Fields;
using TestBrixCMS.Models.Base;
using System.Collections.Generic;

namespace TestBrixCMS.Models.ColumnBlocks
{
    [BlockType(Name = "Dynamic Grid Layout", Category = "Layout", Icon = "fas fa-th",
        Description = "Flexible CSS grid with configurable rows and columns. Each row can have different column counts.")]
    public class GridColumn : BlockGroupBase
    {
        // ── Background Section ─────────────────────────────────────────────
        [Header("Background")]
        [Field(Title = "Background Color")]
        public ColorField BackgroundColor { get; set; } = new();

        [Field(Title = "Background Image")]
        public ImageField BackgroundImage { get; set; } = new();

        [Field(Title = "Overlay Opacity (0-1)", Placeholder = "0.5")]
        public StringField BackgroundOverlayOpacity { get; set; } = new() { Value = "0" };

        [Field(Title = "Overlay Color")]
        public ColorField BackgroundOverlayColor { get; set; } = new() { Value = "#000000" };

        // ── Layout Configuration ────────────────────────────────────────────
        [Header("Grid Layout")]

        [Field(Title = "Max Columns", Description = "Number of columns on desktop (1–6). Automatically collapses on mobile/tablet. Simpler than Row Layouts JSON — use this for standard grids.", Placeholder = "1, 2, 3, 4, 5 or 6")]
        public StringField MaxColumns { get; set; } = new() { Value = "3" };

        [Field(Title = "Row Layouts (JSON — Advanced)", Description = @"
                Advanced: define custom columns per row. Leave empty to use Max Columns above.
                Example: [{ 'columns': 3, 'breakpoint': 'lg' }, { 'columns': 2, 'breakpoint': 'lg' }]")]
        public TextAreaField RowLayoutsJson { get; set; } = new();

        [Field(Title = "Gap Between Items", Placeholder = "gap-4, gap-6, gap-8")]
        public StringField Gap { get; set; } = new() { Value = "gap-6" };

        [Field(Title = "Vertical Padding", Placeholder = "3rem, 48px, 2em")]
        public StringField PaddingY { get; set; } = new() { Value = "3rem" };

        [Field(Title = "Horizontal Padding", Placeholder = "1.5rem, 24px, 2em")]
        public StringField PaddingX { get; set; } = new() { Value = "1.5rem" };

        [Field(Title = "Items Alignment", Placeholder = "start | center | end | stretch")]
        public StringField ItemsAlign { get; set; } = new() { Value = "stretch" };

        [Field(Title = "Section ID (Anchor)", Placeholder = "e.g., services, team, contact")]
        public StringField SectionId { get; set; } = new();

        // ── Responsive Overrides ──────────────────────────────────────────
        [Header("Responsive Overrides")]

        [Field(Title = "Mobile Columns (Default)", Description = "Fallback columns for mobile (1-2)", Placeholder = "1 or 2")]
        public StringField MobileDefaultCols { get; set; } = new() { Value = "1" };

        [Field(Title = "Tablet Columns (Default)", Description = "Fallback columns for tablet when row not specified", Placeholder = "1, 2, 3 or 4")]
        public StringField TabletDefaultCols { get; set; } = new() { Value = "2" };

        // ── Section Header ─────────────────────────────────────────────────
        [Header("Section Header")]
        [Field(Title = "Header Text", Placeholder = "e.g., Our Services")]
        public StringField TitleSida { get; set; } = new();

        [Field(Title = "Header Color")]
        public ColorField TitleSidaColor { get; set; } = new();

        [Field(Title = "Header Alignment", Placeholder = "left | center | right")]
        public StringField TitleSidaAlign { get; set; } = new() { Value = "left" };

        // ── Main Title ─────────────────────────────────────────────────────
        [Header("Main Title")]
        [Field(Title = "Title", Placeholder = "e.g., What We Offer")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Title Color")]
        public ColorField TitleColor { get; set; } = new();

        [Field(Title = "Title Weight", Description = "Font weight (normal, bold, 400, 600, 700, 800, 900).", Placeholder = "normal, bold, 700, 800, 900")]
        public StringField TitleWeight { get; set; } = new() { Value = "900" };

        [Field(Title = "Title Line Height", Description = "Line height of the title.", Placeholder = "1.2, 1.4, 1.5, 2")]
        public StringField TitleLineHeight { get; set; } = new() { Value = "1.2" };

        [Field(Title = "Subtitle", Placeholder = "e.g., Custom Solutions")]
        public StringField SubTitle { get; set; } = new();

        [Field(Title = "Subtitle Color")]
        public ColorField SubtitleColor { get; set; } = new();

        [Field(Title = "Subtitle Weight", Description = "Font weight of the subtitle.", Placeholder = "normal, bold, 400, 500, 600")]
        public StringField SubtitleWeight { get; set; } = new() { Value = "normal" };

        [Field(Title = "Description", Placeholder = "e.g., Detailed description of our services...")]
        public TextAreaField Description { get; set; } = new();

        [Field(Title = "Description Color")]
        public ColorField DescriptionColor { get; set; } = new();

        [Field(Title = "Description Weight", Description = "Font weight (normal, bold, 400, 600, 700, 800, 900).", Placeholder = "normal, bold, 400, 500, 600")]
        public StringField DescriptionWeight { get; set; } = new() { Value = "normal" };

        [Field(Title = "Description Line Height", Description = "Line height of the description.", Placeholder = "1.4, 1.5, 1.6, 2")]
        public StringField DescriptionLineHeight { get; set; } = new() { Value = "1.5" };

        [Field(Title = "Text Alignment", Placeholder = "left | center | right")]
        public StringField HeaderTextAlign { get; set; } = new() { Value = "left" };

        // ── Per-Column Styling ─────────────────────────────────────────────
        [Header("Per-Column Styling")]
        [Field(Title = "Column Background Color", Placeholder = "e.g. #ffffff, #1e293b or transparent")]
        public ColorField ColumnBackgroundColor { get; set; } = new();

        [Field(Title = "Column Border Color", Placeholder = "e.g. #e2e8f0 or #3b82f6")]
        public ColorField ColumnBorderColor { get; set; } = new();

        [Field(Title = "Column Border Width", Placeholder = "e.g. 1px, 2px, or 0")]
        public StringField ColumnBorderWidth { get; set; } = new() { Value = "0" };

        [Field(Title = "Column Border Radius", Placeholder = "e.g. 0.5rem, 8px, or 0")]
        public StringField ColumnBorderRadius { get; set; } = new() { Value = "0" };

        [Field(Title = "Column Padding", Placeholder = "e.g. 0, 1rem, 1.5rem, 2rem")]
        public StringField ColumnPadding { get; set; } = new() { Value = "0" };

        [Field(Title = "Column Shadow", Placeholder = "none | sm | md | lg")]
        public StringField ColumnShadow { get; set; } = new() { Value = "none" };
    }

    // Helper class for JSON deserialization
    public class RowLayout
    {
        public int Columns { get; set; } = 3;
        public string Breakpoint { get; set; } = "lg"; // "md" or "lg"
    }
}
