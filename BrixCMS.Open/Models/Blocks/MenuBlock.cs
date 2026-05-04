using BrixCMS.Open.Models.Base;
using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.Blocks
{
    [BlockType(Name = "Restaurant Menu", Category = "Content", Icon = "fas fa-utensils",
        Description = "Display a restaurant menu with categories, dish names, descriptions, and prices.")]
    public class MenuBlock : BlockBase
    {
        [Header("Header")]
        [Field(Title = "Section Title", Placeholder = "Our Menu")]
        public StringField Title { get; set; } = new() { Value = "Our Menu" };

        [Field(Title = "Subtitle", Placeholder = "Fresh ingredients, prepared with passion.")]
        public StringField Subtitle { get; set; } = new() { Value = "Fresh ingredients, prepared with passion." };

        [Header("Menu Items")]
        [Field(Title = "Menu Content",
               Description = "Write [Category Name] to start a section. Then one item per line: Name | Description | Price | Badge (badge is optional). Leave a blank line between sections.",
               Placeholder = "[Starters]\nGarlic Bread | Toasted ciabatta with herb butter | €5\nSoup of the Day | Ask your server | €7 | Chef's pick\n\n[Mains]\nGrilled Chicken | Free-range chicken with seasonal vegetables | €16\nPasta Primavera | Fresh pasta with roasted vegetables | €14 | Vegan")]
        public TextAreaField MenuContent { get; set; } = new()
        {
            Value = """
[Starters]
Garlic Bread | Toasted ciabatta with herb butter | €5
Soup of the Day | Ask your server | €7 | Chef's pick

[Mains]
Grilled Chicken | Free-range chicken with seasonal vegetables | €16
Pasta Primavera | Fresh pasta with roasted vegetables and pesto | €14 | Vegan
""",
        };

        [Header("Style")]
        [Field(Title = "Accent Color")]
        public ColorField AccentColor { get; set; } = new() { Value = "#b45309" };

        [Field(Title = "Show Dividers")]
        public BoolField ShowDividers { get; set; } = new() { Value = "true" };
    }
}
