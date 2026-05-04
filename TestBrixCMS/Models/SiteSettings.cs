using TestBrixCMS.Data.Fields;

namespace TestBrixCMS.Models
{
    public class SiteSettings
    {
        public NavbarSettings Navbar { get; set; } = new();
        public FooterSettings Footer { get; set; } = new();
    }

    public class NavbarSettings
    {
        public string BackgroundColor { get; set; } = "#ffffff";
        public string TextColor { get; set; } = "#000000";
        public string Logo { get; set; } = "";
        public string LogoAltText { get; set; } = "Logo";
        public string LogoWidth { get; set; } = "150px";
        public string LogoLink { get; set; } = "/";
        public bool IsSticky { get; set; } = true;
        public bool HasShadow { get; set; } = true;
        public string PaddingVertical { get; set; } = "py-3";
        public List<MenuItemConfig> MenuItems { get; set; } = new();
    }

    public class MenuItemConfig
    {
        public string CustomText { get; set; } = "";
        public string CustomUrl { get; set; } = "";
        public bool IsCustomUrl { get; set; } = false;
        public string PageSlug { get; set; } = "";
    }

    public class FooterSettings
    {
        public string BackgroundColor { get; set; } = "#1a1a1a";
        public string TextColor { get; set; } = "#ffffff";
        public string Logo { get; set; } = "";
        public string LogoAltText { get; set; } = "Logo";
        public string LogoWidth { get; set; } = "150px";
        public string LogoPosition { get; set; } = "left";
        public bool ShowPagesColumn { get; set; } = true;
        public string PagesColumnTitle { get; set; } = "Páginas";
        public List<MenuItemConfig> Pages { get; set; } = new();
        public bool ShowSocialMediaColumn { get; set; } = true;
        public string SocialMediaColumnTitle { get; set; } = "Síguenos";
        public List<SocialMediaConfig> SocialMedia { get; set; } = new();
        public bool ShowCopyrightRow { get; set; } = true;
        public string CompanyName { get; set; } = "";
        public string CompanyNumber { get; set; } = "";
        public string CopyrightText { get; set; } = "Todos los derechos reservados";
        public bool ShowHorizontalLine { get; set; } = true;
        public string PaddingVertical { get; set; } = "py-6";
        public string ColumnsGap { get; set; } = "gap-8";
    }

    public class SocialMediaConfig
    {
        public string Platform { get; set; } = "";
        public string Url { get; set; } = "";
        public string IconType { get; set; } = "class";
        public string IconClass { get; set; } = "";
    }
}
