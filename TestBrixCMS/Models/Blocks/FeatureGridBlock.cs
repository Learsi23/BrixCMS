using TestBrixCMS.Data.Fields;
using TestBrixCMS.Models.Base;

namespace TestBrixCMS.Models.Blocks
{
    [BlockType(Name = "Feature Grid", Category = "Content", Icon = "fas fa-th-large",
        Description = "Display a grid of feature cards with icons. Perfect for services, benefits, or platform features sections.")]
    public class FeatureGridBlock : BlockBase
    {
        [Header("Header")]
        [Field(Title = "Section Title")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Title Color")]
        public ColorField TitleColor { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Title Size", Placeholder = "E.g.: 2rem or 32px")]
        public StringField TitleSize { get; set; } = new() { Value = "2rem" };

        [Field(Title = "Subtitle / Description")]
        public StringField Subtitle { get; set; } = new();

        [Field(Title = "Subtitle Color")]
        public ColorField SubtitleColor { get; set; } = new() { Value = "#9ca3af" };

        [Field(Title = "Subtitle Size", Placeholder = "E.g.: 1rem")]
        public StringField SubtitleSize { get; set; } = new() { Value = "1rem" };

        [Header("Layout")]
        [Field(Title = "Columns", Placeholder = "E.g.: 3")]
        public StringField Columns { get; set; } = new() { Value = "3" };

        [Field(Title = "Gap", Placeholder = "E.g.: 1rem or 16px")]
        public StringField Gap { get; set; } = new() { Value = "1rem" };

        [Header("Container Style")]
        [Field(Title = "Background Color")]
        public ColorField BackgroundColor { get; set; } = new();

        [Field(Title = "Container Padding", Placeholder = "E.g.: 3rem")]
        public StringField PaddingY { get; set; } = new() { Value = "3rem" };

        [Field(Title = "Container Max Width", Placeholder = "E.g.: 1200px")]
        public StringField MaxWidth { get; set; } = new() { Value = "1200px" };

        [Header("Feature 1")]
        [Field(Title = "Icon (FA class)", Placeholder = "E.g.: fas fa-key")]
        public StringField Icon1 { get; set; } = new();

        [Field(Title = "Icon Color")]
        public ColorField Icon1Color { get; set; } = new() { Value = "#5B6EF5" };

        [Field(Title = "Title")]
        public StringField Title1 { get; set; } = new();

        [Field(Title = "Description")]
        public StringField Description1 { get; set; } = new();

        [Header("Feature 2")]
        [Field(Title = "Icon (FA class)", Placeholder = "E.g.: fas fa-credit-card")]
        public StringField Icon2 { get; set; } = new();

        [Field(Title = "Icon Color")]
        public ColorField Icon2Color { get; set; } = new() { Value = "#5B6EF5" };

        [Field(Title = "Title")]
        public StringField Title2 { get; set; } = new();

        [Field(Title = "Description")]
        public StringField Description2 { get; set; } = new();

        [Header("Feature 3")]
        [Field(Title = "Icon (FA class)", Placeholder = "E.g.: fas fa-building")]
        public StringField Icon3 { get; set; } = new();

        [Field(Title = "Icon Color")]
        public ColorField Icon3Color { get; set; } = new() { Value = "#5B6EF5" };

        [Field(Title = "Title")]
        public StringField Title3 { get; set; } = new();

        [Field(Title = "Description")]
        public StringField Description3 { get; set; } = new();

        [Header("Feature 4")]
        [Field(Title = "Icon (FA class)", Placeholder = "E.g.: fas fa-robot")]
        public StringField Icon4 { get; set; } = new();

        [Field(Title = "Icon Color")]
        public ColorField Icon4Color { get; set; } = new() { Value = "#5B6EF5" };

        [Field(Title = "Title")]
        public StringField Title4 { get; set; } = new();

        [Field(Title = "Description")]
        public StringField Description4 { get; set; } = new();

        [Header("Feature 5")]
        [Field(Title = "Icon (FA class)", Placeholder = "E.g.: fas fa-paint-brush")]
        public StringField Icon5 { get; set; } = new();

        [Field(Title = "Icon Color")]
        public ColorField Icon5Color { get; set; } = new() { Value = "#5B6EF5" };

        [Field(Title = "Title")]
        public StringField Title5 { get; set; } = new();

        [Field(Title = "Description")]
        public StringField Description5 { get; set; } = new();

        [Header("Feature 6")]
        [Field(Title = "Icon (FA class)", Placeholder = "E.g.: fas fa-pen-ruler")]
        public StringField Icon6 { get; set; } = new();

        [Field(Title = "Icon Color")]
        public ColorField Icon6Color { get; set; } = new() { Value = "#5B6EF5" };

        [Field(Title = "Title")]
        public StringField Title6 { get; set; } = new();

        [Field(Title = "Description")]
        public StringField Description6 { get; set; } = new();
    }
}
