using BrixCMS.Open.Models.Base;
using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.Blocks
{
    [BlockType(Name = "Button", Category = "Interactive", Icon = "fas fa-hand-pointer",
        Description = "A single button — text, link, and full styling. Use inside a Buttons Group block.", Child = true)]
    public class ButtonItemBlock : BlockBase
    {
        [Field(Title = "Text")]
        public StringField Text { get; set; } = new() { Value = "Click here" };

        [Field(Title = "URL")]
        public StringField Url { get; set; } = new() { Value = "#" };

        [Field(Title = "Open in new tab")]
        public BoolField NewTab { get; set; } = new() { Value = "false" };

        [Field(Title = "Background Color")]
        public ColorField BgColor { get; set; } = new() { Value = "#3b82f6" };

        [Field(Title = "Text Color")]
        public ColorField TextColor { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Hover Background Color")]
        public ColorField HoverBgColor { get; set; } = new() { Value = "#2563eb" };

        [Field(Title = "Border Radius", Description = "CSS border-radius value, e.g. 0.5rem, 9999px for a pill shape.")]
        public StringField BorderRadius { get; set; } = new() { Value = "0.5rem" };

        [Field(Title = "Padding", Description = "CSS padding value, e.g. 0.75rem 1.75rem.")]
        public StringField Padding { get; set; } = new() { Value = "0.75rem 1.75rem" };

        [Field(Title = "Font Size", Description = "CSS font-size value, e.g. 1rem.")]
        public StringField FontSize { get; set; } = new() { Value = "1rem" };

        [Field(Title = "Icon (FontAwesome)", Placeholder = "fas fa-arrow-right")]
        public StringField IconClass { get; set; } = new();
    }
}
