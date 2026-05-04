using TestBrixCMS.Data.Fields;
using TestBrixCMS.Models.Base;

namespace TestBrixCMS.Models.Blocks;

[BlockType(Name = "Divider", Category = "Layout", Icon = "fas fa-grip-lines", Description = "Horizontal separator line with configurable color, thickness, and style.")]
public class DividerBlock : BlockBase
{
    [Field(Title = "Estilo", Placeholder = "solid | dashed | dotted | double | none")]
    public StringField Style { get; set; } = new() { Value = "solid" };

    [Field(Title = "Color")]
    public ColorField Color { get; set; } = new() { Value = "#e5e7eb" };

    [Field(Title = "Grosor", Placeholder = "Ej: 1px, 2px")]
    public StringField Thickness { get; set; } = new() { Value = "1px" };

    [Field(Title = "Ancho", Placeholder = "Ej: 100%, 80%, 600px")]
    public StringField Width { get; set; } = new() { Value = "100%" };

    [Field(Title = "Espaciado vertical (padding Y)", Placeholder = "Ej: 2rem, 32px")]
    public StringField PaddingY { get; set; } = new() { Value = "2rem" };
}
