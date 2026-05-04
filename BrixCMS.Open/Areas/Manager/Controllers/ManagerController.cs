using BrixCMS.Open.Areas.Manager.Filters;
using BrixCMS.Open.Data;
using BrixCMS.Open.Data.Fields;
using BrixCMS.Open.DTOs;
using BrixCMS.Open.Extensions;
using BrixCMS.Open.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;

namespace BrixCMS.Open.Areas.Manager.Controllers;

[Area("Manager")]
[RequireAdminLogin]
public class ManagerController : Controller
{
    private readonly BrixDbContext _db;
    private readonly BlockRegistry _registry;

    public ManagerController(BrixDbContext db, BlockRegistry registry)
    {
        _db = db;
        _registry = registry;
    }

    // LISTADO DE P�GINAS
    public async Task<IActionResult> Index()
    {
        var pages = await _db.Pages.ToListAsync();
        return View(pages);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePage(string title, string pageType = "standard")
    {
        if (string.IsNullOrEmpty(title)) return RedirectToAction(nameof(Index));

        var maxOrder = await _db.Pages.AnyAsync()
            ? await _db.Pages.MaxAsync(p => p.SortOrder) + 1
            : 0;

        var page = new Page
        {
            Id = Guid.NewGuid(),
            Title = title,
            Slug = title.ToLower().Trim().Replace(" ", "-"),
            SortOrder = maxOrder,
            PageType = pageType  // ? �nico cambio aqu�
        };

        _db.Pages.Add(page);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // ? NUEVO � mover p�gina arriba/abajo
    [HttpPost]
    public async Task<IActionResult> MovePage(Guid pageId, string direction)
    {
        var pages = await _db.Pages.OrderBy(p => p.SortOrder).ToListAsync();
        var index = pages.FindIndex(p => p.Id == pageId);
        if (index == -1) return RedirectToAction(nameof(Index));

        int newIndex = direction == "up" ? index - 1 : index + 1;
        if (newIndex < 0 || newIndex >= pages.Count)
            return RedirectToAction(nameof(Index));

        // Intercambiar SortOrder
        var temp = pages[index].SortOrder;
        pages[index].SortOrder = pages[newIndex].SortOrder;
        pages[newIndex].SortOrder = temp;

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // EDITOR DE P�GINA (GET)
    public async Task<IActionResult> Edit(Guid id)
    {
        var page = await _db.Pages.FirstOrDefaultAsync(p => p.Id == id);
        if (page == null) return NotFound();

        // ? Cargar TODOS los bloques (ra�ces + hijos) en page.Blocks
        page.Blocks = await _db.Blocks
            .Where(b => b.PageId == id)
            .OrderBy(b => b.SortOrder)
            .ToListAsync();

        // Solo ra�ces para el panel lateral del editor
        var rootBlocks = page.Blocks.Where(b => b.ParentId == null).ToList();

        // Metadata de bloques disponibles para el selector
        var availableBlocks = _registry.GetRegisteredNames()
            .Select(name => {
                var type = _registry.GetBlockType(name);
                return new
                {
                    Type = name,
                    Info = type?.GetCustomAttribute<BlockTypeAttribute>()
                };
            })
            .Where(x => x.Info != null)
            .ToList();

        // Media Library
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        var mediaUrls = new List<string>();
        if (Directory.Exists(basePath))
        {
            mediaUrls = Directory.GetFiles(basePath, "*.*", SearchOption.AllDirectories)
                .Select(f => "/uploads/" + Path.GetRelativePath(basePath, f).Replace("\\", "/"))
                .ToList();
        }

        ViewBag.Blocks = rootBlocks;
        ViewBag.AvailableBlocks = availableBlocks;
        ViewBag.MediaLibrary = mediaUrls;

        return View(page);
    }

    // ? PREVIEW DE P�GINA
    public async Task<IActionResult> Preview(Guid id)
    {
        var page = await _db.Pages.FirstOrDefaultAsync(p => p.Id == id);
        if (page == null) return NotFound();

        // Cargar TODOS los bloques: ra�ces + hijos + nietos
        page.Blocks = await _db.Blocks
            .Where(b => b.PageId == id)
            .OrderBy(b => b.SortOrder)
            .ToListAsync();

        return View(page);
    }

    // ? PUBLICAR P�GINA DESDE PREVIEW
    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> PublishPage(Guid pageId, [FromBody] PublishPageDto data)
    {
        var page = await _db.Pages.FindAsync(pageId);
        if (page == null)
            return Json(new { success = false, error = "P�gina no encontrada" });

        try
        {
            page.Title = data.Title;
            page.Slug = data.Slug;
            page.JsonData = data.JsonData;
            page.IsPublished = true;
            page.PublishedAt = DateTime.UtcNow;

            var existing = await _db.Blocks.Where(b => b.PageId == pageId).ToListAsync();
            _db.Blocks.RemoveRange(existing);
            await _db.SaveChangesAsync();

            // ? Mapa de ID viejo ? ID nuevo
            var idMap = new Dictionary<Guid, Guid>();

            // ? Primero insertar ra�ces y registrar sus IDs nuevos
            foreach (var b in data.Blocks.Where(b => b.ParentId == null))
            {
                var newId = Guid.NewGuid();
                idMap[Guid.Parse(b.OriginalId)] = newId; // guardar mapeo

                _db.Blocks.Add(new Block
                {
                    Id = newId,
                    PageId = pageId,
                    Type = b.Type,
                    JsonData = b.JsonData,
                    SortOrder = b.SortOrder,
                    ParentId = null
                });
            }

            // ? Luego hijos con ParentId resuelto al nuevo ID
            foreach (var b in data.Blocks.Where(b => b.ParentId != null))
            {
                var newId = Guid.NewGuid();
                var newParentId = b.ParentId.HasValue && idMap.ContainsKey(b.ParentId.Value)
                    ? idMap[b.ParentId.Value]
                    : b.ParentId;

                _db.Blocks.Add(new Block
                {
                    Id = newId,
                    PageId = pageId,
                    Type = b.Type,
                    JsonData = b.JsonData,
                    SortOrder = b.SortOrder,
                    ParentId = newParentId
                });
            }

            await _db.SaveChangesAsync();

            // Delete seed pages when the first real page is published
            page.IsSeed = false;
            var seeds = await _db.Pages.Where(p => p.IsSeed && p.Id != pageId).ToListAsync();
            foreach (var s in seeds)
            {
                _db.Blocks.RemoveRange(_db.Blocks.Where(b => b.PageId == s.Id));
                _db.Pages.Remove(s);
            }
            if (seeds.Any()) await _db.SaveChangesAsync();

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, error = ex.Message });
        }
    }

    // A�ADIR BLOQUE (SOPORTA PARENTID PARA COLUMNAS)
    [HttpPost]
    public async Task<IActionResult> AddBlock(Guid pageId, string blockType, Guid? parentId = null)
    {
        if (string.IsNullOrEmpty(blockType))
        {
            TempData["Error"] = "El tipo de bloque no puede estar vac�o";
            return RedirectToAction(nameof(Edit), new { id = pageId });
        }

        var block = new Block
        {
            Id = Guid.NewGuid(),
            PageId = pageId,
            ParentId = parentId,
            Type = blockType,
            JsonData = "{}",
            SortOrder = await _db.Blocks.CountAsync(b => b.PageId == pageId && b.ParentId == parentId)
        };

        _db.Blocks.Add(block);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Edit), new { id = pageId });
    }

    // GUARDAR DATOS DEL BLOQUE (JSON)
    [HttpPost]
    public async Task<IActionResult> SaveBlock(Guid blockId, Guid pageId, IFormCollection form)
    {
        var block = await _db.Blocks.FindAsync(blockId);
        if (block == null) return NotFound();

        var cleanedData = new Dictionary<string, object>();

        foreach (var key in form.Keys.Where(k => k.StartsWith("values[")))
        {
            var parts = key.Split('[', ']');
            if (parts.Length < 2) continue;
            
            var propName = parts[1];
            var subProp = parts.Length > 2 ? parts[2].TrimStart('.') : "Value";
            var value = form[key].ToString();

            if (!cleanedData.ContainsKey(propName))
            {
                cleanedData[propName] = new Dictionary<string, object>();
            }

            var propDict = (Dictionary<string, object>)cleanedData[propName];
            propDict[subProp] = value;
        }

        block.JsonData = JsonSerializer.Serialize(cleanedData);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Edit), new { id = pageId });
    }

    // BORRAR BLOQUE (CON BORRADO RECURSIVO DE HIJOS)
    [HttpPost]
    public async Task<IActionResult> DeleteBlock(Guid blockId, Guid pageId)
    {
        var block = await _db.Blocks.FindAsync(blockId);
        if (block != null)
        {
            var children = await _db.Blocks.Where(b => b.ParentId == blockId).ToListAsync();
            foreach (var child in children)
            {
                var grandChildren = await _db.Blocks.Where(b => b.ParentId == child.Id).ToListAsync();
                _db.Blocks.RemoveRange(grandChildren);
            }
            _db.Blocks.RemoveRange(children);
            _db.Blocks.Remove(block);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Edit), new { id = pageId });
    }

    // BORRAR P�GINA ENTERA
    [HttpPost]
    public async Task<IActionResult> DeletePage(Guid id)
    {
        var page = await _db.Pages.FindAsync(id);
        if (page != null)
        {
            var blocks = await _db.Blocks.Where(b => b.PageId == id).ToListAsync();
            _db.Blocks.RemoveRange(blocks);
            _db.Pages.Remove(page);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    // MOVER BLOQUE
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MoveBlock(string blockId, string direction, string pageId)
    {
        if (!Guid.TryParse(blockId, out Guid blockGuid)) return NotFound();
        if (!Guid.TryParse(pageId, out Guid pageGuid)) return NotFound();

        var block = await _db.Blocks.FindAsync(blockGuid);
        if (block == null) return NotFound();

        var siblings = await _db.Blocks
            .Where(b => b.ParentId == block.ParentId && b.PageId == pageGuid)
            .OrderBy(b => b.SortOrder)
            .ToListAsync();

        var currentIndex = siblings.FindIndex(b => b.Id == blockGuid);
        if (currentIndex == -1) return NotFound();

        int newIndex = direction == "up" ? currentIndex - 1 : currentIndex + 1;
        if (newIndex < 0 || newIndex >= siblings.Count)
            return RedirectToAction("Edit", new { id = pageGuid });

        var tempSortOrder = block.SortOrder;
        block.SortOrder = siblings[newIndex].SortOrder;
        siblings[newIndex].SortOrder = tempSortOrder;

        await _db.SaveChangesAsync();
        return RedirectToAction("Edit", new { id = pageGuid });
    }

    // PUBLICAR P�GINA DIRECTAMENTE DESDE EL EDITOR
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> QuickPublish(Guid pageId)
    {
        var page = await _db.Pages.FindAsync(pageId);
        if (page == null) return NotFound();

        page.IsPublished = true;
        page.PublishedAt = DateTime.UtcNow;
        page.IsSeed = false;

        var seeds = await _db.Pages.Where(p => p.IsSeed && p.Id != pageId).ToListAsync();
        foreach (var s in seeds)
        {
            _db.Blocks.RemoveRange(_db.Blocks.Where(b => b.PageId == s.Id));
            _db.Pages.Remove(s);
        }

        await _db.SaveChangesAsync();
        return RedirectToAction("Edit", new { id = pageId });
    }

    // GUARDAR CONFIGURACI�N DE P�GINA (FONDO, ETC.)
    [HttpPost]
    public async Task<IActionResult> SavePageSettings(Guid pageId, string backgroundColor, string fontFamily = "", string metaDescription = "", string ogImage = "", string pageTitle = "")
    {
        var page = await _db.Pages.FindAsync(pageId);
        if (page == null) return NotFound();

        var settings = new BrixCMS.Open.Models.Pages.StandardPage
        {
            BackgroundColor = new BrixCMS.Open.Data.Fields.ColorField { Value = backgroundColor },
            FontFamily      = new BrixCMS.Open.Data.Fields.StringField { Value = fontFamily },
        };

        page.JsonData        = JsonSerializer.Serialize(settings);
        page.MetaDescription = metaDescription;
        page.OgImage         = ogImage;
        if (!string.IsNullOrWhiteSpace(pageTitle))
            page.Title = pageTitle.Trim();

        await _db.SaveChangesAsync();
        return RedirectToAction("Edit", new { id = pageId });
    }
}
