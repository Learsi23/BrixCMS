// ====================================================================
// FIELDS.CS - DYNAMIC BLOCK FIELDS FOR brix CMS
// ====================================================================
// This file defines the different types of fields that can be used
// in CMS blocks. Each field implements the IField interface
// and provides implicit conversions for easier use.
// ====================================================================

using BrixCMS.Open.Models.Base;

namespace BrixCMS.Open.Data.Fields
{
    // ====================================================================
    // ATTRIBUTES TO DECORATE BLOCKS AND FIELDS
    // ====================================================================

    [AttributeUsage(AttributeTargets.Property)]
    public class HeaderAttribute : Attribute
    {
        public string Title { get; set; } = "";
        /// <summary>When true, this section is hidden in the editor's "Simple" mode and only
        /// shown in "Advanced" mode — keeps a powerful block approachable without losing controls.</summary>
        public bool Advanced { get; set; } = false;
        public HeaderAttribute(string title) => Title = title;
    }

    /// <summary>
    /// Attribute to decorate classes representing blocks in the CMS
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class BlockTypeAttribute : Attribute
    {
        /// <summary>Visible name of the block in the CMS</summary>
        public string Name { get; set; } = "";

        /// <summary>CSS Icon (FontAwesome) for the block</summary>
        public string Icon { get; set; } = "";

        /// <summary>Block category for organization in the CMS</summary>
        public string Category { get; set; } = "";

        /// <summary>Short description shown on hover in the block library</summary>
        public string Description { get; set; } = "";

        /// <summary>True for item/child blocks that only make sense inside a parent
        /// (e.g. Accordion Item, Tab Item, Team Member). Hidden from the top-level Block
        /// Library, but still offered when adding a block inside a parent group.</summary>
        public bool Child { get; set; } = false;
    }

    /// <summary>
    /// Attribute to decorate properties representing fields in the blocks
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttribute : Attribute
    {
        /// <summary>Visible title of the field in the CMS</summary>
        public string Title { get; set; } = "";

        /// <summary>Placeholder text for the field</summary>
        public string Placeholder { get; set; } = "";

        /// <summary>Additional description for the field</summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// Conditional visibility rule. Format: "FieldName:value"
        /// Example: "DisplayStyle:carousel" ? field is visible only when DisplayStyle equals "carousel"
        /// Supports multiple values with comma: "DisplayStyle:carousel,grid"
        /// </summary>
        public string VisibleWhen { get; set; } = "";
    }

    // ====================================================================
    // BASIC FIELDS
    // ====================================================================

    /// <summary>
    /// Field for colors with a color picker
    /// </summary>
    public class ColorField : IField
    {
        public string Value { get; set; } = "transparent";

        // Method to convert HEX to RGBA to avoid CSS errors
        public string ToRgba(string opacity)
        {
            if (string.IsNullOrEmpty(Value) || !Value.StartsWith("#")) return "rgba(0,0,0,0)";
            try
            {
                var hex = Value.Replace("#", "");
                if (hex.Length == 3) hex = $"{hex[0]}{hex[0]}{hex[1]}{hex[1]}{hex[2]}{hex[2]}";
                int r = Convert.ToInt32(hex.Substring(0, 2), 16);
                int g = Convert.ToInt32(hex.Substring(2, 2), 16);
                int b = Convert.ToInt32(hex.Substring(4, 2), 16);
                return $"rgba({r}, {g}, {b}, {opacity})";
            }
            catch { return "rgba(0,0,0,0)"; }
        }

        public static implicit operator ColorField(string str) => new ColorField { Value = str };
        public static implicit operator string(ColorField field) => field?.Value ?? "transparent";
    }

    /// <summary>
    /// Simple single-line text field
    /// </summary>
    public class StringField : IField
    {
        /// <summary>Text value</summary>
        public string Value { get; set; } = "";

        public static implicit operator StringField(string str) => new StringField { Value = str };
        public static implicit operator string(StringField field) => field?.Value ?? "";

        public override string ToString() => Value;
    }

    /// <summary>
    /// Field for multi-line text area
    /// </summary>
    public class TextAreaField : IField
    {
        /// <summary>Text value</summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>Number of visible rows</summary>
        public int Rows { get; set; } = 4;

