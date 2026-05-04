using TestBrixCMS.Data.Fields;
using TestBrixCMS.Models.Base;

namespace TestBrixCMS.Models.Blocks;

[BlockType(Name = "Map", Category = "Media", Icon = "fas fa-map-marker-alt", Description = "Embed a Google Maps location by address or coordinates. Configurable height and zoom level.")]
public class MapBlock : BlockBase
{
    [Header("Ubicación")]
    [Field(Title = "Dirección", Placeholder = "Ej: Calle Gran Vía 1, Madrid, España")]
    public StringField Address { get; set; } = new();

    [Field(Title = "Zoom (1-20)", Placeholder = "15 = ciudad, 17 = calle, 19 = edificio")]
    public StringField Zoom { get; set; } = new() { Value = "15" };

    [Field(Title = "Tipo de mapa", Placeholder = "roadmap | satellite | hybrid | terrain")]
    public StringField MapType { get; set; } = new() { Value = "roadmap" };

    [Header("Texto opcional")]
    [Field(Title = "Título sobre el mapa")]
    public StringField Title { get; set; } = new();

    [Field(Title = "Color del título")]
    public ColorField TitleColor { get; set; } = new() { Value = "#111827" };

    [Field(Title = "Subtítulo / descripción")]
    public StringField Subtitle { get; set; } = new();

    [Field(Title = "Color del subtítulo")]
    public ColorField SubtitleColor { get; set; } = new() { Value = "#6b7280" };

    [Field(Title = "Alineación del texto", Placeholder = "left | center | right")]
    public StringField TextAlign { get; set; } = new() { Value = "center" };

    [Header("Info card (sobre el mapa)")]
    [Field(Title = "Mostrar card de info", Placeholder = "true | false")]
    public BoolField ShowInfoCard { get; set; } = new() { Value = "true" };

    [Field(Title = "Nombre del lugar / empresa")]
    public StringField PlaceName { get; set; } = new();

    [Field(Title = "Dirección visible en la card")]
    public StringField AddressDisplay { get; set; } = new();

    [Field(Title = "Teléfono")]
    public StringField Phone { get; set; } = new();

    [Field(Title = "Email")]
    public StringField Email { get; set; } = new();

    [Field(Title = "Horario")]
    public StringField Hours { get; set; } = new();

    [Field(Title = "Color fondo card")]
    public ColorField CardBgColor { get; set; } = new() { Value = "#ffffff" };

    [Field(Title = "Color texto card")]
    public ColorField CardTextColor { get; set; } = new() { Value = "#374151" };

    [Header("Dimensiones y estilo")]
    [Field(Title = "Altura del mapa", Placeholder = "Ej: 450px, 500px")]
    public StringField MapHeight { get; set; } = new() { Value = "450px" };

    [Field(Title = "Ancho máximo", Placeholder = "Ej: 100%, 1200px")]
    public StringField MaxWidth { get; set; } = new() { Value = "100%" };

    [Field(Title = "Radio de borde del mapa", Placeholder = "Ej: 16px, 0px")]
    public StringField BorderRadius { get; set; } = new() { Value = "16px" };

    [Field(Title = "Color de fondo de la sección")]
    public ColorField BackgroundColor { get; set; } = new() { Value = "#ffffff" };

    [Field(Title = "Padding vertical", Placeholder = "Ej: 3rem")]
    public StringField PaddingY { get; set; } = new() { Value = "3rem" };
}
