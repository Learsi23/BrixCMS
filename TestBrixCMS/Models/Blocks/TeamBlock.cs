using TestBrixCMS.Models.Base;
using TestBrixCMS.Data.Fields;

namespace TestBrixCMS.Models.Blocks
{
    [BlockType(Name = "Team", Category = "Content", Icon = "fas fa-users",
        Description = "Grid of team members with photo, name, position, and social media links.")]
    public class TeamBlock : BlockGroupBase
    {
        [Header("Header")]
        [Field(Title = "Title", Description = "Main title for the team section.")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Title Color", Description = "Color of the title text.")]
        public ColorField TitleColor { get; set; } = new();

        [Field(Title = "Subtitle", Description = "Subtitle or description for the team section.")]
        public StringField Subtitle { get; set; } = new();

        [Header("Layout")]
        [Field(Title = "Columns (Desktop)", Description = "Number of columns for desktop view (e.g., 2, 3, 4).", Placeholder = "2, 3, 4")]
        public StringField Columns { get; set; } = new() { Value = "3" };

        [Field(Title = "Background Color", Description = "Background color of the team block.")]
        public ColorField BackgroundColor { get; set; } = new();
    }
}
