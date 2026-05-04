using TestBrixCMS.Data.Fields;
using TestBrixCMS.Models.Base;

namespace TestBrixCMS.Models.Blocks;

[BlockType(Name = "CTA Banner", Category = "Content", Icon = "fas fa-bullhorn", Description = "Call-to-action banner with headline, description, and a prominent button to drive conversions.")]
public class CTABannerBlock : BlockBase
{
    [Header("Texto")]
    [Field(Title = "Título principal")]
    public StringField Title { get; set; } = new();

    [Field(Title = "Color del título")]
    public ColorField TitleColor { get; set; } = new() { Value = "#ffffff" };

    [Field(Title = "Tamaño del título", Placeholder = "Ej: 2rem, 2.5rem")]
    public StringField TitleSize { get; set; } = new() { Value = "2rem" };

    [Field(Title = "Subtítulo / descripción")]
    public StringField Subtitle { get; set; } = new();

    [Field(Title = "Color del subtítulo")]
    public ColorField SubtitleColor { get; set; } = new() { Value = "rgba(255,255,255,0.8)" };

    [Header("Botón primario")]
    [Field(Title = "Texto botón 1")]
    public StringField Btn1Text { get; set; } = new() { Value = "Empezar ahora" };

    [Field(Title = "URL botón 1")]
    public StringField Btn1Url { get; set; } = new() { Value = "#" };

    [Field(Title = "Color fondo botón 1")]
    public ColorField Btn1BgColor { get; set; } = new() { Value = "#ffffff" };

    [Field(Title = "Color texto botón 1")]
    public ColorField Btn1TextColor { get; set; } = new() { Value = "#111827" };

    [Header("Botón secundario (opcional)")]
    [Field(Title = "Texto botón 2")]
    public StringField Btn2Text { get; set; } = new();

    [Field(Title = "URL botón 2")]
    public StringField Btn2Url { get; set; } = new() { Value = "#" };

    [Field(Title = "Color borde/texto botón 2")]
    public ColorField Btn2Color { get; set; } = new() { Value = "#ffffff" };

    [Header("Fondo")]
    [Field(Title = "Color de fondo")]
    public ColorField BackgroundColor { get; set; } = new() { Value = "#10b981" };

    [Field(Title = "Color de fondo 2 (gradiente, opcional)")]
    public ColorField BackgroundColor2 { get; set; } = new() { Value = "" };

    [Field(Title = "Imagen de fondo (opcional)")]
    public ImageField BackgroundImage { get; set; } = new();

    [Field(Title = "Padding vertical", Placeholder = "Ej: 5rem")]
    public StringField PaddingY { get; set; } = new() { Value = "5rem" };

    [Field(Title = "Alineación del contenido", Placeholder = "left | center | right")]
    public StringField TextAlign { get; set; } = new() { Value = "center" };
}
