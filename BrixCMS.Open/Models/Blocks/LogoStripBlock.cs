using BrixCMS.Open.Data.Fields;
using BrixCMS.Open.Models.Base;

namespace BrixCMS.Open.Models.Blocks;

[BlockType(Name = "Logo Strip", Category = "Content", Icon = "fas fa-handshake", Description = "Horizontal row of partner or brand logos. Builds trust and social proof on landing pages.")]
public class LogoStripBlock : BlockBase
{
    [Header("Encabezado")]
    [Field(Title = "Texto encabezado", Placeholder = "Ej: Empresas que confían en nosotros")]
    public StringField Heading { get; set; } = new();

    [Field(Title = "Color del encabezado")]
    public ColorField HeadingColor { get; set; } = new() { Value = "#9ca3af" };

    [Header("Logo 1")]
    [Field(Title = "Imagen Logo 1")]
    public ImageField Logo1 { get; set; } = new();
    [Field(Title = "Link Logo 1 (opcional)")]
    public StringField Logo1Url { get; set; } = new();

    [Header("Logo 2")]
    [Field(Title = "Imagen Logo 2")]
    public ImageField Logo2 { get; set; } = new();
    [Field(Title = "Link Logo 2 (opcional)")]
    public StringField Logo2Url { get; set; } = new();

    [Header("Logo 3")]
    [Field(Title = "Imagen Logo 3")]
    public ImageField Logo3 { get; set; } = new();
    [Field(Title = "Link Logo 3 (opcional)")]
    public StringField Logo3Url { get; set; } = new();

    [Header("Logo 4")]
    [Field(Title = "Imagen Logo 4")]
    public ImageField Logo4 { get; set; } = new();
    [Field(Title = "Link Logo 4 (opcional)")]
    public StringField Logo4Url { get; set; } = new();

    [Header("Logo 5")]
    [Field(Title = "Imagen Logo 5")]
    public ImageField Logo5 { get; set; } = new();
    [Field(Title = "Link Logo 5 (opcional)")]
    public StringField Logo5Url { get; set; } = new();

    [Header("Logo 6")]
    [Field(Title = "Imagen Logo 6")]
    public ImageField Logo6 { get; set; } = new();
    [Field(Title = "Link Logo 6 (opcional)")]
    public StringField Logo6Url { get; set; } = new();

    [Header("Diseño")]
    [Field(Title = "Altura de los logos", Placeholder = "Ej: 40px, 50px")]
    public StringField LogoHeight { get; set; } = new() { Value = "40px" };

    [Field(Title = "Filtro B&W (grayscale)", Placeholder = "true | false")]
    public BoolField Grayscale { get; set; } = new() { Value = "true" };

    [Field(Title = "Color de fondo")]
    public ColorField BackgroundColor { get; set; } = new() { Value = "#f9fafb" };

    [Field(Title = "Padding vertical")]
    public StringField PaddingY { get; set; } = new() { Value = "2.5rem" };
}
