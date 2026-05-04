using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace TestBrixCMS.Controllers;

/// <summary>
/// Serves images with on-the-fly resizing, format conversion, and quality control.
/// URL pattern: /img/{*path}?width=800&quality=80&format=webp
/// </summary>
[Route("img")]
public class ImageController : Controller
{
    private readonly IWebHostEnvironment _env;
    private readonly IMemoryCache _cache;
    private readonly ILogger<ImageController> _logger;

    private static readonly TimeSpan CacheDuration = TimeSpan.FromDays(7);

    public ImageController(IWebHostEnvironment env, IMemoryCache cache, ILogger<ImageController> logger)
    {
        _env = env;
        _cache = cache;
        _logger = logger;
    }

    [HttpGet("{*imagePath}")]
    [ResponseCache(Duration = 604800, Location = ResponseCacheLocation.Any, VaryByQueryKeys = ["width", "quality", "format"])]
    public async Task<IActionResult> Serve(
        string imagePath,
        [FromQuery] int? width,
        [FromQuery] int quality = 82,
        [FromQuery] string? format = null)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
            return NotFound();

        // Sanitize — no directory traversal
        imagePath = imagePath.Replace('\\', '/').TrimStart('/');
        if (imagePath.Contains(".."))
            return BadRequest();

        // Resolve physical path: check uploads/ first, then images/
        var physicalPath = Path.Combine(_env.WebRootPath, "uploads", imagePath);
        if (!System.IO.File.Exists(physicalPath))
            physicalPath = Path.Combine(_env.WebRootPath, "images", imagePath);

        if (!System.IO.File.Exists(physicalPath))
            return NotFound();

        // Clamp values
        quality = Math.Clamp(quality, 10, 100);
        if (width.HasValue)
            width = Math.Clamp(width.Value, 1, 4000);

        // Normalize format
        var outputFormat = format?.ToLowerInvariant() switch
        {
            "webp"         => "webp",
            "avif"         => "webp",   // ImageSharp has no AVIF encoder yet — fall back to WebP
            "png"          => "png",
            "jpg" or "jpeg"=> "jpeg",
            _              => null      // keep original
        };

        var cacheKey = $"img|{imagePath}|{width}|{quality}|{outputFormat}";

        if (_cache.TryGetValue(cacheKey, out (byte[] bytes, string mime) cached))
            return File(cached.bytes, cached.mime);

        try
        {
            using var image = await Image.LoadAsync(physicalPath);

            if (width.HasValue && width.Value < image.Width)
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size    = new Size(width.Value, 0),
                    Mode    = ResizeMode.Max,
                    Sampler = KnownResamplers.Lanczos3
                }));
            }

            var ext = Path.GetExtension(physicalPath).TrimStart('.').ToLowerInvariant();
            var finalFormat = outputFormat ?? ext;

            using var ms = new MemoryStream();
            string mimeType;

            switch (finalFormat)
            {
                case "webp":
                    mimeType = "image/webp";
                    await image.SaveAsWebpAsync(ms, new WebpEncoder { Quality = quality });
                    break;

                case "png":
                    mimeType = "image/png";
                    await image.SaveAsPngAsync(ms, new PngEncoder
                    {
                        CompressionLevel = PngCompressionLevel.BestSpeed
                    });
                    break;

                default: // jpeg / jpg / unknown
                    mimeType = "image/jpeg";
                    await image.SaveAsJpegAsync(ms, new JpegEncoder { Quality = quality });
                    break;
            }

            var bytes = ms.ToArray();

            _cache.Set(cacheKey, (bytes, mimeType), new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = CacheDuration,
                Size = bytes.Length
            });

            return File(bytes, mimeType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process image: {Path}", imagePath);
            return NotFound();
        }
    }
}
