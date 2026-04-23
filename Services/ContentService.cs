using System.Reflection;
using BrixCMS.Open.Models.Base;

namespace BrixCMS.Open.Services;

public class ContentService
{
    public List<Type> GetAvailableBlockTypes()
    {
        // Buscamos todas las clases que herbrix de BlockBase
        return Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsSubclassOf(typeof(BlockBase)) && !t.IsAbstract)
            .ToList();
    }
}
