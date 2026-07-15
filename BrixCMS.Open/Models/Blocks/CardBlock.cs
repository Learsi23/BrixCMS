using BrixCMS.Open.Models.Base;
using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.Blocks;

[BlockType(Name = "Universal Hero Card", Category = "Content", Icon = "fas fa-layer-group", Description = "Flexible card with image, title, description, icon, and CTA button. Supports vertical, horizontal, and overlay layouts.")]
public class CardBlock : BlockBase
{
    // --- CONTENIDO ---
    [Header("Contenido")]
    [Field(Title = "T�tulo Principal", Placeholder = "Ej: Nuestros Servicios")]
    public StringField Title { get; set; } = new();

    [Field(Title = "Color del T�tulo")]
    public ColorField TitleColor { get; set; } = new() { Value = "#1f2937" };

    [Field(Title = "Tama�o del T�tulo", Placeholder = "Ej: 1.5rem o 24px")]
    public StringField TitleSize { get; set; } = new() { Value = "1.5rem" };

    [Field(Title = "Subt�tulo / Badge")]
    public StringField Badge { get; set; } = new();

    [Field(Title = "Color de Badge")]
    public ColorField BadgeColor { get; set; } = new() { Value = "#4b5563" };

    [Field(Title = "Tama�o de Badge")]
    public StringField BadgeSize { get; set; } = new() { Value = "0.875rem" };

    [Field(Title = "Subtitle", Description = "Optional second line under the title")]
    public StringField Subtitle { get; set; } = new();

    [Field(Title = "Subtitle Color")]
    public ColorField SubtitleColor { get; set; } = new() { Value = "#6b7280" };

    [Field(Title = "Text Alignment", Placeholder = "left | center | right")]
    public StringField TextAlign { get; set; } = new() { Value = "center" };

    [Field(Title = "Descripci�n Detallada")]
    public TextAreaField Description { get; set; } = new() { Rows = 4 };

    [Field(Title = "Color de Descripci�n")]
    public ColorField DescriptionColor { get; set; } = new() { Value = "#4b5563" };

    [Field(Title = "Tama�o de Descripci�n")]
    public StringField DescriptionSize { get; set; } = new() { Value = "0.875rem" };

    // --- MULTIMEDIA ---
    [Header("Imagen e Icono")]
    [Field(Title = "Imagen Principal")]
    public ImageField Image { get; set; } = new();

    [Field(Title = "Altura de la Imagen", Placeholder = "Ej: 250px o 300px")]
    public StringField ImageHeight { get; set; } = new() { Value = "250px" };

    [Field(Title = "Icono (FontAwesome)", Placeholder = "fas fa-rocket")]
    public StringField IconClass { get; set; } = new();

    // --- ENLACES Y BOT�N ---
    [Header("Acci�n")]
    [Field(Title = "URL de Destino")]
    public StringField TargetUrl { get; set; } = new();

    [Field(Title = "Texto del Bot�n")]
    public StringField ButtonText { get; set; } = new() { Value = "Saber m�s" };

    [Field(Title = "Color del Bot�n (Fondo)")]
    public ColorField AccentColor { get; set; } = new() { Value = "#3b82f6" };

    [Field(Title = "Color del Texto del Bot�n")]
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


    // --- CONFIGURACI�N T�CNICA ---
    [Header("Ajustes de Dise�o")]
    [Field(Title = "Layout", Placeholder = "vertical, horizontal, overlay")]
    public StringField LayoutType { get; set; } = new() { Value = "vertical" };

    [Field(Title = "�Cristal (Glass)?", Placeholder = "Yes / No")]
    public StringField UseGlassmorphism { get; set; } = new() { Value = "No" };

    [Field(Title = "Color de Fondo de la Card")]
    public ColorField CardBgColor { get; set; } = new() { Value = "#ffffff" };
}
