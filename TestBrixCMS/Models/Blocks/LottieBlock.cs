using TestBrixCMS.Data.Fields;
using TestBrixCMS.Models.Base;

namespace TestBrixCMS.Models.Blocks
{
    [BlockType(Name = "Lottie Animation", Category = "Media", Icon = "fas fa-film", Description = "Lottie animation. Lightweight JSON animations for engaging visual content.")]
    public class LottieBlock : BlockBase
    {
        [Field(Title = "Lottie JSON URL", Description = "Direct URL to .json Lottie file")]
        public UrlField LottieUrl { get; set; } = new();

        [Field(Title = "Width", Placeholder = "300px")]
        public StringField Width { get; set; } = new() { Value = "300px" };

        [Field(Title = "Height", Placeholder = "300px")]
        public StringField Height { get; set; } = new() { Value = "300px" };

        [Field(Title = "Auto Play")]
        public SelectField<string> AutoPlay { get; set; } = new()
        {
            Value = "true",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "true", Label = "Yes" },
                new() { Value = "false", Label = "No" }
            }
        };

        [Field(Title = "Loop")]
        public SelectField<string> Loop { get; set; } = new()
        {
            Value = "true",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "true", Label = "Yes" },
                new() { Value = "false", Label = "No" }
            }
        };

        [Field(Title = "Animation Speed", Placeholder = "0.5, 1, 2")]
        public StringField Speed { get; set; } = new() { Value = "1" };

        [Field(Title = "Background Color", Description = "Optional background behind animation")]
        public ColorField BackgroundColor { get; set; } = new() { Value = "transparent" };

        [Field(Title = "Border Radius", Placeholder = "0, 8px, 50%")]
        public StringField BorderRadius { get; set; } = new() { Value = "0" };

        [Field(Title = "Section ID (anchor)")]
        public StringField SectionId { get; set; } = new();

        [Field(Title = "Vertical Padding", Placeholder = "1rem")]
        public StringField PaddingY { get; set; } = new() { Value = "1rem" };
    }
}
