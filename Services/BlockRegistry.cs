using System;
using System.Collections.Generic;
using System.Text.Json;

namespace BrixCMS.Open.Services
{
    public class BlockRegistry
    {
        // Diccionario interno para mapear el nombre del bloque con su Clase C#
        private readonly Dictionary<string, Type> _types = new();

        // Subcarpeta de la vista para cada bloque (opcional)
        private readonly Dictionary<string, string> _viewFolders = new();

        /// <summary>
        /// Registra un nuevo tipo de bloque en el sistema.
        /// </summary>
        /// <param name="viewFolder">Subcarpeta dentro de Views/Cms/Blocks/ donde vive el .cshtml (opcional)</param>
        public void Register<T>(string? viewFolder = null) where T : class
        {
            var type = typeof(T);
            _types[type.Name] = type;
            if (!string.IsNullOrEmpty(viewFolder))
                _viewFolders[type.Name] = viewFolder;
        }

        /// <summary>
        /// Devuelve la ruta completa de la vista para un tipo de bloque.
        /// </summary>
        public string GetViewPath(string blockType)
        {
            if (_viewFolders.TryGetValue(blockType, out var folder))
                return $"~/Views/Cms/Blocks/{folder}/{blockType}.cshtml";
            return $"~/Views/Cms/Blocks/{blockType}.cshtml";
        }

        /// <summary>
        /// Obtiene el tipo de C# asociado a un nombre de bloque.
        /// </summary>
        public Type? GetBlockType(string name)
        {
            return _types.TryGetValue(name, out var type) ? type : null;
        }

        /// <summary>
        /// Devuelve todos los bloques registrados para el panel lateral del editor.
        /// </summary>
        public Dictionary<string, Type> GetAllBlocks()
        {
            return _types;
        }

        /// <summary>
        /// Devuelve solo los nombres de los bloques registrados.
        /// </summary>
        public IEnumerable<string> GetRegisteredNames() => _types.Keys;

        /// <summary>
        /// NUEVO M�TODO: Convierte un bloque crudo de la base de datos (Data.Block)
        /// en un objeto de modelo real (como ChatBlock, HeroBlock, etc.)
        /// </summary>
        /// <param name="block">La entidad de bloque que viene de la base de datos</param>
        /// <returns>El objeto mapeado con sus datos cargados o null si no se encuentra el tipo</returns>
        public object? CreateModel(BrixCMS.Open.Data.Block block)
        {
            // 1. Buscamos qu� clase de C# corresponde al tipo de bloque (ej: "ChatBlock")
            var type = GetBlockType(block.Type);
            if (type == null) return null;

            // 2. Si no tiene datos JSON, devolvemos una instancia limpia de la clase
            if (string.IsNullOrEmpty(block.JsonData))
                return Activator.CreateInstance(type);

            try
            {
                // 3. Deserializamos el JSON de la DB al objeto real. 
                // Esto llena las propiedades como CustomPrompt, Title, etc.
                return JsonSerializer.Deserialize(block.JsonData, type, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception)
            {
                // 4. En caso de error en el JSON, devolvemos una instancia vac�a para no romper la web
                return Activator.CreateInstance(type);
            }
        }
    }
}
