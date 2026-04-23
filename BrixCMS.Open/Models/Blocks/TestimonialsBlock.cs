using BrixCMS.Open.Models.Base;
using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.Blocks
{
    [BlockType(Name = "Testimonials", Category = "Content", Icon = "fas fa-quote-left",
        Description = "Carousel of customer reviews with avatar, name, role, and stars.")]
    public class TestimonialsBlock : BlockGroupBase
    {
        [Header("Header")]
        [Field(Title = "Title", Description = "Main title for the testimonials section.")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Title Color", Description = "Color of the title text.")]
        public ColorField TitleColor { get; set; } = new();

        [Field(Title = "Subtitle", Description = "Subtitle or description for the testimonials section.")]
        public StringField Subtitle { get; set; } = new();

        [Header("Style")]
        [Field(Title = "Background Color", Description = "Background color of the testimonials block.")]
        public ColorField BackgroundColor { get; set; } = new();

        [Field(Title = "Card Background Color", Description = "Background color of the testimonial cards.")]
        public ColorField CardBgColor { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Star Color", Description = "Color of the stars in the reviews.")]
        public ColorField StarColor { get; set; } = new() { Value = "#f59e0b" };

        [Header("Carousel")]
        [Field(Title = "Auto Play", Description = "If true, the carousel will auto-play.")]
        public BoolField AutoPlay { get; set; } = new() { Value = "true" };

        [Field(Title = "Auto Play Interval (ms)", Description = "Interval in milliseconds for auto-play.", Placeholder = "4000")]
        public StringField AutoPlayInterval { get; set; } = new() { Value = "4000" };
    }
}
