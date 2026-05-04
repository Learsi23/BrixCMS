using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TestBrixCMS.TagHelpers;

/// <summary>
/// Razor TagHelper that generates a fully-optimized &lt;picture&gt; element.
///
/// Usage:
///   &lt;optimized-img src="/uploads/photo.jpg" alt="A photo" width="800" height="600" /&gt;
///   &lt;optimized-img src="..." alt="..." critical="true" /&gt;  (disables lazy-load for LCP images)
///
/// Output:
///   &lt;picture&gt;
///     &lt;source type="image/avif" srcset="..." sizes="..." /&gt;
///     &lt;source type="image/webp" srcset="..." sizes="..." /&gt;
///     &lt;img src="..." srcset="..." sizes="..." loading="lazy" width="..." height="..." alt="..." /&gt;
///   &lt;/picture&gt;
/// </summary>
[HtmlTargetElement("optimized-img")]
public class OptimizedImgTagHelper : TagHelper
{
    private static readonly int[] DefaultWidths = [400, 800, 1200];

    /// <summary>Source image path (relative to /uploads or /images, or absolute URL).</summary>
    [HtmlAttributeName("src")]
    public string Src { get; set; } = "";

    [HtmlAttributeName("alt")]
    public string Alt { get; set; } = "";

    /// <summary>Display width in pixels used for &lt;img width="..."&gt; and as the default src width.</summary>
    [HtmlAttributeName("width")]
    public int? Width { get; set; }

    /// <summary>Display height in pixels used for &lt;img height="..."&gt; to prevent CLS.</summary>
    [HtmlAttributeName("height")]
    public int? Height { get; set; }

    /// <summary>Custom CSS class applied to the &lt;img&gt; element.</summary>
    [HtmlAttributeName("class")]
    public string? CssClass { get; set; }

    /// <summary>
    /// Responsive sizes hint.
    /// Defaults to "(max-width: 640px) 100vw, (max-width: 1024px) 50vw, 800px".
    /// </summary>
    [HtmlAttributeName("sizes")]
    public string? Sizes { get; set; }

    /// <summary>
    /// Set to true for above-the-fold / LCP images to disable lazy loading.
    /// </summary>
    [HtmlAttributeName("critical")]
    public bool Critical { get; set; } = false;

    /// <summary>JPEG/WebP quality (10–100). Defaults to 82.</summary>
    [HtmlAttributeName("quality")]
    public int Quality { get; set; } = 82;

    /// <summary>
    /// Comma-separated list of widths to generate in srcset.
    /// Defaults to "400,800,1200".
    /// </summary>
    [HtmlAttributeName("srcset-widths")]
    public string? SrcsetWidths { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (string.IsNullOrWhiteSpace(Src))
        {
            output.SuppressOutput();
            return;
        }

        // Parse srcset widths
        var widths = ParseWidths(SrcsetWidths) ?? DefaultWidths;

        // Default fallback width
        var defaultWidth = Width ?? widths[^1];

        // Sizes hint
        var sizesAttr = Sizes ?? "(max-width: 640px) 100vw, (max-width: 1024px) 50vw, 800px";

        // Determine if this is an external URL — skip processing for those
        var isExternal = Src.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                      || Src.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
                      || Src.StartsWith("//", StringComparison.OrdinalIgnoreCase);

        // Build the image base path for the /img route
        var basePath = isExternal ? Src : NormalizeToImgRoute(Src);

        // Build srcset string for a given format
        string BuildSrcset(string? fmt)
        {
            var parts = widths.Select(w =>
            {
                var url = isExternal ? Src : BuildUrl(basePath, w, Quality, fmt);
                return $"{url} {w}w";
            });
            return string.Join(", ", parts);
        }

        // Default src (no format override — browser picks from <source> elements)
        var defaultSrc = isExternal
            ? Src
            : BuildUrl(basePath, defaultWidth, Quality, null);

        // Loading strategy
        var loadingAttr = Critical ? "eager" : "lazy";
        var fetchPriority = Critical ? " fetchpriority=\"high\"" : "";

        // Class attribute
        var classAttr = string.IsNullOrWhiteSpace(CssClass) ? "" : $" class=\"{HtmlEncode(CssClass)}\"";

        // Width/height attributes (prevents CLS)
        var widthAttr  = Width.HasValue  ? $" width=\"{Width}\""  : "";
        var heightAttr = Height.HasValue ? $" height=\"{Height}\"" : "";

        // Output as <picture> element
        output.TagName = "picture";
        output.TagMode = TagMode.StartTagAndEndTag;

        var avifSrcset = BuildSrcset("avif");
        var webpSrcset = BuildSrcset("webp");
        var fallbackSrcset = BuildSrcset(null);

        output.Content.SetHtmlContent(
            $"""
            <source type="image/avif" srcset="{avifSrcset}" sizes="{sizesAttr}" />
            <source type="image/webp" srcset="{webpSrcset}" sizes="{sizesAttr}" />
            <img src="{defaultSrc}" srcset="{fallbackSrcset}" sizes="{sizesAttr}" loading="{loadingAttr}"{fetchPriority}{widthAttr}{heightAttr}{classAttr} alt="{HtmlEncode(Alt)}" />
            """);
    }

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    private static string NormalizeToImgRoute(string src)
    {
        // Strip leading /uploads/ or /images/ — ImageController handles both
        src = src.TrimStart('/');
        if (src.StartsWith("uploads/", StringComparison.OrdinalIgnoreCase))
            src = src["uploads/".Length..];
        else if (src.StartsWith("images/", StringComparison.OrdinalIgnoreCase))
            src = src["images/".Length..];

        return src;
    }

    private static string BuildUrl(string path, int width, int quality, string? format)
    {
        var qs = $"?width={width}&quality={quality}";
        if (!string.IsNullOrEmpty(format))
            qs += $"&format={format}";
        return $"/img/{path}{qs}";
    }

    private static string HtmlEncode(string value) =>
        System.Net.WebUtility.HtmlEncode(value);

    private static int[]? ParseWidths(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;
        var parts = raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var result = new List<int>();
        foreach (var p in parts)
            if (int.TryParse(p, out var n) && n > 0)
                result.Add(n);
        return result.Count > 0 ? [.. result] : null;
    }
}
