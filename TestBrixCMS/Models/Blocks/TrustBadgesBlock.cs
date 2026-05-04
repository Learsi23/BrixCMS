using TestBrixCMS.Models.Base;
using TestBrixCMS.Data.Fields;

namespace TestBrixCMS.Models.Blocks
{
    [BlockType(Name = "Trust Badges", Category = "Ecommerce", Icon = "fas fa-shield-halved",
        Description = "Row of e-commerce trust badges: free shipping, secure payment, easy returns, etc.")]
    public class TrustBadgesBlock : BlockBase
    {
        [Header("Badges (JSON)")]
        [Field(Title = "Badges JSON",
            Description = "Array of { icon, label, sublabel } objects. Icon is a FontAwesome class.",
            Placeholder = "[]")]
        public TextAreaField BadgesJson { get; set; } = new()
        {
            Value = """
[
  { "icon": "fas fa-truck",          "label": "Free Shipping",    "sublabel": "On orders over €50" },
  { "icon": "fas fa-rotate-left",    "label": "Easy Returns",     "sublabel": "30-day return policy" },
  { "icon": "fas fa-lock",           "label": "Secure Payment",   "sublabel": "SSL encrypted checkout" },
  { "icon": "fas fa-headset",        "label": "24/7 Support",     "sublabel": "We're here to help" }
]
"""
        };

        [Header("Style")]
        [Field(Title = "Icon Color")]
        public ColorField IconColor { get; set; } = new() { Value = "#111827" };

        [Field(Title = "Background Color")]
        public ColorField BackgroundColor { get; set; } = new() { Value = "#f9fafb" };

        [Field(Title = "Border")]
        public BoolField ShowBorder { get; set; } = new() { Value = "true" };
    }
}
