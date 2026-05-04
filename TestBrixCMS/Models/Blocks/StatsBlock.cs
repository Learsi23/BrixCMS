using TestBrixCMS.Data.Fields;
using TestBrixCMS.Models.Base;

namespace TestBrixCMS.Models.Blocks;

[BlockType(Name = "Stats", Category = "Content", Icon = "fas fa-chart-bar", Description = "Display key metrics or numbers with labels. Ideal for social proof and achievement sections.")]
public class StatsBlock : BlockBase
{
    [Header("Header")]
    [Field(Title = "Section Title")]
    public StringField Title { get; set; } = new();

    [Field(Title = "Title Color")]
    public ColorField TitleColor { get; set; } = new() { Value = "#111827" };

    [Field(Title = "Subtitle")]
    public StringField Subtitle { get; set; } = new();

    [Field(Title = "Subtitle Color")]
    public ColorField SubtitleColor { get; set; } = new() { Value = "#6b7280" };

    [Header("Statistic 1")]
    [Field(Title = "Number / Value", Placeholder = "Ex: +500, 99%, 10K")]
    public StringField Stat1Number { get; set; } = new();

    [Field(Title = "Label")]
    public StringField Stat1Label { get; set; } = new();

    [Field(Title = "FA Icon (optional)", Placeholder = "fas fa-users")]
    public StringField Stat1Icon { get; set; } = new();

    [Header("Statistic 2")]
    [Field(Title = "Number / Value")]
    public StringField Stat2Number { get; set; } = new();

    [Field(Title = "Label")]
    public StringField Stat2Label { get; set; } = new();

    [Field(Title = "FA Icon (optional)")]
    public StringField Stat2Icon { get; set; } = new();

    [Header("Statistic 3")]
    [Field(Title = "Number / Value")]
    public StringField Stat3Number { get; set; } = new();

    [Field(Title = "Label")]
    public StringField Stat3Label { get; set; } = new();

    [Field(Title = "FA Icon (optional)")]
    public StringField Stat3Icon { get; set; } = new();

    [Header("Statistic 4")]
    [Field(Title = "Number / Value")]
    public StringField Stat4Number { get; set; } = new();

    [Field(Title = "Label")]
    public StringField Stat4Label { get; set; } = new();

    [Field(Title = "FA Icon (optional)")]
    public StringField Stat4Icon { get; set; } = new();

    [Header("Design")]
    [Field(Title = "Number Color")]
    public ColorField NumberColor { get; set; } = new() { Value = "#10b981" };

    [Field(Title = "Label Color")]
    public ColorField LabelColor { get; set; } = new() { Value = "#6b7280" };

    [Field(Title = "Background Color")]
    public ColorField BackgroundColor { get; set; } = new() { Value = "#f9fafb" };

    [Field(Title = "Card Background Color")]
    public ColorField CardBgColor { get; set; } = new() { Value = "#ffffff" };

    [Field(Title = "Vertical Padding", Placeholder = "Ex: 4rem")]
    public StringField PaddingY { get; set; } = new() { Value = "4rem" };
}
