using BrixCMS.Open.Data.Fields;
using BrixCMS.Open.Models.Base;

namespace BrixCMS.Open.Models.Blocks;

[BlockType(Name = "Markdown", Category = "Content", Icon = "fab fa-markdown", Description = "Write content in Markdown with full WYSIWYG editor (TinyMCE). Ideal for articles, docs, and rich text.")]
public class MarkdownBlock : BlockBase
{
    [Field(Title = "Markdown")]
  
    public MarkdownField Content { get; set; } = new();
}
