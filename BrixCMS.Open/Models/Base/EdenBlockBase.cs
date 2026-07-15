using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.Base;

/// <summary>
/// Shared base for all blocks. Provides layout, spacing, animation and font fields
/// that apply at the wrapper level in Index.cshtml.
///
/// These fields carry [Header(...)] so the editor groups them into clean, collapsible
/// sections after each block's own content fields: Spacing → Layout → Advanced.
/// The editor pushes base-class fields to the end (see _BlockEditor.cshtml), so the order
/// here is what renders regardless of reflection quirks.
/// </summary>
public abstract class brixBlockBase
{
    // ── SPACING ────────────────────────────────────────────────────────────
    // BlockPadding renders as the visual click-an-edge spacing box in the editor;
    // BlockMargin is drawn inside that same box, so it has no separate control.

    [Header("Spacing")]
    [Field(Title = "Block Padding", Placeholder = "2rem | 1rem 2rem | 0.5rem 1rem 2rem 1rem")]
    public StringField BlockPadding { get; set; } = new() { Value = "" };

    [Field(Title = "Block Margin", Placeholder = "0 | 0 auto | 1rem 0")]
    public StringField BlockMargin { get; set; } = new() { Value = "" };


    // ── LAYOUT ──────────────────────────────────────────────────────────────

    [Header("Layout")]
    [Field(Title = "Full Width",
           Description = "Enabled: block takes full width. Disabled: respects the central container.")]
    public BoolField FullWidth { get; set; } = new() { Value = "false" };

    [Field(Title = "Vertical Content Alignment",
           Placeholder = "top | center | bottom",
           Description = "Controls the vertical position of content within the block")]
    public StringField BlockVerticalAlign { get; set; } = new() { Value = "top" };


    // ── ADVANCED (hidden in the editor's Simple mode) ───────────────────────

    [Header("Advanced", Advanced = true)]
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

    [Field(Title = "Block Font (Google Fonts)",
           Description = "Overrides the site font for this block only")]
    public FontSelectField BlockFont { get; set; } = new();
}
