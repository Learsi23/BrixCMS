using BrixCMS.Open.Models.Base;
using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.Blocks
{
    [BlockType(Name = "Testimonial", Category = "Content", Icon = "fas fa-user-circle",
        Description = "A customer review. Use within the Testimonials block.")]
    public class TestimonialItemBlock : BlockBase
    {
        [Field(Title = "Review Text", Description = "Text of the customer review.")]
        public TextAreaField ReviewText { get; set; } = new() { Rows = 4 };

        [Field(Title = "Customer Name", Description = "Name of the customer.")]
        public StringField Name { get; set; } = new();

        [Field(Title = "Role / Company", Description = "Role or company of the customer.")]
        public StringField Role { get; set; } = new();

        [Field(Title = "Photo (URL)", Description = "URL of the customer's photo.")]
        public ImageField Avatar { get; set; } = new();

        [Field(Title = "Stars (1–5)", Description = "Number of stars (1–5) for the review.", Placeholder = "5")]
        public StringField Stars { get; set; } = new() { Value = "5" };
    }
}
