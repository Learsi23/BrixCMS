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
[AutoValidateAntiforgeryToken]
public class ManagerController : Controller
{
    private readonly BrixDbContext _db;
    private readonly BlockRegistry _registry;
    private readonly SiteSettingsImporter _siteImporter;
    private readonly PageVersionService _versions;

    public ManagerController(BrixDbContext db, BlockRegistry registry, SiteSettingsImporter siteImporter, PageVersionService versions)
    {
        _versions = versions;
        _db = db;
        _registry = registry;
        _siteImporter = siteImporter;
    }

    // LISTADO DE P�GINAS
    public async Task<IActionResult> Index()
    {
        var pages = await _db.Pages.ToListAsync();
        return View(pages);
    }

    [HttpPost]
    public async Task<IActionResult> RenamePage(Guid pageId, string title, bool updateSlug = true, string? returnUrl = null)
    {
        var page = await _db.Pages.FindAsync(pageId);
        if (page == null) return BackTo(returnUrl);

        var oldSlug = page.Slug;
        page.Title = title;
        if (updateSlug)
            page.Slug = title.ToLower().Trim().Replace(" ", "-");

        await _db.SaveChangesAsync();

        if (page.IsPublished)
        {
            if (!string.IsNullOrEmpty(oldSlug) && oldSlug != page.Slug)
                await _siteImporter.RemovePageFromNavAsync(oldSlug);
            await _siteImporter.AddPageToNavAsync(page.Title, page.Slug ?? "", isSubpage: page.ParentId.HasValue);
        }

        TempData["Success"] = $"Renamed to \"{title}\".";
        return BackTo(returnUrl);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePage(string title, string pageType = "standard", Guid? parentId = null)
    {
        if (string.IsNullOrEmpty(title)) return RedirectToAction(nameof(Index));

        var maxOrder = await _db.Pages
            .Where(p => p.ParentId == parentId)
            .OrderByDescending(p => p.SortOrder)
            .Select(p => (int?)p.SortOrder)
            .FirstOrDefaultAsync() ?? -1;

        var page = new Page
        {
            Id = Guid.NewGuid(),
            Title = title,
            Slug = title.ToLower().Trim().Replace(" ", "-"),
            SortOrder = maxOrder + 1,
            PageType = pageType,
            ParentId = parentId,
        };

        _db.Pages.Add(page);
        await _db.SaveChangesAsync();

        if (page.ParentId == null)
            await _siteImporter.AddPageToNavAsync(page.Title, page.Slug ?? "");

        return RedirectToAction(nameof(Index));
    }

    // ? NUEVO � mover p�gina arriba/abajo (scope a hermanos)
    [HttpPost]
    public async Task<IActionResult> MovePage(Guid pageId, string direction, string? returnUrl = null)
    {
        var current = await _db.Pages.FindAsync(pageId);
        if (current == null) return BackTo(returnUrl);

        var siblings = await _db.Pages
            .Where(p => p.ParentId == current.ParentId)
            .OrderBy(p => p.SortOrder)
            .ToListAsync();
        var index = siblings.FindIndex(p => p.Id == pageId);
        if (index == -1) return BackTo(returnUrl);

        int newIndex = direction == "up" ? index - 1 : index + 1;
        if (newIndex < 0 || newIndex >= siblings.Count)
            return BackTo(returnUrl);

        var temp = siblings[index].SortOrder;
        siblings[index].SortOrder = siblings[newIndex].SortOrder;
        siblings[newIndex].SortOrder = temp;

        await _db.SaveChangesAsync();
        return BackTo(returnUrl);
    }

    // RE-PARENT A PAGE: convert top-level -> subpage or subpage -> top-level.
    // Validates: no cycles, slug unique among new siblings.
    // Syncs navbar: top-level pages appear there; subpages don't.
    [HttpPost]
    public async Task<IActionResult> SetPageParent(Guid pageId, Guid? newParentId, string? returnUrl = null)
    {
        var page = await _db.Pages.FindAsync(pageId);
        if (page == null) { TempData["Error"] = "Page not found."; return BackTo(returnUrl); }

        if (page.ParentId == newParentId)
            return BackTo(returnUrl);

        if (newParentId == page.Id)
        {
            TempData["Error"] = "A page cannot be its own parent.";
            return BackTo(returnUrl);
        }

        if (newParentId.HasValue)
        {
            var newParent = await _db.Pages.FindAsync(newParentId.Value);
            if (newParent == null) { TempData["Error"] = "New parent page not found."; return BackTo(returnUrl); }

            var allPages = await _db.Pages.Select(p => new { p.Id, p.ParentId }).ToListAsync();
            var byId = allPages.ToDictionary(p => p.Id);
            var cursor = newParentId.Value;
            int hops = 0;
            while (byId.TryGetValue(cursor, out var node) && node.ParentId.HasValue && hops++ < 50)
            {
                if (node.ParentId.Value == pageId)
                {
                    TempData["Error"] = "Cannot move — that would create a loop.";
                    return BackTo(returnUrl);
                }
                cursor = node.ParentId.Value;
            }
        }

        if (!string.IsNullOrEmpty(page.Slug))
        {
            var clash = await _db.Pages.AnyAsync(p => p.Id != pageId && p.ParentId == newParentId && p.Slug == page.Slug);
            if (clash)
            {
                TempData["Error"] = "A sibling under the new parent already uses that slug.";
                return BackTo(returnUrl);
            }
        }

        var nextSort = await _db.Pages.Where(p => p.ParentId == newParentId).MaxAsync(p => (int?)p.SortOrder) ?? -1;
        var oldParentId = page.ParentId;
        page.ParentId = newParentId;
        page.SortOrder = nextSort + 1;

        await _db.SaveChangesAsync();

        if (page.IsPublished)
        {
            if (oldParentId == null && newParentId != null && !string.IsNullOrEmpty(page.Slug))
                await _siteImporter.RemovePageFromNavAsync(page.Slug);
            else if (oldParentId != null && newParentId == null)
                await _siteImporter.AddPageToNavAsync(page.Title, page.Slug ?? "");
        }

        TempData["Success"] = newParentId == null
            ? $"Promoted \"{page.Title}\" to top-level."
            : $"Moved \"{page.Title}\" under a new parent.";
        return BackTo(returnUrl);
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
    public async Task<IActionResult> Preview(Guid id, int frame = 0)
    {
        var page = await _db.Pages.FirstOrDefaultAsync(p => p.Id == id);
        if (page == null) return NotFound();

        // Cargar TODOS los bloques: ra�ces + hijos + nietos
        page.Blocks = await _db.Blocks
            .Where(b => b.PageId == id)
            .OrderBy(b => b.SortOrder)
            .ToListAsync();

        // frame=1: embedded inside the editor's "Live edit" iframe — hide the amber preview
        // banner so double-click-to-edit isn't fighting a fixed-position bar for space.
        ViewData["IsFrame"] = frame == 1;

        return View(page);
    }

    // ? PUBLICAR P�GINA DESDE PREVIEW
    [HttpPost]
    [ValidateAntiForgeryToken]   // token sent in X-CSRF-TOKEN header by csrfHeaders() JS helper
    public async Task<IActionResult> PublishPage(Guid pageId, [FromBody] PublishPageDto data)
    {
        var page = await _db.Pages.FindAsync(pageId);
        if (page == null)
            return Json(new { success = false, error = "P�gina no encontrada" });

        try
        {
            // Snapshot the pre-edit state so this save becomes an undo point — see
            // /Manager/PageVersions/{pageId}. Never blocks the publish if it fails.
            try { await _versions.SnapshotAsync(pageId, "Autosave", HttpContext.Session.GetString("AdminEmail")); }
            catch { /* history is best-effort — never block a publish over it */ }

            page.Title = data.Title;
            page.Slug = data.Slug;
            page.JsonData = data.JsonData;
            page.ParentId = data.ParentId;
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

            // Sync navbar/footer: remove old slug entry if slug changed, then add new one.
            var oldSlug = (await _db.Pages.AsNoTracking().Where(p => p.Id == pageId).Select(p => p.Slug).FirstOrDefaultAsync()) ?? "";
            if (!string.IsNullOrEmpty(oldSlug) && oldSlug != data.Slug)
                await _siteImporter.RemovePageFromNavAsync(oldSlug);
            await _siteImporter.AddPageToNavAsync(data.Title, data.Slug, isSubpage: page.ParentId.HasValue);

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
    public async Task<IActionResult> DeletePage(Guid id, string? returnUrl = null)
    {
        var page = await _db.Pages.FindAsync(id);
        if (page != null)
        {
            // Remove from navbar/footer before deleting
            if (page.IsPublished && !string.IsNullOrEmpty(page.Slug))
                await _siteImporter.RemovePageFromNavAsync(page.Slug);

            // Recursive delete: collect all descendant pages
            var toDelete = new List<Page>();
            var queue = new Queue<Guid>();
            queue.Enqueue(id);
            while (queue.Count > 0)
            {
                var cur = queue.Dequeue();
                var cp = await _db.Pages.FindAsync(cur);
                if (cp == null) continue;
                toDelete.Add(cp);
                var childIds = await _db.Pages.Where(p => p.ParentId == cur).Select(p => p.Id).ToListAsync();
                foreach (var cid in childIds) queue.Enqueue(cid);
            }

            foreach (var p in toDelete)
            {
                _db.Blocks.RemoveRange(_db.Blocks.Where(b => b.PageId == p.Id));
                _db.Pages.Remove(p);
            }
            await _db.SaveChangesAsync();
        }
        return BackTo(returnUrl ?? Url.Action(nameof(Index)));
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

    // REORDER BLOCKS (batch drag-and-drop) — renumbers SortOrder for one sibling set (the root
    // list, or one block-group's children, identified by parentId) in a single call instead of the
    // one-swap-at-a-time MoveBlock above. MoveBlock/the up-down buttons stay untouched as the
    // accessible fallback; this only adds an AJAX alternative that SortableJS in Edit.cshtml /
    // _BlockEditor.cshtml calls on drop. Returns JSON — no redirect/reload, unlike MoveBlock.
    [HttpPost]
    public async Task<IActionResult> ReorderBlocks(string pageId, string? parentId, string blockIdsJson)
    {
        if (!Guid.TryParse(pageId, out var pageGuid))
            return Json(new { error = "Invalid page ID." });

        Guid? parentGuid = null;
        if (!string.IsNullOrWhiteSpace(parentId))
        {
            if (!Guid.TryParse(parentId, out var pg)) return Json(new { error = "Invalid parent block ID." });
            parentGuid = pg;
        }

        List<Guid>? orderedIds;
        try { orderedIds = JsonSerializer.Deserialize<List<Guid>>(blockIdsJson); }
        catch { return Json(new { error = "Invalid block IDs." }); }

        if (orderedIds == null || orderedIds.Count == 0)
            return Json(new { error = "No block IDs provided." });

        var siblings = await _db.Blocks
            .Where(b => b.PageId == pageGuid && b.ParentId == parentGuid)
            .ToListAsync();

        if (siblings.Count == 0) return Json(new { error = "No blocks found." });

        var byId = siblings.ToDictionary(b => b.Id);
        int sort = 0;
        foreach (var id in orderedIds)
            if (byId.TryGetValue(id, out var b)) b.SortOrder = sort++;
        foreach (var b in siblings.OrderBy(b => b.SortOrder))
            if (!orderedIds.Contains(b.Id)) b.SortOrder = sort++;

        await _db.SaveChangesAsync();
        return Json(new { success = true });
    }

    private IActionResult BackTo(string? returnUrl) =>
        !string.IsNullOrEmpty(returnUrl) ? (IActionResult)Redirect(returnUrl) : RedirectToAction(nameof(Index));

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
        await _siteImporter.AddPageToNavAsync(page.Title, page.Slug ?? "", isSubpage: page.ParentId.HasValue);

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
