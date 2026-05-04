using TestBrixCMS.Data; // Donde tengas tu entidad de BD 'Block'
using TestBrixCMS.Data.Fields;

namespace TestBrixCMS.Models.Base
{
    public abstract class BlockGroupBase : brixBlockBase
    {
        // Contenedor de bloques hijo.
        // Todos los campos compartidos (FullWidth, padding, margin, animation, font) están en brixBlockBase.
        public List<TestBrixCMS.Data.Block> Items { get; set; } = new();
    }
}
