using BrixCMS.Open.Models.Base;
using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.Blocks
{
    [BlockType(Name = "Newsletter Signup", Category = "Interactive", Icon = "fas fa-envelope-open-text",
        Description = "Email subscription form with a title, subtitle, and customisable call-to-action button.")]
    public class NewsletterBlock : BlockBase
    {
        [Header("Content")]
        [Field(Title = "Heading", Placeholder = "Subscribe to our newsletter")]
        public StringField Heading { get; set; } = new() { Value = "Subscribe to our newsletter" };

        [Field(Title = "Subtitle", Placeholder = "Get the latest news and offers directly in your inbox.")]
        public StringField Subtitle { get; set; } = new() { Value = "Get the latest news and offers directly in your inbox." };

        [Field(Title = "Button Label", Placeholder = "Subscribe")]
        public StringField ButtonLabel { get; set; } = new() { Value = "Subscribe" };

        [Field(Title = "Success Message", Placeholder = "Thanks for subscribing!")]
        public StringField SuccessMessage { get; set; } = new() { Value = "Thanks for subscribing!" };

        [Header("Style")]
        [Field(Title = "Background Color")]
        public ColorField BackgroundColor { get; set; } = new() { Value = "#f9fafb" };

        [Field(Title = "Button Color")]
        public ColorField ButtonColor { get; set; } = new() { Value = "#111827" };

        [Field(Title = "Button Text Color")]
        public ColorField ButtonTextColor { get; set; } = new() { Value = "#ffffff" };
    }
}
