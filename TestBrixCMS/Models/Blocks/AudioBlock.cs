using TestBrixCMS.Data.Fields;
using TestBrixCMS.Models.Base;

namespace TestBrixCMS.Models.Blocks
{
    [BlockType(Name = "Audio / Podcast", Category = "Media", Icon = "fas fa-headphones", Description = "Audio player for podcasts, music, or voice recordings. Supports custom cover image and episode info.")]
    public class AudioBlock : BlockBase
    {
        [Field(Title = "Audio URL", Description = "Direct URL to audio file (mp3, wav, ogg)")]
        public UrlField AudioUrl { get; set; } = new();

        [Field(Title = "Cover Image", Description = "Album art or episode thumbnail")]
        public ImageField CoverImage { get; set; } = new();

        [Field(Title = "Episode/Track Title")]
        public StringField Title { get; set; }

        [Field(Title = "Description", Placeholder = "Episode description or track info")]
        public TextAreaField Description { get; set; }

        [Field(Title = "Artist/Podcast Name")]
        public StringField Artist { get; set; }

        [Field(Title = "Show Controls")]
        public SelectField<string> ShowControls { get; set; } = new()
        {
            Value = "true",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "true", Label = "Yes" },
                new() { Value = "false", Label = "No" }
            }
        };

        [Field(Title = "Auto Play")]
        public SelectField<string> AutoPlay { get; set; } = new()
        {
            Value = "false",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "false", Label = "No" },
                new() { Value = "true", Label = "Yes" }
            }
        };

        [Field(Title = "Loop")]
        public SelectField<string> Loop { get; set; } = new()
        {
            Value = "false",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "false", Label = "No" },
                new() { Value = "true", Label = "Yes" }
            }
        };

        [Field(Title = "Player Style")]
        public SelectField<string> Style { get; set; } = new()
        {
            Value = "card",
            Options = new List<SelectOption<string>>
            {
                new() { Value = "minimal", Label = "Minimal" },
                new() { Value = "card", Label = "Card with Cover" },
                new() { Value = "full", Label = "Full Width" }
            }
        };

        [Field(Title = "Background Color")]
        public ColorField BackgroundColor { get; set; } = new() { Value = "#f3f4f6" };

        [Field(Title = "Text Color")]
        public ColorField TextColor { get; set; } = new() { Value = "#000000" };

        [Field(Title = "Accent Color")]
        public ColorField AccentColor { get; set; } = new() { Value = "#5B6EF5" };

        [Field(Title = "Section ID (anchor)")]
        public StringField SectionId { get; set; } = new();
    }
}
