using TestBrixCMS.Models.Base;
using TestBrixCMS.Data.Fields;

namespace TestBrixCMS.Models.Blocks
{
    [BlockType(Name = "Countdown", Category = "Interactive", Icon = "fas fa-hourglass-half",
        Description = "Countdown timer for offers, launches, or events. Set the target date.")]
    public class CountdownBlock : BlockBase
    {
        [Header("Content")]
        [Field(Title = "Title", Description = "Title for the countdown, e.g., 'Offer ends soon!'", Placeholder = "e.g., Offer ends soon!")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Target Date (ISO)", Description = "Target date in ISO format, e.g., '2025-12-31T23:59:59'.", Placeholder = "e.g., 2025-12-31T23:59:59")]
        public StringField TargetDate { get; set; } = new();

        [Field(Title = "Text When Ended", Description = "Text to display when the countdown ends.", Placeholder = "e.g., It's over!")]
        public StringField OnEndText { get; set; } = new() { Value = "It's over!" };

        [Header("Style")]
        [Field(Title = "Background Color", Description = "Background color of the countdown block.")]
        public ColorField BackgroundColor { get; set; } = new();

        [Field(Title = "Text Color", Description = "Color of the countdown title and labels.")]
        public ColorField TextColor { get; set; } = new() { Value = "#1f2937" };

        [Field(Title = "Digit Background Color", Description = "Background color of the countdown digits.")]
        public ColorField DigitBgColor { get; set; } = new() { Value = "#1f2937" };

        [Field(Title = "Digit Color", Description = "Color of the countdown digits.")]
        public ColorField DigitColor { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Show Labels (Days, Hrs…)", Description = "If true, labels like 'Days', 'Hours' will be displayed.")]
        public BoolField ShowLabels { get; set; } = new() { Value = "true" };
    }
}
