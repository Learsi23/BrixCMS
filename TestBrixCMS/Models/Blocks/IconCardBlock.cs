using TestBrixCMS.Data.Fields;
using TestBrixCMS.Models.Base;

namespace TestBrixCMS.Models.Blocks
{
    [BlockType(Name = "Icon Card", Category = "Content", Icon = "fas fa-id-card", Description = "Card with a large icon, title, and description. Great for features or services sections.")]
    public class IconCardBlock : BlockBase
    {
        // ── Section: Container ────────────────────────────────────────
        [Header("Container")]

        [Field(Title = "Background Color")]
        public ColorField BackgroundColor { get; set; } = new();

        [Field(Title = "Border Color")]
        public ColorField BorderColor { get; set; } = new();

        [Field(Title = "Border Radius", Placeholder = "E.g.: 12px or 1rem")]
        public StringField BorderRadius { get; set; } = new();

        [Field(Title = "Border Width", Placeholder = "E.g.: 1px or 2px")]
        public StringField BorderWidth { get; set; } = new();

        [Field(Title = "Inner Padding", Placeholder = "E.g.: 1.5rem or 24px")]
        public StringField Padding { get; set; } = new();

        [Field(Title = "Shadow", Placeholder = "E.g.: 0 4px 20px rgba(0,0,0,0.1)")]
        public StringField Shadow { get; set; } = new();

        // ── Section: Layout ────────────────────────────────────────────
        [Header("Layout")]

        [Field(Title = "Icon Position", Placeholder = "top | left | right")]
        public StringField IconPosition { get; set; } = new() { Value = "left" };

        [Field(Title = "Text Alignment", Placeholder = "left | center | right")]
        public StringField TextAlign { get; set; } = new() { Value = "left" };

        // ── Section: Main Icon (Image) ────────────────────────────────
        [Header("Main Icon (Image)")]

        [Field(Title = "Icon Image")]
        public ImageField LeftIcon { get; set; } = new();

        [Field(Title = "Icon Image Size", Placeholder = "E.g.: 48px")]
        public StringField LeftIconSize { get; set; } = new();

        [Header("Main Icon (FontAwesome)")]

        [Field(Title = "FontAwesome Class", Placeholder = "E.g.: fas fa-rocket")]
        public StringField LeftIconClass { get; set; } = new();

        [Field(Title = "FA Icon Color")]
        public ColorField LeftIconColor { get; set; } = new();

        [Field(Title = "FA Icon Size", Placeholder = "E.g.: 2rem")]
        public StringField LeftIconFaSize { get; set; } = new() { Value = "2rem" };

        // ── Section: Right Icon (Decorative) ────────────────────────────
        [Header("Right Icon (Decorative)")]

        [Field(Title = "Right Icon Image")]
        public ImageField RightIcon { get; set; } = new();

        [Field(Title = "Right Icon Size", Placeholder = "E.g.: 32px")]
        public StringField RightIconSize { get; set; } = new();

        [Field(Title = "Right FontAwesome Class", Placeholder = "E.g.: fas fa-arrow-right")]
        public StringField RightIconClass { get; set; } = new();

        [Field(Title = "Right FA Icon Color")]
        public ColorField RightIconColor { get; set; } = new();

        // ── Section: Text Content ──────────────────────────────────────
        [Header("Text Content")]

        [Field(Title = "Title")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Title Color")]
        public ColorField TitleColor { get; set; } = new();

        [Field(Title = "Title Size", Placeholder = "E.g.: 1.25rem")]
        public StringField TitleSize { get; set; } = new();

        [Field(Title = "Subtitle")]
        public StringField Subtitle { get; set; } = new();

        [Field(Title = "Subtitle Color")]
        public ColorField SubtitleColor { get; set; } = new();

        [Field(Title = "Subtitle Size", Placeholder = "E.g.: 1rem")]
        public StringField SubtitleSize { get; set; } = new();

        [Field(Title = "Body Text")]
        public TextAreaField Text { get; set; } = new();

        [Field(Title = "Text Color")]
        public ColorField TextColor { get; set; } = new();

        [Field(Title = "Text Size", Placeholder = "E.g.: 0.95rem")]
        public StringField TextSize { get; set; } = new();

        [Field(Title = "Markdown / HTML")]
        public MarkdownField MarkDown { get; set; } = new();

        [Field(Title = "Markdown Color")]
        public ColorField MarkDownColor { get; set; } = new();

        // ── Section: Link (CTA) ────────────────────────────────────────
        [Header("Link (CTA)")]

        [Field(Title = "Link URL", Placeholder = "E.g.: /services or https://...")]
        public UrlField LinkUrl { get; set; } = new();

        [Field(Title = "Button Text", Placeholder = "E.g.: Learn more")]
        public StringField LinkText { get; set; } = new();

        [Field(Title = "Button Background Color")]
        public ColorField LinkBgColor { get; set; } = new();

        [Field(Title = "Button Text Color")]
        public ColorField LinkTextColor { get; set; } = new();

        [Field(Title = "Open in New Tab")]
        public BoolField LinkNewTab { get; set; } = new() { Value = "false" };
    }
}
