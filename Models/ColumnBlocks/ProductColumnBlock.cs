using BrixCMS.Open.Models.Base;
using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.ColumnBlocks;

[BlockType(Name = "Product Columns", Category = "Layout", Icon = "fas fa-th-list", Description = "Column layout optimized for product listings. Each column renders a product card with cart support.")]
public class ProductColumnBlock : BlockGroupBase
{

    // =============================
    // MAIN TITLE SETTINGS
    // =============================

    [Field(Title = "Block Title", Placeholder = "e.g. Our Products")]
    public StringField Title { get; set; } = new();

    [Field(Title = "Title Color")]
    public ColorField TitleColor { get; set; } = new() { Value = "#1e293b" };

    [Field(Title = "Title Size", Placeholder = "32px")]
    public StringField TitleSize { get; set; } = new() { Value = "32px" };


    // =============================
    // SUBTITLE SETTINGS
    // =============================

    [Field(Title = "Subtítulo")]
    public StringField SubTitle { get; set; } = new();

    [Field(Title = "Color del subtítulo")]
    public ColorField SubTitleColor { get; set; } = new();

    [Field(Title = "Tamaño del subtítulo", Placeholder = "Ej: 1.1rem o 18px")]
    public StringField SubTitleSize { get; set; } = new();


    // =============================
    // DESCRIPTION SETTINGS
    // =============================

    [Field(Title = "Descripción (bajo subtítulo)")]
    public TextAreaField Description { get; set; } = new();

    [Field(Title = "Color descripción")]
    public ColorField DescriptionColor { get; set; } = new();


    // =============================
    // HEADER LAYOUT SETTINGS
    // =============================

    // Controls alignment of Title + Subtitle + Description
    // Allowed values: left | center | right
    [Field(Title = "Header Text Alignment", Placeholder = "left | center | right")]
    public StringField HeaderTextAlign { get; set; } = new() { Value = "left" };


    // Controls max-width of the header container
    // Allowed values: narrow | normal | wide | full
    // Useful for hero-like headers or compact section titles
    [Field(Title = "Header Width", Placeholder = "narrow | normal | wide | full")]
    public StringField HeaderWidth { get; set; } = new() { Value = "normal" };


    // =============================
    // AUTO MODE — FETCH ALL PRODUCTS
    // =============================

    [Field(Title = "🛍️ Traer todos los productos automáticamente",
           Description = "Activo: muestra todos los productos guardados (se puede filtrar por categoría). Inactivo: inserta bloques hijo manualmente.")]
    public BoolField AutoMode { get; set; } = new() { Value = "true" };


    // =============================
    // CATEGORY FILTER SETTINGS
    // =============================

    [Field(Title = "Mostrar filtro de categorías (tabs)",
           Description = "Muestra pestañas 'Todos' + cada categoría de producto encima del grid")]
    public BoolField ShowCategoryFilter { get; set; } = new() { Value = "false" };

    [Field(Title = "Show Category Sidebar")]
    public BoolField ShowSidebar { get; set; } = new() { Value = "true" };


    // =============================
    // GRID SETTINGS
    // =============================

    [Field(Title = "Columns (products per row)", Placeholder = "4")]
    public StringField Columns { get; set; } = new() { Value = "4" };

    [Field(Title = "Products Per Page", Placeholder = "12")]
    public StringField ProductsPerPage { get; set; } = new() { Value = "12" };


    // =============================
    // COLOR SETTINGS
    // =============================

    [Field(Title = "Background Color")]
    public ColorField BackgroundColor { get; set; } = new() { Value = "#f8fafc" };

    [Field(Title = "Card Background Color")]
    public ColorField CardBgColor { get; set; } = new() { Value = "#ffffff" };

    [Field(Title = "Accent Color (buttons, active state)")]
    public ColorField AccentColor { get; set; } = new() { Value = "#2563eb" };


    // =============================
    // SPACING SETTINGS
    // =============================

    [Field(Title = "Padding Top/Bottom", Placeholder = "3rem")]
    public StringField PaddingY { get; set; } = new() { Value = "3rem" };


    // =============================
    // PRODUCT CARD SETTINGS
    // =============================

    [Field(Title = "Show Stock Badge")]
    public BoolField ShowStock { get; set; } = new() { Value = "true" };

    [Field(Title = "Show Rating Stars")]
    public BoolField ShowRating { get; set; } = new() { Value = "false" };

    [Field(Title = "Button Text", Placeholder = "Add to cart")]
    public StringField ButtonText { get; set; } = new() { Value = "Add to cart" };


    // =============================
    // FILTER SETTINGS
    // =============================

    [Field(Title = "Filter by Category (comma-separated, leave empty for all)", Placeholder = "Manga, Novela, Comics")]
    public StringField FilterCategories { get; set; } = new();


    // =============================
    // MANUAL MODE SETTINGS
    // =============================

    // When enabled, block works as container only
    // Products must be inserted manually as child blocks
    [Field(Title = "Force Manual Mode (use as block container only)")]
    public BoolField ForceManual { get; set; } = new() { Value = "false" };


    // =============================
    // CURRENCY
    // =============================

    [Field(Title = "Currency Symbol", Placeholder = "€, kr, $, £")]
    public StringField CurrencySymbol { get; set; } = new() { Value = "kr" };


    // =============================
    // RESTAURANT / ORDER MODE
    // =============================

    [Field(Title = "Modo Restaurante (pedidos de comida)")]
    public BoolField IsRestaurant { get; set; } = new() { Value = "false" };

    [Field(Title = "Nombre del Restaurante", Placeholder = "Ej: Pizzería Roma",
           VisibleWhen = "IsRestaurant:true")]
    public StringField RestaurantName { get; set; } = new();

    [Field(Title = "Mensaje botón de pedido", Placeholder = "Ej: Hacer pedido",
           VisibleWhen = "IsRestaurant:true")]
    public StringField OrderButtonText { get; set; } = new() { Value = "Hacer pedido" };

    // ── Notificaciones de pedido ─────────────────────────────────────────────

    [Field(Title = "🔔 Canal de notificaciones (ntfy)",
           Placeholder = "Ej: latrattoria-pedidos",
           Description = "Cómo funciona: 1) Descarga la app gratuita 'ntfy' en tu móvil (iOS/Android). 2) Suscríbete al canal que escribas aquí. 3) Cada vez que llegue un pedido, recibirás una notificación push automática.",
           VisibleWhen = "IsRestaurant:true")]
    public StringField NtfyTopic { get; set; } = new();

}
