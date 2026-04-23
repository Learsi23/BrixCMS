using BrixCMS.Open.Data.Fields;
using BrixCMS.Open.Models.Base;

namespace BrixCMS.Open.Models.Blocks
{
    [BlockType(Name = "QR Code", Category = "Media", Icon = "fas fa-qrcode", Description = "QR code generator. Perfect for linking to URLs, payments, WiFi access, or contact info.")]
    public class QRCodeBlock : BlockBase
    {
        [Field(Title = "QR Content", Placeholder = "https://example.com, WiFi network, payment link")]
        public StringField Content { get; set; } = new();

        [Field(Title = "Content Type")]
        public SelectField<string> ContentType { get; set; } = new()
        {
            Value = "url",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "url", Label = "URL/Link" },
                new() { Value = "wifi", Label = "WiFi Network" },
                new() { Value = "vcard", Label = "Contact (vCard)" },
                new() { Value = "email", Label = "Email Address" },
                new() { Value = "phone", Label = "Phone Number" },
                new() { Value = "text", Label = "Plain Text" }
            }
        };

        [Field(Title = "QR Size (px)", Placeholder = "200")]
        public StringField Size { get; set; } = new() { Value = "200" };

        [Field(Title = "Foreground Color")]
        public ColorField ForegroundColor { get; set; } = new() { Value = "#000000" };

        [Field(Title = "Background Color")]
        public ColorField BackgroundColor { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "QR Style")]
        public SelectField<string> Style { get; set; } = new()
        {
            Value = "square",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "square", Label = "Square" },
                new() { Value = "rounded", Label = "Rounded" },
                new() { Value = "dots", Label = "Dots" }
            }
        };

        [Field(Title = "Include Label Below")]
        public SelectField<string> IncludeLabel { get; set; } = new()
        {
            Value = "false",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "false", Label = "No" },
                new() { Value = "true", Label = "Yes" }
            }
        };

        [Field(Title = "Label Text", Placeholder = "e.g. Scan to visit")]
        public StringField Label { get; set; }

        [Field(Title = "Label Color")]
        public ColorField LabelColor { get; set; } = new() { Value = "#000000" };

        [Field(Title = "Border Radius", Placeholder = "8px")]
        public StringField BorderRadius { get; set; } = new() { Value = "8px" };

        [Field(Title = "Border Width (px)", Placeholder = "0")]
        public StringField BorderWidth { get; set; } = new() { Value = "0" };

        [Field(Title = "Alignment")]
        public SelectField<string> Align { get; set; } = new()
        {
            Value = "center",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "left", Label = "Left" },
                new() { Value = "center", Label = "Center" },
                new() { Value = "right", Label = "Right" }
            }
        };

        [Field(Title = "Section ID (anchor)")]
        public StringField SectionId { get; set; } = new();
    }
}
