using System.ComponentModel.DataAnnotations;

namespace BrixCMS.Open.Data;

public class ProductEntity
{
    [Key]
    public Guid ProductId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    public string? Description { get; set; }

    public string? LongDescription { get; set; }

    [Required]
    public decimal Price { get; set; }

    public decimal? OriginalPrice { get; set; }

    public string? CurrencySymbol { get; set; }

    public int Stock { get; set; }

    // Imagen principal (backward compat)
    public string? ImageUrl { get; set; }

    // JSON array de URLs — hasta 6 imágenes: ["url1","url2",...]
    public string? ImagesJson { get; set; }

    public string? Category { get; set; }

    public string? Tags { get; set; }

    // Variantes separadas por coma
    public string? Sizes { get; set; }

    public string? Colors { get; set; }

    public string? CustomOptions { get; set; }

    public string? Badge { get; set; }

    public decimal? Rating { get; set; }

    public int? ReviewCount { get; set; }

    public string? Sku { get; set; }

    public string? StripePriceId { get; set; }
}