        /// <summary>Maximum text length</summary>
        public int MaxLength { get; set; } = 2000;

        /// <summary>
        /// Gets the text value with new lines converted to HTML line breaks (&lt;br/&gt;)
        /// This allows the text to maintain formatting when rendered in HTML
        /// </summary>
        public string HtmlValue => !string.IsNullOrEmpty(Value)
            ? Value.Replace("\r\n", "<br/>").Replace("\n", "<br/>")
            : string.Empty;

        public static implicit operator TextAreaField(string str) => new TextAreaField { Value = str };
        public static implicit operator string(TextAreaField field) => field?.Value ?? string.Empty;

        public override string ToString() => Value;
    }

    /// <summary>
    /// Field for Markdown content
    /// </summary>
    public class MarkdownField : IField
    {
        /// <summary>Value in Markdown format</summary>
        public string Value { get; set; } = "";

        public static implicit operator MarkdownField(string str) => new MarkdownField { Value = str };
        public static implicit operator string(MarkdownField field) => field?.Value ?? "";

        public override string ToString() => Value;
    }

    // ====================================================================
    // MULTIMEDIA FIELDS
    // ====================================================================

    /// <summary>
    /// Field for images with additional properties
    /// </summary>
    public class ImageField : IField
    {
        /// <summary>Image URL</summary>
        public string Value { get; set; } = "";

        /// <summary>Image title (title attribute)</summary>
        public string Title { get; set; } = "";

        /// <summary>Alternative text (alt attribute)</summary>
        public string AltText { get; set; } = "";

        /// <summary>Image width in pixels as string (e.g. "400")</summary>
        public string Width { get; set; } = "";

        /// <summary>Image height in pixels as string (e.g. "300", empty = auto)</summary>
        public string Height { get; set; } = "";

        /// <summary>CSS object-fit value (cover, contain, fill, none)</summary>
        public string ObjectFit { get; set; } = "cover";

        public static implicit operator string(ImageField field) => field?.Value ?? "";
        public static implicit operator ImageField(string url) => new ImageField { Value = url };

        public override string ToString() => Value;
    }

    // ====================================================================
    // SELECTION FIELDS
    // ====================================================================

    /// <summary>
    /// Option for selection fields
    /// </summary>
    /// <typeparam name="T">Type of the option value</typeparam>
    public class SelectOption<T>
    {
        /// <summary>Internal value of the option</summary>
        public T Value { get; set; } = default!;

        /// <summary>Label visible to the user</summary>
        public string Label { get; set; } = "";

        /// <summary>CSS Icon (FontAwesome) for the option</summary>
        public string Icon { get; set; } = "";
    }

    /// <summary>
    /// Generic selection field with support for different data types
    /// </summary>
    /// <typeparam name="T">Type of the selected value</typeparam>
    public class SelectField<T> : IField
    {
        private T _value = default!;

        /// <summary>Currently selected value</summary>
        public T Value
        {
            get => _value;
            set => _value = value;
        }

        /// <summary>List of available options</summary>
        public List<SelectOption<T>> Options { get; set; } = new();

        // Explicit implementation of IField.Value
        string IField.Value
        {
            get => _value?.ToString() ?? "";
            set
            {
                if (value != null)
                {
                    try
                    {
                        // Try to convert directly to type T
                        _value = (T)Convert.ChangeType(value, typeof(T));
                    }
                    catch
                    {
                        // If it fails, search in the available options
                        var option = Options.Find(o => o.Value?.ToString() == value);
                        if (option != null && option.Value != null)
                            _value = option.Value;
                        else
                            _value = default!;
                    }
                }
            }
        }

        /// <summary>Access to value as string (useful for bindings)</summary>
        public string StringValue
        {
            get => _value?.ToString() ?? "";
            set
            {
                if (value != null)
                {
                    try
                    {
                        _value = (T)Convert.ChangeType(value, typeof(T));
                    }
                    catch
                    {
                        var option = Options.Find(o => o.Value?.ToString() == value);
                        if (option != null && option.Value != null)
                            _value = option.Value;
                        else
                            _value = default!;
                    }
                }
            }
        }

        // Implicit conversions to facilitate use
        public static implicit operator T(SelectField<T> field) => field._value;
        public static implicit operator SelectField<T>(T value) => new SelectField<T> { Value = value };

