using BrixCMS.Open.Data.Fields;
using BrixCMS.Open.Models.Base;

namespace BrixCMS.Open.Models.Blocks
{
    [BlockType(Name = "Code Block", Category = "Content", Icon = "fas fa-code", Description = "Syntax highlighted code block. Perfect for code snippets, API examples, and tutorials.")]
    public class CodeBlock : BlockBase
    {
        [Field(Title = "Code", Placeholder = "Paste your code here")]
        public TextAreaField Code { get; set; } = new();

        [Field(Title = "Language")]
        public SelectField<string> Language { get; set; } = new()
        {
            Value = "javascript",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "javascript", Label = "JavaScript" },
                new() { Value = "typescript", Label = "TypeScript" },
                new() { Value = "python", Label = "Python" },
                new() { Value = "java", Label = "Java" },
                new() { Value = "csharp", Label = "C#" },
                new() { Value = "go", Label = "Go" },
                new() { Value = "rust", Label = "Rust" },
                new() { Value = "php", Label = "PHP" },
                new() { Value = "ruby", Label = "Ruby" },
                new() { Value = "sql", Label = "SQL" },
                new() { Value = "html", Label = "HTML" },
                new() { Value = "css", Label = "CSS" },
                new() { Value = "json", Label = "JSON" },
                new() { Value = "yaml", Label = "YAML" },
                new() { Value = "bash", Label = "Bash/Shell" },
                new() { Value = "plaintext", Label = "Plain Text" }
            }
        };

        [Field(Title = "Syntax Theme")]
        public SelectField<string> Theme { get; set; } = new()
        {
            Value = "dark",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "default", Label = "Default" },
                new() { Value = "dark", Label = "Dark" },
                new() { Value = "github", Label = "GitHub" },
                new() { Value = "monokai", Label = "Monokai" },
                new() { Value = "dracula", Label = "Dracula" }
            }
        };

        [Field(Title = "Background Color")]
        public ColorField BackgroundColor { get; set; } = new() { Value = "#1e293b" };

        [Field(Title = "Text Color")]
        public ColorField TextColor { get; set; } = new() { Value = "#e2e8f0" };

        [Field(Title = "Border Radius", Placeholder = "8px")]
        public StringField BorderRadius { get; set; } = new() { Value = "8px" };

        [Field(Title = "Show Line Numbers")]
        public SelectField<string> ShowLineNumbers { get; set; } = new()
        {
            Value = "true",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "true", Label = "Yes" },
                new() { Value = "false", Label = "No" }
            }
        };

        [Field(Title = "Show Copy Button")]
        public SelectField<string> ShowCopyButton { get; set; } = new()
        {
            Value = "true",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "true", Label = "Yes" },
                new() { Value = "false", Label = "No" }
            }
        };

        [Field(Title = "Max Height", Placeholder = "e.g. 400px, auto")]
        public StringField MaxHeight { get; set; } = new();

        [Field(Title = "Font Size", Placeholder = "14px")]
        public StringField FontSize { get; set; } = new() { Value = "14px" };

        [Field(Title = "Code Title/Filename", Placeholder = "e.g. example.js")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Title Bar Background")]
        public ColorField TitleBackground { get; set; } = new() { Value = "#0f172a" };

        [Field(Title = "Section ID (anchor)")]
        public StringField SectionId { get; set; } = new();
    }
}
