using TestBrixCMS.Models.Base;
using TestBrixCMS.Data.Fields;

namespace TestBrixCMS.Models.Blocks;

[BlockType(Name = "Universal Hero Card", Category = "Content", Icon = "fas fa-layer-group", Description = "Flexible card with image, title, description, icon, and CTA button. Supports vertical, horizontal, and overlay layouts.")]
public class CardBlock : BlockBase
{
    // --- CONTENIDO ---
    [Header("Contenido")]
    [Field(Title = "Título Principal", Placeholder = "Ej: Nuestros Servicios")]
    public StringField Title { get; set; } = new();

    [Field(Title = "Color del Título")]
    public ColorField TitleColor { get; set; } = new() { Value = "#1f2937" };

    [Field(Title = "Tamaño del Título", Placeholder = "Ej: 1.5rem o 24px")]
    public StringField TitleSize { get; set; } = new() { Value = "1.5rem" };

    [Field(Title = "Subtítulo / Badge")]
    public StringField Badge { get; set; } = new();

    [Field(Title = "Color de Badge")]
    public ColorField BadgeColor { get; set; } = new() { Value = "#4b5563" };

    [Field(Title = "Tamaño de Badge")]
    public StringField BadgeSize { get; set; } = new() { Value = "0.875rem" };

    [Field(Title = "Descripción Detallada")]
    public TextAreaField Description { get; set; } = new() { Rows = 4 };

    [Field(Title = "Color de Descripción")]
    public ColorField DescriptionColor { get; set; } = new() { Value = "#4b5563" };

    [Field(Title = "Tamaño de Descripción")]
    public StringField DescriptionSize { get; set; } = new() { Value = "0.875rem" };

    // --- MULTIMEDIA ---
    [Header("Imagen e Icono")]
    [Field(Title = "Imagen Principal")]
    public ImageField Image { get; set; } = new();

    [Field(Title = "Altura de la Imagen", Placeholder = "Ej: 250px o 300px")]
    public StringField ImageHeight { get; set; } = new() { Value = "250px" };

    [Field(Title = "Icono (FontAwesome)", Placeholder = "fas fa-rocket")]
    public StringField IconClass { get; set; } = new();

    // --- ENLACES Y BOTÓN ---
    [Header("Acción")]
    [Field(Title = "URL de Destino")]
    public StringField TargetUrl { get; set; } = new();

    [Field(Title = "Texto del Botón")]
    public StringField ButtonText { get; set; } = new() { Value = "Saber más" };

    [Field(Title = "Color del Botón (Fondo)")]
    public ColorField AccentColor { get; set; } = new() { Value = "#3b82f6" };

    [Field(Title = "Color del Texto del Botón")]
    public ColorField ButtonTextColor { get; set; } = new() { Value = "#ffffff" };

    [Field(Title = "Button Hover Color")]
    public ColorField HoverColor { get; set; }

    [Field(Title = "Border Radius", Placeholder = "fas fa-font")]
    public StringField BorderRadius { get; set; }

    [Field(Title = "Button Border", Placeholder = "e.g., '2px solid #000'")]
    public StringField Border { get; set; }


    [Field(Title = "Padding", Placeholder = "fas fa-font")]
    public StringField Padding { get; set; }

    [Field(Title = "Text Color", Placeholder = "Color for the button text")]
    public ColorField TextColor { get; set; }

    [Field(Title = "Button Position", Placeholder = "Position of the button (e.g., 'left', 'center', 'right')")]
    public StringField ButtonPosition { get; set; }


    // --- CONFIGURACIÓN TÉCNICA ---
    [Header("Ajustes de Diseño")]
    [Field(Title = "Layout", Placeholder = "vertical, horizontal, overlay")]
    public StringField LayoutType { get; set; } = new() { Value = "vertical" };

    [Field(Title = "¿Cristal (Glass)?", Placeholder = "Yes / No")]
    public StringField UseGlassmorphism { get; set; } = new() { Value = "No" };

    [Field(Title = "Color de Fondo de la Card")]
    public ColorField CardBgColor { get; set; } = new() { Value = "#ffffff" };
}
