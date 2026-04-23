using BrixCMS.Open.Data.Fields;
using BrixCMS.Open.Models.Base;

namespace BrixCMS.Open.Models.Blocks;

[BlockType(Name = "Spacer", Category = "Layout", Icon = "fas fa-arrows-alt-v", Description = "Adds vertical whitespace between blocks. Set exact height in px, rem, or vh.")]
public class SpacerBlock : BlockBase
{
    [Field(Title = "Altura del espacio", Placeholder = "Ej: 40px, 3rem, 80px")]
    public StringField Height { get; set; } = new() { Value = "48px" };

    [Field(Title = "Color de fondo (opcional)")]
    public ColorField BackgroundColor { get; set; } = new() { Value = "transparent" };
}
