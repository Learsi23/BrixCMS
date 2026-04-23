using BrixCMS.Open.Data.Fields;
using BrixCMS.Open.Models.Base;

namespace BrixCMS.Open.Models.Blocks
{
    [BlockType(Name = "Cookie Banner", Category = "Interactive", Icon = "fas fa-cookie", Description = "GDPR cookie consent banner. Display privacy notice and consent buttons.")]
    public class CookieBannerBlock : BlockBase
    {
        [Field(Title = "Position")]
        public SelectField<string> Position { get; set; } = new()
        {
            Value = "bottom",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "bottom", Label = "Bottom" },
                new() { Value = "top", Label = "Top" },
                new() { Value = "bottom-left", Label = "Bottom Left" },
                new() { Value = "bottom-right", Label = "Bottom Right" }
            }
        };

        [Field(Title = "Background Color")]
        public ColorField BackgroundColor { get; set; } = new() { Value = "#1f2937" };

        [Field(Title = "Text Color")]
        public ColorField TextColor { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Title")]
        public StringField Title { get; set; } = new() { Value = "We use cookies" };

        [Field(Title = "Message")]
        public TextAreaField Message { get; set; } = new() { Value = "We use cookies to enhance your browsing experience and analyze our traffic." };

        [Field(Title = "Privacy Policy URL", Placeholder = "/privacy-policy")]
        public UrlField PrivacyPolicyUrl { get; set; } = new();

        [Field(Title = "Accept Button Text")]
        public StringField AcceptButtonText { get; set; } = new() { Value = "Accept All" };

        [Field(Title = "Decline Button Text")]
        public StringField DeclineButtonText { get; set; } = new() { Value = "Decline" };

        [Field(Title = "Accept Button Color")]
        public ColorField AcceptButtonColor { get; set; } = new() { Value = "#10b981" };

        [Field(Title = "Decline Button Color")]
        public ColorField DeclineButtonColor { get; set; } = new() { Value = "#6b7280" };

        [Field(Title = "Button Text Color")]
        public ColorField ButtonTextColor { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Border Radius", Placeholder = "8px")]
        public StringField BorderRadius { get; set; } = new() { Value = "8px" };

        [Field(Title = "Width", Placeholder = "100%")]
        public StringField Width { get; set; } = new() { Value = "100%" };

        [Field(Title = "Padding", Placeholder = "16px")]
        public StringField Padding { get; set; } = new() { Value = "16px" };

        [Field(Title = "Z-Index", Placeholder = "9999")]
        public StringField ZIndex { get; set; } = new() { Value = "9999" };
    }
}
