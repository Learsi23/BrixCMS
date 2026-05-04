using TestBrixCMS.Data.Fields;
using TestBrixCMS.Models.Base;

namespace TestBrixCMS.Models.Blocks
{
    [BlockType(Name = "Feature List", Category = "Content", Icon = "fas fa-list-check", Description = "List of features with icons. Perfect for showing features, benefits, or checklists.")]
    public class FeatureListBlock : BlockBase
    {
        [Field(Title = "Section ID (anchor)", Placeholder = "e.g. features")]
        public StringField SectionId { get; set; }

        [Field(Title = "Section Title")]
        public StringField Title { get; set; }

        [Field(Title = "Title Color")]
        public ColorField TitleColor { get; set; } = new() { Value = "#000000" };

        [Field(Title = "Title Alignment")]
        public SelectField<string> TitleAlign { get; set; } = new()
        {
            Value = "left",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "left", Label = "Left" },
                new() { Value = "center", Label = "Center" },
                new() { Value = "right", Label = "Right" }
            }
        };

        [Field(Title = "Columns")]
        public SelectField<string> Columns { get; set; } = new()
        {
            Value = "2",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "1", Label = "1 Column" },
                new() { Value = "2", Label = "2 Columns" },
                new() { Value = "3", Label = "3 Columns" },
                new() { Value = "4", Label = "4 Columns" }
            }
        };

        [Field(Title = "Gap Between Items", Placeholder = "1rem")]
        public StringField Gap { get; set; } = new() { Value = "1rem" };

        [Field(Title = "Icon Color")]
        public ColorField ItemIconColor { get; set; } = new() { Value = "#10b981" };

        [Field(Title = "Icon Size", Placeholder = "24px")]
        public StringField ItemIconSize { get; set; } = new() { Value = "24px" };

        [Field(Title = "Item Title Color")]
        public ColorField ItemTitleColor { get; set; } = new() { Value = "#000000" };

        [Field(Title = "Item Title Weight", Placeholder = "normal, bold, 600")]
        public StringField ItemTitleWeight { get; set; } = new() { Value = "600" };

        [Field(Title = "Item Description Color")]
        public ColorField ItemDescriptionColor { get; set; } = new() { Value = "#666666" };

        [Field(Title = "Item Description Size", Placeholder = "14px")]
        public StringField ItemDescriptionSize { get; set; } = new() { Value = "14px" };

        [Field(Title = "Background Color")]
        public ColorField BackgroundColor { get; set; } = new() { Value = "transparent" };

        [Field(Title = "Vertical Padding", Placeholder = "2rem")]
        public StringField PaddingY { get; set; } = new() { Value = "2rem" };
    }
}
