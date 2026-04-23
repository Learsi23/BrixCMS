using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.Base;

/// <summary>
/// Shared base for all blocks. Provides layout, spacing, animation and font fields
/// that apply at the wrapper level in Index.cshtml.
/// </summary>
public abstract class brixBlockBase
{
    // ── LAYOUT ──────────────────────────────────────────────────────────────

    [Field(Title = "Full Width",
           Description = "Enabled: block takes full width. Disabled: respects the central container.")]
    public BoolField FullWidth { get; set; } = new() { Value = "false" };


    // ── SPACING ────────────────────────────────────────────────────────────

    [Field(Title = "Block Padding", Placeholder = "2rem | 1rem 2rem | 0.5rem 1rem 2rem 1rem")]
    public StringField BlockPadding { get; set; } = new() { Value = "" };

    [Field(Title = "Block Margin", Placeholder = "0 | 0 auto | 1rem 0")]
    public StringField BlockMargin { get; set; } = new() { Value = "" };


    // ── VERTICAL CONTENT ALIGNMENT ───────────────────────────────────

    [Field(Title = "Vertical Content Alignment",
           Placeholder = "top | center | bottom",
           Description = "Controls the vertical position of content within the block")]
    public StringField BlockVerticalAlign { get; set; } = new() { Value = "top" };


    // ── SCROLL ANIMATION ────────────────────────────────────────────

    [Field(Title = "✨ Animate on Scroll",
           Description = "Block will appear with animation when visible in the viewport")]
    public BoolField AnimateOnScroll { get; set; } = new() { Value = "false" };

    [Field(Title = "Animation Style",
           Placeholder = "fade-up | fade-down | fade-left | fade-right | zoom-in | zoom-out",
           VisibleWhen = "AnimateOnScroll:true")]
    public StringField AnimationStyle { get; set; } = new() { Value = "fade-up" };

    [Field(Title = "Animation Duration (ms)",
           Placeholder = "600",
           VisibleWhen = "AnimateOnScroll:true")]
    public StringField AnimationDuration { get; set; } = new() { Value = "600" };


    // ── BLOCK FONT ────────────────────────────────────────────────────

    [Field(Title = "Block Font (Google Fonts)",
           Description = "Overrides the site font for this block only")]
    public FontSelectField BlockFont { get; set; } = new();
}
