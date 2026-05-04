namespace TestBrixCMS.Models.Pages;
using TestBrixCMS.Data.Fields;
using TestBrixCMS.Data;

public class StandardPage
{
    public string Title { get; set; } = string.Empty;
    public ColorField BackgroundColor { get; set; } = new() { Value = "#ffffff" };
    public StringField FontFamily { get; set; } = new() { Value = "" };

    // Lista de bloques que vienen de la base de datos
    public List<Block> Blocks { get; set; } = new();
}
