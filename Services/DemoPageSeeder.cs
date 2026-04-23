using BrixCMS.Open.Data;
using System.Text.Json;

namespace BrixCMS.Open.Services;

/// <summary>
/// Inserta una página de muestra completa en la BD si no existe ya.
/// Incluye todos los bloques del CMS con contenido de ejemplo real.
/// </summary>
public static class DemoPageSeeder
{
    public static void SeedIfEmpty(BrixDbContext db)
    {
        if (db.Pages.Any(p => p.Slug == "inicio-demo")) return;

        // ── Página ────────────────────────────────────────────────
        var page = new Page
        {
            Id         = Guid.NewGuid(),
            Title      = "Página Demo",
            Slug       = "inicio-demo",
            IsPublished = true,
            PublishedAt = DateTime.UtcNow,
            SortOrder  = 99,
            JsonData   = JsonSerializer.Serialize(new { BackgroundColor = new { Value = "#ffffff" } })
        };
        db.Pages.Add(page);

        int sort = 0;
        var blocks = new List<Block>();

        // Helper
        Block B(string type, object fields, Guid? parentId = null)
        {
            var b = new Block
            {
                Id       = Guid.NewGuid(),
                PageId   = page.Id,
                Type     = type,
                SortOrder = sort++,
                ParentId = parentId,
                JsonData = JsonSerializer.Serialize(fields)
            };
            blocks.Add(b);
            return b;
        }

        static object F(string value) => new { Value = value };
        static object C(string hex)   => new { Value = hex };

        // ── 1. HERO ───────────────────────────────────────────────
        B("HeroBlock", new
        {
            Title       = F("Bienvenido a BrixCMS"),
            TitleColor  = C("#ffffff"),
            TitleSize   = F("3.5rem"),
            Subtitle    = F("Crea sitios web profesionales sin código.\nRápido, flexible y moderno."),
            SubtitleColor = C("#e5e7eb"),
            SubtitleSize  = F("1.25rem"),
            Description = F(""),
            Background  = F("/images/hero-bg.jpg"),
        });

        // ── 2. LOGOS STRIP ────────────────────────────────────────
        B("LogoStripBlock", new
        {
            Heading      = F("Tecnologías que usamos"),
            HeadingColor = C("#9ca3af"),
            Logo1        = F("/images/logo-tailwind.svg"),
            Logo2        = F("/images/logo-dotnet.svg"),
            Logo3        = F("/images/logo-blazor.svg"),
            Logo4        = F("/images/logo-sqlite.svg"),
            LogoHeight   = F("36px"),
            Grayscale    = F("true"),
            BackgroundColor = C("#f9fafb"),
            PaddingY     = F("2.5rem"),
        });

        // ── 3. SPACER ─────────────────────────────────────────────
        B("SpacerBlock", new { Height = F("56px"), BackgroundColor = C("transparent") });

        // ── 4. GRID SERVICIOS (GridColumn + 3 IconCardBlock hijos) ─
        var grid1 = B("GridColumn", new
        {
            TitleSida      = F("LO QUE HACEMOS"),
            TitleSidaColor = C("#10b981"),
            TitleSidaSize  = F("0.8rem"),
            TitleSidaAlign = F("center"),
            Title          = F("Nuestros Servicios"),
            TitleColor     = C("#111827"),
            TitleSize      = F("2rem"),
            SubTitle       = F("Soluciones completas para tu presencia digital"),
            SubTitleColor  = C("#6b7280"),
            SubTitleSize   = F("1rem"),
            MaxColumns     = F("3"),
            Gap            = F("gap-6"),
            PaddingY       = F("4rem"),
            BackgroundColor = C("#ffffff"),
            HeaderTextAlign = F("center"),
        });

        B("IconCardBlock", new
        {
            LeftIconClass  = F("fas fa-rocket"),
            LeftIconColor  = C("#10b981"),
            LeftIconFaSize = F("2rem"),
            IconPosition   = F("top"),
            TextAlign      = F("center"),
            Title          = F("Diseño Web"),
            TitleColor     = C("#111827"),
            TitleSize      = F("1.2rem"),
            Text           = F("Creamos sitios web modernos, rápidos y optimizados para SEO que convierten visitantes en clientes."),
            TextColor      = C("#6b7280"),
            BackgroundColor = C("#f9fafb"),
            BorderRadius   = F("16px"),
            Padding        = F("2rem"),
            Shadow         = F("0 4px 20px rgba(0,0,0,0.06)"),
        }, grid1.Id);

        B("IconCardBlock", new
        {
            LeftIconClass  = F("fas fa-mobile-alt"),
            LeftIconColor  = C("#6366f1"),
            LeftIconFaSize = F("2rem"),
            IconPosition   = F("top"),
            TextAlign      = F("center"),
            Title          = F("Aplicaciones Móviles"),
            TitleColor     = C("#111827"),
            TitleSize      = F("1.2rem"),
            Text           = F("Desarrollamos apps nativas e híbridas para iOS y Android con experiencias de usuario excepcionales."),
            TextColor      = C("#6b7280"),
            BackgroundColor = C("#f9fafb"),
            BorderRadius   = F("16px"),
            Padding        = F("2rem"),
            Shadow         = F("0 4px 20px rgba(0,0,0,0.06)"),
        }, grid1.Id);

        B("IconCardBlock", new
        {
            LeftIconClass  = F("fas fa-brain"),
            LeftIconColor  = C("#f59e0b"),
            LeftIconFaSize = F("2rem"),
            IconPosition   = F("top"),
            TextAlign      = F("center"),
            Title          = F("Inteligencia Artificial"),
            TitleColor     = C("#111827"),
            TitleSize      = F("1.2rem"),
            Text           = F("Integramos IA en tus procesos: chatbots, análisis de datos, automatización y búsqueda semántica."),
            TextColor      = C("#6b7280"),
            BackgroundColor = C("#f9fafb"),
            BorderRadius   = F("16px"),
            Padding        = F("2rem"),
            Shadow         = F("0 4px 20px rgba(0,0,0,0.06)"),
        }, grid1.Id);

        // ── 5. DIVIDER ────────────────────────────────────────────
        B("DividerBlock", new
        {
            Style     = F("solid"),
            Color     = C("#e5e7eb"),
            Thickness = F("1px"),
            Width     = F("80%"),
            PaddingY  = F("0"),
        });

        // ── 6. STATS ──────────────────────────────────────────────
        B("StatsBlock", new
        {
            Title         = F("Números que hablan"),
            TitleColor    = C("#111827"),
            Subtitle      = F("Resultados reales con clientes reales"),
            SubtitleColor = C("#6b7280"),
            Stat1Number   = F("+500"),
            Stat1Label    = F("Clientes satisfechos"),
            Stat1Icon     = F("fas fa-users"),
            Stat2Number   = F("99%"),
            Stat2Label    = F("Tasa de satisfacción"),
            Stat2Icon     = F("fas fa-star"),
            Stat3Number   = F("12"),
            Stat3Label    = F("Años de experiencia"),
            Stat3Icon     = F("fas fa-award"),
            Stat4Number   = F("+1200"),
            Stat4Label    = F("Proyectos entregados"),
            Stat4Icon     = F("fas fa-check-circle"),
            NumberColor   = C("#10b981"),
            LabelColor    = C("#6b7280"),
            BackgroundColor = C("#f9fafb"),
            CardBgColor   = C("#ffffff"),
            PaddingY      = F("5rem"),
        });

        // ── 7. FLEXIBLE IMAGE + TEXT (About Us) ───────────────────
        B("FlexibleImageTextBlock", new
        {
            Title       = F("Sobre nosotros"),
            TitleColor  = C("#111827"),
            Subtitle    = F("Una agencia apasionada por la tecnología"),
            SubtitleColor = C("#6b7280"),
            Description = F("Somos un equipo multidisciplinar con más de 12 años de experiencia creando soluciones digitales innovadoras. Nos especializamos en desarrollo web, aplicaciones móviles e inteligencia artificial aplicada a negocios reales.\n\nNuestro enfoque es simple: entender tu negocio, proponer la mejor solución y ejecutarla con excelencia."),
            Image       = F("/images/about-us.jpg"),
            ImagePosition = F("right"),
            BackgroundColor = C("#ffffff"),
        });

        // ── 8. VIDEO ──────────────────────────────────────────────
        B("VideoBlock", new
        {
            Title       = F("Mira cómo trabajamos"),
            TitleColor  = C("#111827"),
            Subtitle    = F("Un vistazo a nuestro proceso creativo"),
            SubtitleColor = C("#6b7280"),
            VideoUrl    = F("https://www.youtube.com/watch?v=dQw4w9WgXcQ"),
            AspectRatio = F("16/9"),
            MaxWidth    = F("900px"),
            TextAlign   = F("center"),
            BackgroundColor = C("#f9fafb"),
            PaddingY    = F("4rem"),
        });

        // ── 9. GRID TESTIMONIALS (GridColumn + 3 CardBlock hijos) ─
        var grid2 = B("GridColumn", new
        {
            TitleSida      = F("TESTIMONIOS"),
            TitleSidaColor = C("#10b981"),
            TitleSidaSize  = F("0.8rem"),
            TitleSidaAlign = F("center"),
            Title          = F("Lo que dicen nuestros clientes"),
            TitleColor     = C("#111827"),
            TitleSize      = F("2rem"),
            MaxColumns     = F("3"),
            Gap            = F("gap-6"),
            PaddingY       = F("4rem"),
            BackgroundColor = C("#ffffff"),
            HeaderTextAlign = F("center"),
        });

        B("IconCardBlock", new
        {
            LeftIconClass  = F("fas fa-quote-left"),
            LeftIconColor  = C("#10b981"),
            LeftIconFaSize = F("1.5rem"),
            IconPosition   = F("top"),
            TextAlign      = F("center"),
            Title          = F("María García — CEO, TechStart"),
            TitleColor     = C("#111827"),
            TitleSize      = F("0.95rem"),
            Text           = F("BrixCMS transformó completamente nuestra presencia digital. En 3 semanas teníamos un sitio web profesional que nuestro equipo puede actualizar sin ayuda técnica."),
            TextColor      = C("#6b7280"),
            BackgroundColor = C("#f9fafb"),
            BorderRadius   = F("16px"),
            Padding        = F("2rem"),
        }, grid2.Id);

        B("IconCardBlock", new
        {
            LeftIconClass  = F("fas fa-quote-left"),
            LeftIconColor  = C("#6366f1"),
            LeftIconFaSize = F("1.5rem"),
            IconPosition   = F("top"),
            TextAlign      = F("center"),
            Title          = F("Carlos Ruiz — Director, Innovatech"),
            TitleColor     = C("#111827"),
            TitleSize      = F("0.95rem"),
            Text           = F("La integración de IA en nuestro soporte redujo las consultas repetitivas en un 70%. El ROI fue inmediato y el equipo está encantado con la solución."),
            TextColor      = C("#6b7280"),
            BackgroundColor = C("#f9fafb"),
            BorderRadius   = F("16px"),
            Padding        = F("2rem"),
        }, grid2.Id);

        B("IconCardBlock", new
        {
            LeftIconClass  = F("fas fa-quote-left"),
            LeftIconColor  = C("#f59e0b"),
            LeftIconFaSize = F("1.5rem"),
            IconPosition   = F("top"),
            TextAlign      = F("center"),
            Title          = F("Ana López — Fundadora, Bloom Store"),
            TitleColor     = C("#111827"),
            TitleSize      = F("0.95rem"),
            Text           = F("Nuestra tienda online pasó de cero a 200 pedidos mensuales en 2 meses. El equipo nos acompañó en cada paso con una atención excepcional."),
            TextColor      = C("#6b7280"),
            BackgroundColor = C("#f9fafb"),
            BorderRadius   = F("16px"),
            Padding        = F("2rem"),
        }, grid2.Id);

        // ── 10. CTA BANNER ────────────────────────────────────────
        B("CTABannerBlock", new
        {
            Title           = F("¿Listo para llevar tu negocio al siguiente nivel?"),
            TitleColor      = C("#ffffff"),
            TitleSize       = F("2.25rem"),
            Subtitle        = F("Hablemos de tu proyecto. Primera consulta gratuita, sin compromisos."),
            SubtitleColor   = C("rgba(255,255,255,0.85)"),
            Btn1Text        = F("Solicitar consulta gratuita"),
            Btn1Url         = F("/contacto"),
            Btn1BgColor     = C("#ffffff"),
            Btn1TextColor   = C("#111827"),
            Btn2Text        = F("Ver proyectos"),
            Btn2Url         = F("/proyectos"),
            Btn2Color       = C("#ffffff"),
            BackgroundColor = C("#10b981"),
            BackgroundColor2 = C("#059669"),
            PaddingY        = F("6rem"),
            TextAlign       = F("center"),
        });

        // ── 11. SPACER final ──────────────────────────────────────
        B("SpacerBlock", new { Height = F("40px"), BackgroundColor = C("transparent") });

        db.Blocks.AddRange(blocks);
        db.SaveChanges();
    }
}