        public override string ToString() => _value?.ToString() ?? "";
    }

    /// <summary>
    /// Selection field for strings (non-generic version for compatibility)
    /// </summary>
    public class SelectField : IField
    {
        /// <summary>Selected value as string</summary>
        public string Value { get; set; } = "";

        /// <summary>List of available options</summary>
        public List<SelectOption<string>> Options { get; set; } = new();

        public static implicit operator string(SelectField field) => field?.Value ?? "";
    }

    /// <summary>
    /// Marker field for PDF multi-selection.
    /// The value is a comma-separated list of DocumentIds (filenames).
    /// The block editor renders this as a dynamic checkbox list populated from ingested documents.
    /// </summary>
    public class PdfSelectField : IField
    {
        /// <summary>Comma-separated list of selected PDF document IDs</summary>
        public string Value { get; set; } = "";

        public static implicit operator string(PdfSelectField f) => f?.Value ?? "";
        public static implicit operator PdfSelectField(string v) => new() { Value = v };
    }

    // ====================================================================
    // SPECIALIZED FIELDS (BOOLEAN / CHECKBOX)
    // ====================================================================

    /// <summary>
    /// Boolean field for Yes/No, True/False (Checkbox)
    /// </summary>
    public class BoolField : IField
    {
        /// <summary>Value as string ("true"/"false")</summary>
        public string Value { get; set; } = "false";

        /// <summary>Interpreted boolean value</summary>
        public bool IsTrue => !string.IsNullOrWhiteSpace(Value) &&
                             Value.ToLowerInvariant() is "true" or "1" or "yes";

        // Implicit conversions to treat the field as a normal bool in C#
        public static implicit operator bool(BoolField f) => f?.IsTrue ?? false;
        public static implicit operator BoolField(bool v) => new() { Value = v.ToString().ToLowerInvariant() };

        public override string ToString() => IsTrue.ToString().ToLowerInvariant();
    }

    /// <summary>
    /// Alias for BoolField to provide a more intuitive name for Checkboxes
    /// </summary>
    public class CheckBoxField : BoolField { }

    // ====================================================================
    // REMAINING SPECIALIZED FIELDS
    // ====================================================================

    /// <summary>
    /// Numeric field with support for integer and decimal values
    /// </summary>
    public class NumberField : IField
    {
        /// <summary>Value as string</summary>
        public string Value { get; set; } = "0";

        /// <summary>Minimum allowed value</summary>
        public int Min { get; set; } = int.MinValue;

        /// <summary>Maximum allowed value</summary>
        public int Max { get; set; } = int.MaxValue;

        /// <summary>Increment when using arrows</summary>
        public int Step { get; set; } = 1;

        /// <summary>Value as integer</summary>
        public int AsInt => int.TryParse(Value, out var result) ? result : 0;

        /// <summary>Value as double</summary>
        public double AsDouble => double.TryParse(Value, out var result) ? result : 0;

        public static implicit operator NumberField(int value) => new NumberField { Value = value.ToString() };
        public static implicit operator int(NumberField field) => field?.AsInt ?? 0;
        public static implicit operator double(NumberField field) => field?.AsDouble ?? 0;
    }

    /// <summary>
    /// Field for URLs with option to open in a new tab
    /// </summary>
    public class UrlField : IField
    {
        /// <summary>Full URL</summary>
        public string Value { get; set; } = "";

        /// <summary>Indicates if it should open in a new tab</summary>
        public bool OpenInNewTab { get; set; } = true;

        public static implicit operator string(UrlField field) => field?.Value ?? "";
        public static implicit operator UrlField(string url) => new UrlField { Value = url };
    }

    /// <summary>
    /// Field for dates with ISO format (yyyy-MM-dd)
    /// </summary>
    public class DateField : IField
    {
        /// <summary>Date in yyyy-MM-dd format</summary>
        public string Value { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

        /// <summary>Date as DateTime</summary>
        public DateTime AsDateTime => DateTime.TryParse(Value, out var result) ? result : DateTime.Now;

        public static implicit operator DateTime(DateField field) => field?.AsDateTime ?? DateTime.Now;
        public static implicit operator DateField(DateTime date) => new DateField { Value = date.ToString("yyyy-MM-dd") };
    }

