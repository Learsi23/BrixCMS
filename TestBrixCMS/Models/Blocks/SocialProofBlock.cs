using TestBrixCMS.Models.Base;
using TestBrixCMS.Data.Fields;

namespace TestBrixCMS.Models.Blocks
{
    [BlockType(Name = "Social Proof / Logos", Category = "Content", Icon = "fas fa-award",
        Description = "Client logos + number of Google or Trustpilot reviews. Builds trust.")]
    public class SocialProofBlock : BlockBase
    {
        [Header("Text")]
        [Field(Title = "Title", Description = "Title for the social proof section.")]
        public StringField Title { get; set; } = new();

        [Header("Logos (Comma-separated URLs)")]
        [Field(Title = "Logo URLs", Description = "URLs of client logos, separated by commas.", Placeholder = "https://... , https://... , https://...")]
        public TextAreaField LogoUrls { get; set; } = new() { Rows = 3 };

        [Field(Title = "Logo Height", Description = "Height of the logos, e.g., '40px'.", Placeholder = "e.g., 40px")]
        public StringField LogoHeight { get; set; } = new() { Value = "40px" };

        [Header("Review Badge")]
        [Field(Title = "Review Count", Description = "Number of reviews, e.g., '2,400+'.", Placeholder = "e.g., 2,400+")]
        public StringField ReviewCount { get; set; } = new();

        [Field(Title = "Average Score", Description = "Average review score, e.g., '4.9'.", Placeholder = "e.g., 4.9")]
        public StringField ReviewScore { get; set; } = new();

        [Field(Title = "Review Platform", Description = "Platform for reviews, e.g., 'Google' or 'Trustpilot'.", Placeholder = "Google | Trustpilot")]
        public StringField ReviewSource { get; set; } = new() { Value = "Google" };

        [Header("Style")]
        [Field(Title = "Background Color", Description = "Background color of the social proof block.")]
        public ColorField BackgroundColor { get; set; } = new();

        [Field(Title = "Text Color", Description = "Color of the text in the social proof block.")]
        public ColorField TextColor { get; set; } = new() { Value = "#6b7280" };
    }
}
