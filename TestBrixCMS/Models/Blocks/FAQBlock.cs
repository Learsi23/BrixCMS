using TestBrixCMS.Data.Fields;
using TestBrixCMS.Models.Base;

namespace TestBrixCMS.Models.Blocks
{
    [BlockType(Name = "FAQ (Specialized)", Category = "Interactive", Icon = "fas fa-question-circle", Description = "Specialized FAQ block with structured questions and answers. Different from accordion - optimized for Q&A content.")]
    public class FAQBlock : BlockBase
    {
        [Field(Title = "Section ID (anchor)", Placeholder = "e.g. faq")]
        public StringField SectionId { get; set; } = new();

        [Field(Title = "FAQ Style")]
        public SelectField<string> Style { get; set; } = new()
        {
            Value = "cards",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "list", Label = "Simple List" },
                new() { Value = "cards", Label = "Cards" },
                new() { Value = "bordered", Label = "Bordered" }
            }
        };

        [Field(Title = "Background Color")]
        public ColorField BackgroundColor { get; set; } = new() { Value = "transparent" };

        [Field(Title = "Vertical Padding", Placeholder = "3rem")]
        public StringField PaddingY { get; set; } = new() { Value = "3rem" };

        [Field(Title = "Section Title")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Title Color")]
        public ColorField TitleColor { get; set; } = new() { Value = "#000000" };

        [Field(Title = "Title Alignment")]
        public SelectField<string> TitleAlign { get; set; } = new()
        {
            Value = "center",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "left", Label = "Left" },
                new() { Value = "center", Label = "Center" },
                new() { Value = "right", Label = "Right" }
            }
        };

        [Field(Title = "Description", Placeholder = "Optional description below title")]
        public TextAreaField Description { get; set; } = new();

        [Field(Title = "Description Color")]
        public ColorField DescriptionColor { get; set; } = new() { Value = "#666666" };

        [Field(Title = "Question Color")]
        public ColorField QuestionColor { get; set; } = new() { Value = "#000000" };

        [Field(Title = "Question Weight", Placeholder = "normal, bold, 600")]
        public StringField QuestionWeight { get; set; } = new() { Value = "600" };

        [Field(Title = "Answer Color")]
        public ColorField AnswerColor { get; set; } = new() { Value = "#555555" };

        [Field(Title = "Answer Weight", Placeholder = "normal, light, 400")]
        public StringField AnswerWeight { get; set; } = new() { Value = "400" };

        [Field(Title = "Icon Color")]
        public ColorField IconColor { get; set; } = new() { Value = "#5B6EF5" };

        [Field(Title = "Border Color")]
        public ColorField BorderColor { get; set; } = new() { Value = "#e5e7eb" };

        [Field(Title = "Columns Layout")]
        public SelectField<string> Columns { get; set; } = new()
        {
            Value = "1",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "1", Label = "1 Column" },
                new() { Value = "2", Label = "2 Columns" }
            }
        };
    }
}