    // ====================================================================
    // COLLECTION FIELDS
    // ====================================================================

    /// <summary>
    /// Field for lists of elements (useful for galleries, repeatable items, etc.)
    /// </summary>
    /// <typeparam name="T">Field type contained in the list</typeparam>
    public class ListField<T> : IField where T : IField, new()
    {
        /// <summary>JSON serialized value</summary>
        public string Value { get; set; } = "[]";

        private List<T> _items = new();

        /// <summary>Deserialized list of items</summary>
        public List<T> Items
        {
            get
            {
                if (_items.Count == 0 && !string.IsNullOrEmpty(Value) && Value != "[]")
                {
                    try
                    {
                        _items = System.Text.Json.JsonSerializer.Deserialize<List<T>>(Value) ?? new();
                    }
                    catch
                    {
                        _items = new();
                    }
                }
                return _items;
            }
            set
            {
                _items = value;
                Value = System.Text.Json.JsonSerializer.Serialize(_items);
            }
        }

        /// <summary>Adds an item to the list</summary>
        /// <param name="item">Item to add</param>
        public void Add(T item)
        {
            Items.Add(item);
            Value = System.Text.Json.JsonSerializer.Serialize(Items);
        }

        /// <summary>Removes an item from the list</summary>
        /// <param name="item">Item to remove</param>
        public void Remove(T item)
        {
            Items.Remove(item);
            Value = System.Text.Json.JsonSerializer.Serialize(Items);
        }

        /// <summary>Removes an item by index</summary>
        /// <param name="index">Index of the item to remove</param>
        public void RemoveAt(int index)
        {
            if (index >= 0 && index < Items.Count)
            {
                Items.RemoveAt(index);
                Value = System.Text.Json.JsonSerializer.Serialize(Items);
            }
        }

        /// <summary>Clears all items from the list</summary>
        public void Clear()
        {
            Items.Clear();
            Value = "[]";
        }

        /// <summary>Number of items in the list</summary>
        public int Count => Items.Count;

        public static implicit operator ListField<T>(List<T> list)
        {
            var field = new ListField<T>();
            field.Items = list;
            return field;
        }
    }

    /// <summary>
    /// Selection field preloaded with popular Google Fonts
    /// </summary>
    public class FontSelectField : SelectField<string>
    {
        public FontSelectField()
        {
            Value = "";
            Options = new List<SelectOption<string>>
            {
                new() { Value = "",                 Label = "— Site Font —" },
                new() { Value = "Inter",            Label = "Inter" },
                new() { Value = "Roboto",           Label = "Roboto" },
                new() { Value = "Lato",             Label = "Lato" },
                new() { Value = "Open Sans",        Label = "Open Sans" },
                new() { Value = "Montserrat",       Label = "Montserrat" },
                new() { Value = "Poppins",          Label = "Poppins" },
                new() { Value = "Raleway",          Label = "Raleway" },
                new() { Value = "Nunito",           Label = "Nunito" },
                new() { Value = "Playfair Display", Label = "Playfair Display" },
                new() { Value = "Merriweather",     Label = "Merriweather" },
                new() { Value = "Oswald",           Label = "Oswald" },
                new() { Value = "Source Sans 3",    Label = "Source Sans 3" },
                new() { Value = "PT Sans",          Label = "PT Sans" },
                new() { Value = "Ubuntu",           Label = "Ubuntu" },
                new() { Value = "Quicksand",        Label = "Quicksand" },
                new() { Value = "DM Sans",          Label = "DM Sans" },
                new() { Value = "Josefin Sans",     Label = "Josefin Sans" },
                new() { Value = "Libre Baskerville",Label = "Libre Baskerville" },
                new() { Value = "Space Grotesk",    Label = "Space Grotesk" },
                new() { Value = "Sora",             Label = "Sora" },
            };
        }
    }
}
// ====================================================================
// NOTE: IFIELD INTERFACE
// ====================================================================
// The IField interface is defined in BrixCMS.Web.Models.Base
// and contains only:
//
// public interface IField
// {
//     string Value { get; set; }
// }
//
// All fields implement this interface so the CMS
// can handle them uniformly.
// =========================================================================
