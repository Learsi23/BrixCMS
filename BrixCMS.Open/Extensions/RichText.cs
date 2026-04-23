using Microsoft.AspNetCore.Html;
using System.Text.RegularExpressions;
using System.Web;

namespace BrixCMS.Open.Extensions;

/// <summary>
/// Renders inline color markup: [text]{#hex} → &lt;span style="color:#hex"&gt;text&lt;/span&gt;
/// Usage in seeder/fields: "Din infrastruktur, [dina regler.]{#5B6EF5}"
/// Usage in views: @RichText.Render(Model.Title?.Value)
/// </summary>
public static class RichText
{
    private static readonly Regex ColorSpan =
        new(@"\[([^\]]+)\]\{([^}]+)\}", RegexOptions.Compiled);

    public static IHtmlContent Render(string? input)
    {
        if (string.IsNullOrEmpty(input))
            return HtmlString.Empty;

        var result = ColorSpan.Replace(
            HttpUtility.HtmlEncode(input),
            m => $"<span style=\"color:{HttpUtility.HtmlEncode(m.Groups[2].Value)}\">{m.Groups[1].Value}</span>"
        );

        return new HtmlString(result);
    }

    /// <summary>
    /// Processes [text]{#hex} markers on raw HTML content without HTML-encoding the input.
    /// Use this when the field may contain real HTML tags (e.g. HeroBlock title/subtitle).
    /// </summary>
    public static string ApplyColors(string? input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;
        return ColorSpan.Replace(input,
            m => $"<span style=\"color:{HttpUtility.HtmlEncode(m.Groups[2].Value)}\">{m.Groups[1].Value}</span>");
    }
}
