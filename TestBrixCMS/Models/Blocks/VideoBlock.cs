using TestBrixCMS.Data.Fields;
using TestBrixCMS.Models.Base;

namespace TestBrixCMS.Models.Blocks;

[BlockType(Name = "Video", Category = "Media", Icon = "fas fa-play-circle", Description = "Embed a YouTube, Vimeo, or direct video URL with autoplay and loop options.")]
public class VideoBlock : BlockBase
{
    [Header("Video")]
    [Field(Title = "URL de YouTube o Vimeo", Placeholder = "https://www.youtube.com/watch?v=... o https://vimeo.com/...")]
    public StringField VideoUrl { get; set; } = new();

    [Field(Title = "Relación de aspecto", Placeholder = "16/9 | 4/3 | 1/1")]
    public StringField AspectRatio { get; set; } = new() { Value = "16/9" };

    [Field(Title = "Ancho máximo del contenedor", Placeholder = "Ej: 900px, 100%")]
    public StringField MaxWidth { get; set; } = new() { Value = "900px" };

    [Header("Texto opcional")]
    [Field(Title = "Título sobre el video")]
    public StringField Title { get; set; } = new();

    [Field(Title = "Color del título")]
    public ColorField TitleColor { get; set; } = new() { Value = "#111827" };

    [Field(Title = "Subtítulo / descripción")]
    public StringField Subtitle { get; set; } = new();

    [Field(Title = "Color del subtítulo")]
    public ColorField SubtitleColor { get; set; } = new() { Value = "#6b7280" };

    [Field(Title = "Alineación de texto", Placeholder = "left | center | right")]
    public StringField TextAlign { get; set; } = new() { Value = "center" };

    [Header("Fondo")]
    [Field(Title = "Color de fondo de la sección")]
    public ColorField BackgroundColor { get; set; } = new() { Value = "transparent" };

    [Field(Title = "Padding vertical", Placeholder = "Ej: 3rem")]
    public StringField PaddingY { get; set; } = new() { Value = "2rem" };
}
