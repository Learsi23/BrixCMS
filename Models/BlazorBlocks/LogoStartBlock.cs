using BrixCMS.Open.Data.Fields;
using BrixCMS.Open.Models.Base;

namespace BrixCMS.Open.Models.BlazorBlocks
{
    [BlockType(Name = "Animated Logo Header", Category = "Headers", Icon = "fas fa-play",
    Description = "Hero header with animated logo and sequential text or image elements that slide in from the right. Includes decorative side images, main description with optional icon, and call-to-action button. Fully responsive with independent sizing per breakpoint."
)]
    public class LogoStartBlock : BlockBase
    {
        [Header("Background")]
        [Field(Title = "Background Width")]
        public SelectField<string> BackgroundWidth { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "full", Label = "Full Width" },
                new() { Value = "contained", Label = "Contained" }
            },
            Value = "full"
        };

        [Field(Title = "Min Height", Placeholder = "Ej: 65vh")]
        public StringField MinHeight { get; set; } = new() { Value = "65vh" };

        [Field(Title = "Background Color")]
        public ColorField BackgroundColor { get; set; } = new() { Value = "#13294B" };

        [Header("Logo")]
        [Field(Title = "Logo Image")]
        public ImageField LogoImage { get; set; } = new();

        [Field(Title = "Logo Width (Desktop)", Placeholder = "Ej: 200px")]
        public StringField LogoWidth { get; set; } = new() { Value = "200px" };
        [Field(Title = "Logo Width (Tablet)", Placeholder = "Ej: 150px")]
        public StringField LogoWidthTablet { get; set; } = new() { Value = "150px" };
        [Field(Title = "Logo Width (Mobile)", Placeholder = "Ej: 120px")]
        public StringField LogoWidthMobile { get; set; } = new() { Value = "120px" };
        [Field(Title = "Logo Height", Placeholder = "Ej: auto")]
        public StringField LogoHeight { get; set; } = new() { Value = "auto" };

        [Header("Animated Elements")]
        [Field(Title = "Element 1 - Type")]
        public SelectField<string> Element1Type { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "text", Label = "Text" },
                new() { Value = "image", Label = "Image" }
            },
            Value = "image"
        };
        [Field(Title = "Element 1 - Text")]
        public StringField Word1 { get; set; } = new();
        [Field(Title = "Element 1 - Color")]
        public ColorField Word1Color { get; set; } = new() { Value = "#ffffff" };
        [Field(Title = "Element 1 - Size", Placeholder = "Ej: 120px")]
        public StringField Word1Size { get; set; } = new() { Value = "120px" };
        [Field(Title = "Element 1 - Size (Tablet)", Placeholder = "Ej: 80px")]
        public StringField Word1SizeTablet { get; set; } = new() { Value = "80px" };
        [Field(Title = "Element 1 - Size (Mobile)", Placeholder = "Ej: 48px")]
        public StringField Word1SizeMobile { get; set; } = new() { Value = "48px" };
        [Field(Title = "Element 1 - Font Weight")]
        public SelectField<string> Word1Weight { get; set; } = new()
        {
            Options = GetWeightOptions(),
            Value = "700"
        };
        [Field(Title = "Element 1 - Line Break After")]
        public BoolField LineBreak1 { get; set; } = new() { Value = "false" };
        [Field(Title = "Element 1 - Image")]
        public ImageField Image1 { get; set; } = new();
        [Field(Title = "Element 1 - Image Width (Desktop)", Placeholder = "Ej: 150px")]
        public StringField Image1Width { get; set; } = new() { Value = "150px" };
        [Field(Title = "Element 1 - Image Width (Tablet)", Placeholder = "Ej: 100px")]
        public StringField Image1WidthTablet { get; set; } = new() { Value = "100px" };
        [Field(Title = "Element 1 - Image Width (Mobile)", Placeholder = "Ej: 60px")]
        public StringField Image1WidthMobile { get; set; } = new() { Value = "60px" };
        [Field(Title = "Element 1 - Image Height", Placeholder = "Ej: auto")]
        public StringField Image1Height { get; set; } = new() { Value = "auto" };

        [Field(Title = "Element 2 - Type")]
        public SelectField<string> Element2Type { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "text", Label = "Text" },
                new() { Value = "image", Label = "Image" }
            },
            Value = "image"
        };
        [Field(Title = "Element 2 - Text")]
        public StringField Word2 { get; set; } = new();
        [Field(Title = "Element 2 - Color")]
        public ColorField Word2Color { get; set; } = new() { Value = "#ffffff" };
        [Field(Title = "Element 2 - Size", Placeholder = "Ej: 120px")]
        public StringField Word2Size { get; set; } = new() { Value = "120px" };
        [Field(Title = "Element 2 - Size (Tablet)", Placeholder = "Ej: 80px")]
        public StringField Word2SizeTablet { get; set; } = new() { Value = "80px" };
        [Field(Title = "Element 2 - Size (Mobile)", Placeholder = "Ej: 48px")]
        public StringField Word2SizeMobile { get; set; } = new() { Value = "48px" };
        [Field(Title = "Element 2 - Font Weight")]
        public SelectField<string> Word2Weight { get; set; } = new() { Options = GetWeightOptions(), Value = "700" };
        [Field(Title = "Element 2 - Line Break After")]
        public BoolField LineBreak2 { get; set; } = new() { Value = "false" };
        [Field(Title = "Element 2 - Image")]
        public ImageField Image2 { get; set; } = new();
        [Field(Title = "Element 2 - Image Width (Desktop)", Placeholder = "Ej: 150px")]
        public StringField Image2Width { get; set; } = new() { Value = "150px" };
        [Field(Title = "Element 2 - Image Width (Tablet)", Placeholder = "Ej: 100px")]
        public StringField Image2WidthTablet { get; set; } = new() { Value = "100px" };
        [Field(Title = "Element 2 - Image Width (Mobile)", Placeholder = "Ej: 60px")]
        public StringField Image2WidthMobile { get; set; } = new() { Value = "60px" };
        [Field(Title = "Element 2 - Image Height", Placeholder = "Ej: auto")]
        public StringField Image2Height { get; set; } = new() { Value = "auto" };

        [Field(Title = "Element 3 - Type")]
        public SelectField<string> Element3Type { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "text", Label = "Text" },
                new() { Value = "image", Label = "Image" }
            },
            Value = "image"
        };
        [Field(Title = "Element 3 - Text")]
        public StringField Word3 { get; set; } = new();
        [Field(Title = "Element 3 - Color")]
        public ColorField Word3Color { get; set; } = new() { Value = "#ffffff" };
        [Field(Title = "Element 3 - Size", Placeholder = "Ej: 120px")]
        public StringField Word3Size { get; set; } = new() { Value = "120px" };
        [Field(Title = "Element 3 - Size (Tablet)", Placeholder = "Ej: 80px")]
        public StringField Word3SizeTablet { get; set; } = new() { Value = "80px" };
        [Field(Title = "Element 3 - Size (Mobile)", Placeholder = "Ej: 48px")]
        public StringField Word3SizeMobile { get; set; } = new() { Value = "48px" };
        [Field(Title = "Element 3 - Font Weight")]
        public SelectField<string> Word3Weight { get; set; } = new() { Options = GetWeightOptions(), Value = "700" };
        [Field(Title = "Element 3 - Line Break After")]
        public BoolField LineBreak3 { get; set; } = new() { Value = "false" };
        [Field(Title = "Element 3 - Image")]
        public ImageField Image3 { get; set; } = new();
        [Field(Title = "Element 3 - Image Width (Desktop)", Placeholder = "Ej: 150px")]
        public StringField Image3Width { get; set; } = new() { Value = "150px" };
        [Field(Title = "Element 3 - Image Width (Tablet)", Placeholder = "Ej: 100px")]
        public StringField Image3WidthTablet { get; set; } = new() { Value = "100px" };
        [Field(Title = "Element 3 - Image Width (Mobile)", Placeholder = "Ej: 60px")]
        public StringField Image3WidthMobile { get; set; } = new() { Value = "60px" };
        [Field(Title = "Element 3 - Image Height", Placeholder = "Ej: auto")]
        public StringField Image3Height { get; set; } = new() { Value = "auto" };

        [Field(Title = "Element 4 - Type")]
        public SelectField<string> Element4Type { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "text", Label = "Text" },
                new() { Value = "image", Label = "Image" }
            },
            Value = "image"
        };
        [Field(Title = "Element 4 - Text")]
        public StringField Word4 { get; set; } = new();
        [Field(Title = "Element 4 - Color")]
        public ColorField Word4Color { get; set; } = new() { Value = "#ffffff" };
        [Field(Title = "Element 4 - Size", Placeholder = "Ej: 120px")]
        public StringField Word4Size { get; set; } = new() { Value = "120px" };
        [Field(Title = "Element 4 - Size (Tablet)", Placeholder = "Ej: 80px")]
        public StringField Word4SizeTablet { get; set; } = new() { Value = "80px" };
        [Field(Title = "Element 4 - Size (Mobile)", Placeholder = "Ej: 48px")]
        public StringField Word4SizeMobile { get; set; } = new() { Value = "48px" };
        [Field(Title = "Element 4 - Font Weight")]
        public SelectField<string> Word4Weight { get; set; } = new() { Options = GetWeightOptions(), Value = "700" };
        [Field(Title = "Element 4 - Line Break After")]
        public BoolField LineBreak4 { get; set; } = new() { Value = "false" };
        [Field(Title = "Element 4 - Image")]
        public ImageField Image4 { get; set; } = new();
        [Field(Title = "Element 4 - Image Width (Desktop)", Placeholder = "Ej: 150px")]
        public StringField Image4Width { get; set; } = new() { Value = "150px" };
        [Field(Title = "Element 4 - Image Width (Tablet)", Placeholder = "Ej: 100px")]
        public StringField Image4WidthTablet { get; set; } = new() { Value = "100px" };
        [Field(Title = "Element 4 - Image Width (Mobile)", Placeholder = "Ej: 60px")]
        public StringField Image4WidthMobile { get; set; } = new() { Value = "60px" };
        [Field(Title = "Element 4 - Image Height", Placeholder = "Ej: auto")]
        public StringField Image4Height { get; set; } = new() { Value = "auto" };

        [Field(Title = "Element 5 - Type")]
        public SelectField<string> Element5Type { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "text", Label = "Text" },
                new() { Value = "image", Label = "Image" }
            },
            Value = "image"
        };
        [Field(Title = "Element 5 - Text")]
        public StringField Word5 { get; set; } = new();
        [Field(Title = "Element 5 - Color")]
        public ColorField Word5Color { get; set; } = new() { Value = "#ffffff" };
        [Field(Title = "Element 5 - Size", Placeholder = "Ej: 120px")]
        public StringField Word5Size { get; set; } = new() { Value = "120px" };
        [Field(Title = "Element 5 - Size (Tablet)", Placeholder = "Ej: 80px")]
        public StringField Word5SizeTablet { get; set; } = new() { Value = "80px" };
        [Field(Title = "Element 5 - Size (Mobile)", Placeholder = "Ej: 48px")]
        public StringField Word5SizeMobile { get; set; } = new() { Value = "48px" };
        [Field(Title = "Element 5 - Font Weight")]
        public SelectField<string> Word5Weight { get; set; } = new() { Options = GetWeightOptions(), Value = "700" };
        [Field(Title = "Element 5 - Line Break After")]
        public BoolField LineBreak5 { get; set; } = new() { Value = "false" };
        [Field(Title = "Element 5 - Image")]
        public ImageField Image5 { get; set; } = new();
        [Field(Title = "Element 5 - Image Width (Desktop)", Placeholder = "Ej: 150px")]
        public StringField Image5Width { get; set; } = new() { Value = "150px" };
        [Field(Title = "Element 5 - Image Width (Tablet)", Placeholder = "Ej: 100px")]
        public StringField Image5WidthTablet { get; set; } = new() { Value = "100px" };
        [Field(Title = "Element 5 - Image Width (Mobile)", Placeholder = "Ej: 60px")]
        public StringField Image5WidthMobile { get; set; } = new() { Value = "60px" };
        [Field(Title = "Element 5 - Image Height", Placeholder = "Ej: auto")]
        public StringField Image5Height { get; set; } = new() { Value = "auto" };

        [Field(Title = "Element 6 - Type")]
        public SelectField<string> Element6Type { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "text", Label = "Text" },
                new() { Value = "image", Label = "Image" }
            },
            Value = "image"
        };
        [Field(Title = "Element 6 - Text")]
        public StringField Word6 { get; set; } = new();
        [Field(Title = "Element 6 - Color")]
        public ColorField Word6Color { get; set; } = new() { Value = "#ffffff" };
        [Field(Title = "Element 6 - Size", Placeholder = "Ej: 120px")]
        public StringField Word6Size { get; set; } = new() { Value = "120px" };
        [Field(Title = "Element 6 - Size (Tablet)", Placeholder = "Ej: 80px")]
        public StringField Word6SizeTablet { get; set; } = new() { Value = "80px" };
        [Field(Title = "Element 6 - Size (Mobile)", Placeholder = "Ej: 48px")]
        public StringField Word6SizeMobile { get; set; } = new() { Value = "48px" };
        [Field(Title = "Element 6 - Font Weight")]
        public SelectField<string> Word6Weight { get; set; } = new() { Options = GetWeightOptions(), Value = "700" };
        [Field(Title = "Element 6 - Line Break After")]
        public BoolField LineBreak6 { get; set; } = new() { Value = "false" };
        [Field(Title = "Element 6 - Image")]
        public ImageField Image6 { get; set; } = new();
        [Field(Title = "Element 6 - Image Width (Desktop)", Placeholder = "Ej: 150px")]
        public StringField Image6Width { get; set; } = new() { Value = "150px" };
        [Field(Title = "Element 6 - Image Width (Tablet)", Placeholder = "Ej: 100px")]
        public StringField Image6WidthTablet { get; set; } = new() { Value = "100px" };
        [Field(Title = "Element 6 - Image Width (Mobile)", Placeholder = "Ej: 60px")]
        public StringField Image6WidthMobile { get; set; } = new() { Value = "60px" };
        [Field(Title = "Element 6 - Image Height", Placeholder = "Ej: auto")]
        public StringField Image6Height { get; set; } = new() { Value = "auto" };

        [Field(Title = "Element 7 - Type")]
        public SelectField<string> Element7Type { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "text", Label = "Text" },
                new() { Value = "image", Label = "Image" }
            },
            Value = "image"
        };
        [Field(Title = "Element 7 - Text")]
        public StringField Word7 { get; set; } = new();
        [Field(Title = "Element 7 - Color")]
        public ColorField Word7Color { get; set; } = new() { Value = "#ffffff" };
        [Field(Title = "Element 7 - Size", Placeholder = "Ej: 120px")]
        public StringField Word7Size { get; set; } = new() { Value = "120px" };
        [Field(Title = "Element 7 - Size (Tablet)", Placeholder = "Ej: 80px")]
        public StringField Word7SizeTablet { get; set; } = new() { Value = "80px" };
        [Field(Title = "Element 7 - Size (Mobile)", Placeholder = "Ej: 48px")]
        public StringField Word7SizeMobile { get; set; } = new() { Value = "48px" };
        [Field(Title = "Element 7 - Font Weight")]
        public SelectField<string> Word7Weight { get; set; } = new() { Options = GetWeightOptions(), Value = "700" };
        [Field(Title = "Element 7 - Line Break After")]
        public BoolField LineBreak7 { get; set; } = new() { Value = "false" };
        [Field(Title = "Element 7 - Image")]
        public ImageField Image7 { get; set; } = new();
        [Field(Title = "Element 7 - Image Width (Desktop)", Placeholder = "Ej: 150px")]
        public StringField Image7Width { get; set; } = new() { Value = "150px" };
        [Field(Title = "Element 7 - Image Width (Tablet)", Placeholder = "Ej: 100px")]
        public StringField Image7WidthTablet { get; set; } = new() { Value = "100px" };
        [Field(Title = "Element 7 - Image Width (Mobile)", Placeholder = "Ej: 60px")]
        public StringField Image7WidthMobile { get; set; } = new() { Value = "60px" };
        [Field(Title = "Element 7 - Image Height", Placeholder = "Ej: auto")]
        public StringField Image7Height { get; set; } = new() { Value = "auto" };

        [Field(Title = "Element 8 - Type")]
        public SelectField<string> Element8Type { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "text", Label = "Text" },
                new() { Value = "image", Label = "Image" }
            },
            Value = "text"
        };
        [Field(Title = "Element 8 - Text")]
        public StringField Word8 { get; set; } = new();
        [Field(Title = "Element 8 - Color")]
        public ColorField Word8Color { get; set; } = new() { Value = "#ffffff" };
        [Field(Title = "Element 8 - Size (Desktop)", Placeholder = "Ej: 48px")]
        public StringField Word8Size { get; set; } = new() { Value = "48px" };
        [Field(Title = "Element 8 - Size (Tablet)", Placeholder = "Ej: 36px")]
        public StringField Word8SizeTablet { get; set; } = new() { Value = "36px" };
        [Field(Title = "Element 8 - Size (Mobile)", Placeholder = "Ej: 24px")]
        public StringField Word8SizeMobile { get; set; } = new() { Value = "24px" };
        [Field(Title = "Element 8 - Font Weight")]
        public SelectField<string> Word8Weight { get; set; } = new() { Options = GetWeightOptions(), Value = "700" };
        [Field(Title = "Element 8 - Line Break After")]
        public BoolField LineBreak8 { get; set; } = new() { Value = "false" };
        [Field(Title = "Element 8 - Image")]
        public ImageField Image8 { get; set; } = new();
        [Field(Title = "Element 8 - Image Width (Desktop)", Placeholder = "Ej: 150px")]
        public StringField Image8Width { get; set; } = new() { Value = "150px" };
        [Field(Title = "Element 8 - Image Width (Tablet)", Placeholder = "Ej: 100px")]
        public StringField Image8WidthTablet { get; set; } = new() { Value = "100px" };
        [Field(Title = "Element 8 - Image Width (Mobile)", Placeholder = "Ej: 60px")]
        public StringField Image8WidthMobile { get; set; } = new() { Value = "60px" };
        [Field(Title = "Element 8 - Image Height", Placeholder = "Ej: auto")]
        public StringField Image8Height { get; set; } = new() { Value = "auto" };

        [Field(Title = "Element 9 - Type")]
        public SelectField<string> Element9Type { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "text", Label = "Text" },
                new() { Value = "image", Label = "Image" }
            },
            Value = "text"
        };
        [Field(Title = "Element 9 - Text")]
        public StringField Word9 { get; set; } = new();
        [Field(Title = "Element 9 - Color")]
        public ColorField Word9Color { get; set; } = new() { Value = "#ffffff" };
        [Field(Title = "Element 9 - Size (Desktop)", Placeholder = "Ej: 48px")]
        public StringField Word9Size { get; set; } = new() { Value = "48px" };
        [Field(Title = "Element 9 - Size (Tablet)", Placeholder = "Ej: 36px")]
        public StringField Word9SizeTablet { get; set; } = new() { Value = "36px" };
        [Field(Title = "Element 9 - Size (Mobile)", Placeholder = "Ej: 24px")]
        public StringField Word9SizeMobile { get; set; } = new() { Value = "24px" };
        [Field(Title = "Element 9 - Font Weight")]
        public SelectField<string> Word9Weight { get; set; } = new() { Options = GetWeightOptions(), Value = "700" };
        [Field(Title = "Element 9 - Line Break After")]
        public BoolField LineBreak9 { get; set; } = new() { Value = "false" };
        [Field(Title = "Element 9 - Image")]
        public ImageField Image9 { get; set; } = new();
        [Field(Title = "Element 9 - Image Width (Desktop)", Placeholder = "Ej: 150px")]
        public StringField Image9Width { get; set; } = new() { Value = "150px" };
        [Field(Title = "Element 9 - Image Width (Tablet)", Placeholder = "Ej: 100px")]
        public StringField Image9WidthTablet { get; set; } = new() { Value = "100px" };
        [Field(Title = "Element 9 - Image Width (Mobile)", Placeholder = "Ej: 60px")]
        public StringField Image9WidthMobile { get; set; } = new() { Value = "60px" };
        [Field(Title = "Element 9 - Image Height", Placeholder = "Ej: auto")]
        public StringField Image9Height { get; set; } = new() { Value = "auto" };

        [Field(Title = "Element 10 - Type")]
        public SelectField<string> Element10Type { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "text", Label = "Text" },
                new() { Value = "image", Label = "Image" }
            },
            Value = "text"
        };
        [Field(Title = "Element 10 - Text")]
        public StringField Word10 { get; set; } = new();
        [Field(Title = "Element 10 - Color")]
        public ColorField Word10Color { get; set; } = new() { Value = "#ffffff" };
        [Field(Title = "Element 10 - Size (Desktop)", Placeholder = "Ej: 48px")]
        public StringField Word10Size { get; set; } = new() { Value = "48px" };
        [Field(Title = "Element 10 - Size (Tablet)", Placeholder = "Ej: 36px")]
        public StringField Word10SizeTablet { get; set; } = new() { Value = "36px" };
        [Field(Title = "Element 10 - Size (Mobile)", Placeholder = "Ej: 24px")]
        public StringField Word10SizeMobile { get; set; } = new() { Value = "24px" };
        [Field(Title = "Element 10 - Font Weight")]
        public SelectField<string> Word10Weight { get; set; } = new() { Options = GetWeightOptions(), Value = "700" };
        [Field(Title = "Element 10 - Line Break After")]
        public BoolField LineBreak10 { get; set; } = new() { Value = "false" };
        [Field(Title = "Element 10 - Image")]
        public ImageField Image10 { get; set; } = new();
        [Field(Title = "Element 10 - Image Width (Desktop)", Placeholder = "Ej: 150px")]
        public StringField Image10Width { get; set; } = new() { Value = "150px" };
        [Field(Title = "Element 10 - Image Width (Tablet)", Placeholder = "Ej: 100px")]
        public StringField Image10WidthTablet { get; set; } = new() { Value = "100px" };
        [Field(Title = "Element 10 - Image Width (Mobile)", Placeholder = "Ej: 60px")]
        public StringField Image10WidthMobile { get; set; } = new() { Value = "60px" };
        [Field(Title = "Element 10 - Image Height", Placeholder = "Ej: auto")]
        public StringField Image10Height { get; set; } = new() { Value = "auto" };

        [Field(Title = "Main Text (Description)")]
        public StringField MainText { get; set; } = new();
        [Field(Title = "Main Text Color")]
        public ColorField MainTextColor { get; set; } = new() { Value = "#ffffff" };
        [Field(Title = "Main Text Size (Desktop)", Placeholder = "Ej: 32px")]
        public StringField MainTextSize { get; set; } = new() { Value = "32px" };
        [Field(Title = "Main Text Size (Tablet)", Placeholder = "Ej: 24px")]
        public StringField MainTextSizeTablet { get; set; } = new() { Value = "24px" };
        [Field(Title = "Main Text Size (Mobile)", Placeholder = "Ej: 18px")]
        public StringField MainTextSizeMobile { get; set; } = new() { Value = "18px" };
        [Field(Title = "Main Text Alignment")]
        public SelectField<string> MainTextAlignment { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "left", Label = "Left" },
                new() { Value = "center", Label = "Center" },
                new() { Value = "right", Label = "Right" }
            },
            Value = "left"
        };

        [Header("Main Text Icon")]
        [Field(Title = "Icon Type")]
        public SelectField<string> MainTextIconType { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "none", Label = "None" },
                new() { Value = "image", Label = "Image" },
                new() { Value = "fontawesome", Label = "FontAwesome" }
            },
            Value = "none"
        };
        [Field(Title = "Icon Image")]
        public ImageField MainTextIconImage { get; set; } = new();
        [Field(Title = "Icon Image Width (Desktop)", Placeholder = "Ej: 32px")]
        public StringField MainTextIconImageWidth { get; set; } = new() { Value = "32px" };
        [Field(Title = "Icon Image Width (Tablet)", Placeholder = "Ej: 24px")]
        public StringField MainTextIconImageWidthTablet { get; set; } = new() { Value = "24px" };
        [Field(Title = "Icon Image Width (Mobile)", Placeholder = "Ej: 20px")]
        public StringField MainTextIconImageWidthMobile { get; set; } = new() { Value = "20px" };
        [Field(Title = "Icon Image Height", Placeholder = "Ej: auto")]
        public StringField MainTextIconImageHeight { get; set; } = new() { Value = "auto" };
        [Field(Title = "FontAwesome Class", Placeholder = "Ej: fas fa-arrow-down")]
        public StringField MainTextIconClass { get; set; } = new();
        [Field(Title = "Icon Color")]
        public ColorField MainTextIconColor { get; set; } = new() { Value = "#ffffff" };
        [Field(Title = "Icon Size (Desktop)", Placeholder = "Ej: 24px")]
        public StringField MainTextIconSize { get; set; } = new() { Value = "24px" };
        [Field(Title = "Icon Size (Tablet)", Placeholder = "Ej: 20px")]
        public StringField MainTextIconSizeTablet { get; set; } = new() { Value = "20px" };
        [Field(Title = "Icon Size (Mobile)", Placeholder = "Ej: 16px")]
        public StringField MainTextIconSizeMobile { get; set; } = new() { Value = "16px" };

        [Header("Decorative Images")]
        [Field(Title = "Left Decorative Image")]
        public ImageField LeftImage { get; set; } = new();
        [Field(Title = "Left Image Width (Desktop)", Placeholder = "Ej: 150px")]
        public StringField LeftImageWidth { get; set; } = new() { Value = "150px" };
        [Field(Title = "Left Image Width (Tablet)", Placeholder = "Ej: 100px")]
        public StringField LeftImageWidthTablet { get; set; } = new() { Value = "100px" };
        [Field(Title = "Left Image Width (Mobile)", Placeholder = "Ej: 60px")]
        public StringField LeftImageWidthMobile { get; set; } = new() { Value = "60px" };
        [Field(Title = "Left Image Height", Placeholder = "Ej: auto")]
        public StringField LeftImageHeight { get; set; } = new() { Value = "auto" };
        [Field(Title = "Left Image Vertical Position")]
        public SelectField<string> LeftImageVertical { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "top", Label = "Top" },
                new() { Value = "center", Label = "Center" },
                new() { Value = "bottom", Label = "Bottom" }
            },
            Value = "center"
        };
        [Field(Title = "Left Image Left Offset", Placeholder = "Ej: 0px")]
        public StringField LeftImageLeft { get; set; } = new() { Value = "0px" };

        [Field(Title = "Right Decorative Image")]
        public ImageField RightImage { get; set; } = new();
        [Field(Title = "Right Image Width (Desktop)", Placeholder = "Ej: 150px")]
        public StringField RightImageWidth { get; set; } = new() { Value = "150px" };
        [Field(Title = "Right Image Width (Tablet)", Placeholder = "Ej: 100px")]
        public StringField RightImageWidthTablet { get; set; } = new() { Value = "100px" };
        [Field(Title = "Right Image Width (Mobile)", Placeholder = "Ej: 60px")]
        public StringField RightImageWidthMobile { get; set; } = new() { Value = "60px" };
        [Field(Title = "Right Image Height", Placeholder = "Ej: auto")]
        public StringField RightImageHeight { get; set; } = new() { Value = "auto" };
        [Field(Title = "Right Image Vertical Position")]
        public SelectField<string> RightImageVertical { get; set; } = new()
        {
            Options = new List<SelectOption<string>>
            {
                new() { Value = "top", Label = "Top" },
                new() { Value = "center", Label = "Center" },
                new() { Value = "bottom", Label = "Bottom" }
            },
            Value = "center"
        };
        [Field(Title = "Right Image Right Offset", Placeholder = "Ej: 0px")]
        public StringField RightImageRight { get; set; } = new() { Value = "0px" };

        [Header("Button")]
        [Field(Title = "Button Text")]
        public StringField ButtonText { get; set; } = new();
        [Field(Title = "Button Link")]
        public StringField ButtonUrl { get; set; } = new();
        [Field(Title = "Button Color")]
        public ColorField ButtonColor { get; set; } = new() { Value = "#007bff" };
        [Field(Title = "Button Text Color")]
        public ColorField ButtonTextColor { get; set; } = new() { Value = "#ffffff" };
        [Field(Title = "Button Border Radius", Placeholder = "Ej: 8px")]
        public StringField ButtonBorderRadius { get; set; } = new() { Value = "8px" };
        [Field(Title = "Button Padding", Placeholder = "Ej: 12px 24px")]
        public StringField ButtonPadding { get; set; } = new() { Value = "12px 24px" };

        private static List<SelectOption<string>> GetWeightOptions() => new()
        {
            new() { Value = "100", Label = "Thin" },
            new() { Value = "300", Label = "Light" },
            new() { Value = "400", Label = "Normal" },
            new() { Value = "600", Label = "Semibold" },
            new() { Value = "700", Label = "Bold" },
            new() { Value = "900", Label = "Black" }
        };
    }
}
