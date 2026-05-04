using BrixCMS.Open.Models.Base;
using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.Blocks
{
    [BlockType(Name = "Trust Badges", Category = "Ecommerce", Icon = "fas fa-shield-halved",
        Description = "Row of e-commerce trust badges: free shipping, secure payment, easy returns, etc.")]
    public class TrustBadgesBlock : BlockBase
    {
        // ── Badge 1 ──────────────────────────────────────────────────────────
        [Header("Badge 1")]
        [Field(Title = "Icon (FontAwesome class)", Placeholder = "fas fa-truck")]
        public StringField Badge1Icon { get; set; } = new() { Value = "fas fa-truck" };

        [Field(Title = "Label", Placeholder = "Free Shipping")]
        public StringField Badge1Label { get; set; } = new() { Value = "Free Shipping" };

        [Field(Title = "Sub-label", Placeholder = "On orders over €50")]
        public StringField Badge1Sublabel { get; set; } = new() { Value = "On orders over €50" };

        // ── Badge 2 ──────────────────────────────────────────────────────────
        [Header("Badge 2")]
        [Field(Title = "Icon (FontAwesome class)", Placeholder = "fas fa-rotate-left")]
        public StringField Badge2Icon { get; set; } = new() { Value = "fas fa-rotate-left" };

        [Field(Title = "Label", Placeholder = "Easy Returns")]
        public StringField Badge2Label { get; set; } = new() { Value = "Easy Returns" };

        [Field(Title = "Sub-label", Placeholder = "30-day return policy")]
        public StringField Badge2Sublabel { get; set; } = new() { Value = "30-day return policy" };

        // ── Badge 3 ──────────────────────────────────────────────────────────
        [Header("Badge 3")]
        [Field(Title = "Icon (FontAwesome class)", Placeholder = "fas fa-lock")]
        public StringField Badge3Icon { get; set; } = new() { Value = "fas fa-lock" };

        [Field(Title = "Label", Placeholder = "Secure Payment")]
        public StringField Badge3Label { get; set; } = new() { Value = "Secure Payment" };

        [Field(Title = "Sub-label", Placeholder = "SSL encrypted checkout")]
        public StringField Badge3Sublabel { get; set; } = new() { Value = "SSL encrypted checkout" };

        // ── Badge 4 ──────────────────────────────────────────────────────────
        [Header("Badge 4")]
        [Field(Title = "Icon (FontAwesome class)", Placeholder = "fas fa-headset")]
        public StringField Badge4Icon { get; set; } = new() { Value = "fas fa-headset" };

        [Field(Title = "Label", Placeholder = "24/7 Support")]
        public StringField Badge4Label { get; set; } = new() { Value = "24/7 Support" };

        [Field(Title = "Sub-label", Placeholder = "We're here to help")]
        public StringField Badge4Sublabel { get; set; } = new() { Value = "We're here to help" };

        // ── Style ─────────────────────────────────────────────────────────────
        [Header("Style")]
        [Field(Title = "Icon Color")]
        public ColorField IconColor { get; set; } = new() { Value = "#111827" };

        [Field(Title = "Background Color")]
        public ColorField BackgroundColor { get; set; } = new() { Value = "#f9fafb" };

        [Field(Title = "Show Border")]
        public BoolField ShowBorder { get; set; } = new() { Value = "true" };
    }
}
