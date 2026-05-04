using System;
using System.Collections.Generic;
using System.Text.Json;

namespace TestBrixCMS.Services
{
    public class BlockRegistry
    {
        // Internal dictionary to map block name to its C# class
        private readonly Dictionary<string, Type> _types = new();

        // Optional view subfolder for each block
        private readonly Dictionary<string, string> _viewFolders = new();

        /// <summary>
        /// Registers a new block type in the system.
        /// </summary>
        /// <param name="viewFolder">Subfolder within Views/Cms/Blocks/ where the .cshtml file lives (optional)</param>
        public void Register<T>(string? viewFolder = null) where T : class
        {
            var type = typeof(T);
            _types[type.Name] = type;
            if (!string.IsNullOrEmpty(viewFolder))
                _viewFolders[type.Name] = viewFolder;
        }

        /// <summary>
        /// Returns the full view path for a block type.
        /// </summary>
        public string GetViewPath(string blockType)
        {
            if (_viewFolders.TryGetValue(blockType, out var folder))
                return $"~/Views/Cms/Blocks/{folder}/{blockType}.cshtml";
            return $"~/Views/Cms/Blocks/{blockType}.cshtml";
        }

        /// <summary>
        /// Gets the C# type associated with a block name.
        /// </summary>
        public Type? GetBlockType(string name)
        {
            return _types.TryGetValue(name, out var type) ? type : null;
        }

        /// <summary>
        /// Returns all registered blocks for the editor sidebar.
        /// </summary>
        public Dictionary<string, Type> GetAllBlocks()
        {
            return _types;
        }

        /// <summary>
        /// Returns only the names of registered blocks.
        /// </summary>
        public IEnumerable<string> GetRegisteredNames() => _types.Keys;

        /// <summary>
        /// NEW METHOD: Converts a raw block from the database (Data.Block)
        /// into a real model object (like ChatBlock, HeroBlock, etc.)
        /// </summary>
        /// <param name="block">The block entity coming from the database</param>
        /// <returns>The mapped object with its data loaded, or null if the type is not found</returns>
        public object? CreateModel(TestBrixCMS.Data.Block block)
        {
            // 1. Look up which C# class corresponds to the block type (e.g., "ChatBlock")
            var type = GetBlockType(block.Type);
            if (type == null) return null;

            // 2. If there's no JSON data, return a clean instance of the class
            if (string.IsNullOrEmpty(block.JsonData))
                return Activator.CreateInstance(type);

            try
            {
                // 3. Deserialize the JSON from DB into the actual object.
                // This populates properties like CustomPrompt, Title, etc.
                return JsonSerializer.Deserialize(block.JsonData, type, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception)
            {
                // 4. In case of JSON error, return an empty instance to avoid breaking the site
                return Activator.CreateInstance(type);
            }
        }
    }
}