using BrixCMS.Open.Data; // Donde tengas tu entidad de BD 'Block'
using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.Base
{
    public abstract class BlockGroupBase : brixBlockBase
    {
        // Contenedor de bloques hijo.
        // Todos los campos compartidos (FullWidth, padding, margin, animation, font) están en brixBlockBase.
        public List<BrixCMS.Open.Data.Block> Items { get; set; } = new();
    }
}
