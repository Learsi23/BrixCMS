using TestBrixCMS.Models.Base;
using TestBrixCMS.Data.Fields;

namespace TestBrixCMS.Models.Blocks.Gallery
{
    [BlockType(Name = "Gallery", Category = "Media", Icon = "fas fa-images", Description = "Image gallery with grid, masonry, or carousel display modes. Supports lightbox on click.")]
    public class GalleryBlock : BlockGroupBase  
    {
        [Field(Title = "Gallery Title", Placeholder = "Enter gallery title (optional)")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Title Color")]
        public ColorField TitleColor { get; set; } = new();

        [Field(Title = "Title Size", Placeholder = "e.g., 24px, 2rem, text-3xl")]
        public StringField TitleSize { get; set; } = new();

        [Field(Title = "Layout Type", Description = "How to display gallery items")]
        public SelectField<string> LayoutType { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "carousel", Label = "Carousel", Icon = "fas fa-images" },
                new() { Value = "grid", Label = "Grid", Icon = "fas fa-th" },
                new() { Value = "slider", Label = "Slider", Icon = "fas fa-arrow-left" },
                new() { Value = "masonry", Label = "Masonry", Icon = "fas fa-th-large" }
            },
            Value = "carousel"
        };

        [Field(Title = "Auto Play")]
        public BoolField AutoPlay { get; set; } = new() { Value = "true" };

        [Field(Title = "Auto Play Interval (ms)", Placeholder = "3000")]
        public NumberField AutoPlayInterval { get; set; } = new() { Value = "3000" };

        [Field(Title = "Show Arrows")]
        public BoolField ShowArrows { get; set; } = new() { Value = "true" };

        [Field(Title = "Show Dots")]
        public BoolField ShowDots { get; set; } = new() { Value = "true" };

        [Field(Title = "Infinite Loop")]
        public BoolField InfiniteLoop { get; set; } = new() { Value = "true" };

        [Field(Title = "Lightbox al hacer clic")]
        public BoolField EnableLightbox { get; set; } = new() { Value = "true" };

        [Field(Title = "Items per View", Placeholder = "3")]
        public NumberField ItemsPerView { get; set; } = new() { Value = "3" };

        [Field(Title = "Gap between items", Placeholder = "16px")]
        public StringField Gap { get; set; } = new() { Value = "16px" };

        [Field(Title = "Item Height", Placeholder = "300px")]
        public StringField ItemHeight { get; set; } = new() { Value = "300px" };

        [Field(Title = "Background Color")]
        public ColorField BackgroundColor { get; set; } = new();

        [Field(Title = "Padding", Placeholder = "20px")]
        public StringField Padding { get; set; } = new() { Value = "20px" };

        [Field(Title = "Border Radius", Placeholder = "8px")]
        public StringField BorderRadius { get; set; } = new() { Value = "0px" };
    }
}
