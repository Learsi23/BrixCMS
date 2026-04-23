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

        [Header("Menu Items (JSON)")]
        [Field(Title = "Menu JSON",
            Description = "JSON array of menu sections. Each section: { \"category\": \"Starters\", \"items\": [{ \"name\": \"...\", \"description\": \"...\", \"price\": \"€8\", \"badge\": \"New\" }] }",
            Placeholder = "[]")]
        public TextAreaField MenuJson { get; set; } = new()
        {
            Value = """
[
  {
    "category": "Starters",
    "items": [
      { "name": "Garlic Bread", "description": "Toasted ciabatta with herb butter", "price": "€5" },
      { "name": "Soup of the Day", "description": "Ask your server", "price": "€7", "badge": "Chef's pick" }
    ]
  },
  {
    "category": "Mains",
    "items": [
      { "name": "Grilled Chicken", "description": "Free-range chicken with seasonal vegetables", "price": "€16" },
      { "name": "Pasta Primavera", "description": "Fresh pasta with roasted vegetables and pesto", "price": "€14", "badge": "Vegan" }
    ]
  }
]
"""
        };

        [Header("Style")]
        [Field(Title = "Accent Color")]
        public ColorField AccentColor { get; set; } = new() { Value = "#b45309" };

        [Field(Title = "Show Dividers")]
        public BoolField ShowDividers { get; set; } = new() { Value = "true" };
    }
}
