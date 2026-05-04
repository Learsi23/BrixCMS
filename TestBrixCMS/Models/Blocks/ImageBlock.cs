using TestBrixCMS.Models.Base;
using TestBrixCMS.Data.Fields;

namespace TestBrixCMS.Models.Blocks;

[BlockType(Name = "Image", Category = "Media", Icon = "fas fa-image", Description = "Single image with configurable size, alt text, border radius, and optional caption.")]
public class ImageBlock : BlockBase
{
    [Field(Title = "Seleccionar Imagen")]
    public ImageField Source { get; set; } = new();

    [Field(Title = "Texto Alternativo (Alt)")]
    public StringField AltText { get; set; } = new();
}
