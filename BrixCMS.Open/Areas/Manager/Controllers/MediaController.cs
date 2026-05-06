using BrixCMS.Open.Areas.Manager.Filters;
using BrixCMS.Open.Areas.Manager.Models;
using Microsoft.AspNetCore.Mvc;

namespace BrixCMS.Open.Areas.Manager.Controllers
{
    [Area("Manager")]
    [RequireAdminLogin]
    [AutoValidateAntiforgeryToken]
    public class MediaController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _basePath;

        public MediaController(IWebHostEnvironment env)
        {
            _env = env;
            _basePath = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(_basePath)) Directory.CreateDirectory(_basePath);
        }

        private bool HasPermission(string perm)
        {
            var role = HttpContext.Session.GetString("AdminRole") ?? "admin";
            if (role is "owner" or "admin") return true;
            var perms = HttpContext.Session.GetString("AdminPermissions") ?? "";
            return perms.Contains($"\"{perm}\"");
        }

        public IActionResult Index(string folder = "", bool picker = false)
        {
            if (!HasPermission("media")) return RedirectToAction("Index", "Manager");
            ViewBag.PickerMode = picker;

            var currentPath = Path.Combine(_basePath, folder ?? "");

            // Seguridad: evitar salir de la carpeta uploads
            if (!currentPath.StartsWith(_basePath))
                currentPath = _basePath;

            ViewBag.CurrentFolder = folder;

            // Construir breadcrumbs
            var breadcrumbs = new List<(string Name, string Path)>();
            if (!string.IsNullOrEmpty(folder))
            {
                var parts = folder.Split('/');
                var accumulatedPath = "";
                foreach (var part in parts)
                {
                    accumulatedPath = string.IsNullOrEmpty(accumulatedPath) ? part : accumulatedPath + "/" + part;
                    breadcrumbs.Add((part, accumulatedPath));
                }
            }
            ViewBag.Breadcrumbs = breadcrumbs;

            var model = new MediaViewModel
            {
                Folders = Directory.GetDirectories(currentPath)
                    .Select(Path.GetFileName)
                    .OrderBy(f => f)
                    .ToList()!,
                Files = Directory.GetFiles(currentPath)
                    .Where(f => IsImageFile(f))
                    .Select(Path.GetFileName)
                    .OrderBy(f => f)
                    .ToList()!,
                AllFolders = new List<string> { "" }.Concat(GetAllFoldersRecursive("")).ToList()
            };

            return View(model);
        }

        private static readonly HashSet<string> _allowedMimeTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg", "image/png", "image/gif", "image/webp", "image/svg+xml",
            "video/mp4", "video/webm",
            "application/pdf"
        };

        [HttpPost]
        public async Task<IActionResult> Upload(string folder, IFormFile file)
        {
            const long MaxImageBytes = 10 * 1024 * 1024; // 10 MB
            if (file != null && file.Length > MaxImageBytes)
            {
                TempData["Error"] = $"File too large ({file.Length / 1024 / 1024} MB). Maximum allowed: 10 MB.";
                return RedirectToAction("Index", new { folder, picker = Request.Query["picker"] });
            }
            if (file != null && !_allowedMimeTypes.Contains(file.ContentType))
            {
                TempData["Error"] = $"File type '{file.ContentType}' is not allowed.";
                return RedirectToAction("Index", new { folder, picker = Request.Query["picker"] });
            }
            if (file != null && file.Length > 0)
            {
                // Prevent path traversal
                var safeFileName = Path.GetFileName(file.FileName);
                if (string.IsNullOrEmpty(safeFileName) || safeFileName.Contains(".."))
                {
                    TempData["Error"] = "Invalid file name.";
                    return RedirectToAction("Index", new { folder, picker = Request.Query["picker"] });
                }
                var uploadPath = Path.Combine(_basePath, folder ?? "");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, safeFileName);

                // Avoid overwriting existing files
                if (System.IO.File.Exists(filePath))
                {
                    var fileNameWithoutExt = Path.GetFileNameWithoutExtension(safeFileName);
                    var extension = Path.GetExtension(safeFileName);
                    var counter = 1;

                    while (System.IO.File.Exists(filePath))
                    {
                        var newFileName = $"{fileNameWithoutExt}_{counter}{extension}";
                        filePath = Path.Combine(uploadPath, newFileName);
                        counter++;
                    }
                }

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
            }

            return RedirectToAction("Index", new { folder, picker = Request.Query["picker"] });
        }

        [HttpPost]
        public IActionResult CreateFolder(string folder, string newFolderName)
        {
            if (!string.IsNullOrEmpty(newFolderName))
            {
                // Validar nombre de carpeta (sin caracteres especiales)
                newFolderName = SanitizeFolderName(newFolderName);

                if (!string.IsNullOrEmpty(newFolderName))
                {
                    var currentPath = Path.Combine(_basePath, folder ?? "");
                    var newFolderPath = Path.Combine(currentPath, newFolderName);

                    if (!Directory.Exists(newFolderPath))
                    {
                        Directory.CreateDirectory(newFolderPath);
                    }
                }
            }

            return RedirectToAction("Index", new { folder, picker = Request.Query["picker"] });
        }

        [HttpPost]
        public IActionResult Delete(string folder, string fileName)
        {
            var filePath = Path.Combine(_basePath, folder ?? "", fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            return RedirectToAction("Index", new { folder, picker = Request.Query["picker"] });
        }

        [HttpPost]
        public IActionResult MoveFile(string sourceFolder, string fileName, string targetFolder, bool picker = false)
        {
            if (string.IsNullOrEmpty(fileName))
                return RedirectToAction("Index", new { folder = sourceFolder, picker });

            var src = Path.Combine(_basePath, sourceFolder ?? "", fileName);
            var dstDir = Path.Combine(_basePath, targetFolder ?? "");

            if (!src.StartsWith(_basePath) || !dstDir.StartsWith(_basePath))
                return RedirectToAction("Index", new { folder = sourceFolder, picker });

            if (System.IO.File.Exists(src))
            {
                if (!Directory.Exists(dstDir)) Directory.CreateDirectory(dstDir);

                var dst = Path.Combine(dstDir, fileName);
                if (System.IO.File.Exists(dst))
                {
                    var nameNoExt = Path.GetFileNameWithoutExtension(fileName);
                    var ext = Path.GetExtension(fileName);
                    var counter = 1;
                    while (System.IO.File.Exists(dst))
                        dst = Path.Combine(dstDir, $"{nameNoExt}_{counter++}{ext}");
                }
                System.IO.File.Move(src, dst);
            }

            return RedirectToAction("Index", new { folder = sourceFolder, picker });
        }

        [HttpPost]
        public IActionResult MoveFolder(string parentFolder, string folderName, string targetParent, bool picker = false)
        {
            if (string.IsNullOrEmpty(folderName))
                return RedirectToAction("Index", new { folder = parentFolder, picker });

            var src = Path.Combine(_basePath, parentFolder ?? "", folderName);
            var dst = Path.Combine(_basePath, targetParent ?? "", folderName);

            if (!src.StartsWith(_basePath) || !dst.StartsWith(_basePath))
                return RedirectToAction("Index", new { folder = parentFolder, picker });

            if (dst.StartsWith(src + Path.DirectorySeparatorChar) || dst == src)
            {
                TempData["Error"] = "Cannot move a folder into itself.";
                return RedirectToAction("Index", new { folder = parentFolder, picker });
            }

            if (Directory.Exists(src))
            {
                if (Directory.Exists(dst))
                {
                    TempData["Error"] = $"A folder named '{folderName}' already exists in the destination.";
                }
                else
                {
                    var dstParentDir = Path.Combine(_basePath, targetParent ?? "");
                    if (!Directory.Exists(dstParentDir)) Directory.CreateDirectory(dstParentDir);
                    Directory.Move(src, dst);
                }
            }

            return RedirectToAction("Index", new { folder = parentFolder, picker });
        }

        [HttpPost]
        public IActionResult DeleteFolder(string folder, string folderName, bool force = false)
        {
            var folderPath = Path.Combine(_basePath, folder ?? "", folderName);

            // Security: must stay inside uploads/
            if (!folderPath.StartsWith(_basePath, StringComparison.OrdinalIgnoreCase))
                return RedirectToAction("Index", new { folder, picker = Request.Query["picker"] });

            if (Directory.Exists(folderPath))
            {
                bool isEmpty = !Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories).Any();
                if (isEmpty || force)
                    Directory.Delete(folderPath, recursive: true);
                else
                    TempData["Error"] = "Folder is not empty. Use force delete to remove it with all its contents.";
            }

            return RedirectToAction("Index", new { folder, picker = Request.Query["picker"] });
        }

        // -- POST /Manager/Media/DeleteMultiple -------------------------------
        /// <summary>Batch-deletes a list of files and/or folders in the current folder.</summary>
        [HttpPost]
        public IActionResult DeleteMultiple(
            string folder,
            [FromForm] List<string> files,
            [FromForm] List<string> folders,
            bool force = false)
        {
            var currentPath = Path.Combine(_basePath, folder ?? "");

            foreach (var fileName in files ?? new List<string>())
            {
                var filePath = Path.Combine(currentPath, fileName);
                if (!filePath.StartsWith(_basePath, StringComparison.OrdinalIgnoreCase)) continue;
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            foreach (var folderName in folders ?? new List<string>())
            {
                var folderPath = Path.Combine(currentPath, folderName);
                if (!folderPath.StartsWith(_basePath, StringComparison.OrdinalIgnoreCase)) continue;
                if (Directory.Exists(folderPath))
                {
                    bool isEmpty = !Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories).Any();
                    if (isEmpty || force)
                        Directory.Delete(folderPath, recursive: true);
                }
            }

            return RedirectToAction("Index", new { folder, picker = Request.Query["picker"] });
        }

        private List<string> GetAllFoldersRecursive(string relPath)
        {
            var result = new List<string>();
            var absPath = string.IsNullOrEmpty(relPath) ? _basePath : Path.Combine(_basePath, relPath);
            if (!Directory.Exists(absPath)) return result;
            foreach (var dir in Directory.GetDirectories(absPath).OrderBy(d => d))
            {
                var name = Path.GetFileName(dir);
                var sub = string.IsNullOrEmpty(relPath) ? name : relPath + "/" + name;
                result.Add(sub);
                result.AddRange(GetAllFoldersRecursive(sub));
            }
            return result;
        }

        // -- GET /Manager/Media/ListFolder?folder=figma/BrixCMS/spring-1 ----------
        /// <summary>Returns a simple list of image file names inside the given uploads sub-folder.
        /// Used by the Figma "Cached Pages" panel to show preview thumbnails.</summary>
        [HttpGet]
        public IActionResult ListFolder([FromQuery] string folder = "")
        {
            var cleanFolder = (folder ?? "").Replace('\\', '/').Trim('/');
            var absPath     = Path.GetFullPath(Path.Combine(_basePath, cleanFolder));

            // Security: must stay inside uploads/
            if (!absPath.StartsWith(_basePath, StringComparison.OrdinalIgnoreCase))
                return Json(new { files = new List<string>() });

            if (!Directory.Exists(absPath))
                return Json(new { files = new List<string>() });

            var files = Directory.GetFiles(absPath)
                .Where(f => IsImageFile(f))
                .Select(Path.GetFileName)
                .OrderBy(f => f)
                .ToList();

            return Json(new { files });
        }

        private bool IsImageFile(string fileName)
        {
            var extensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".svg" };
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            return extensions.Contains(ext);
        }

        private string SanitizeFolderName(string name)
        {
            // Eliminar caracteres no v�lidos
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c.ToString(), "");
            }

            // Reemplazar espacios por guiones
            name = name.Replace(" ", "-").Trim();

            return name;
        }
    }
}
