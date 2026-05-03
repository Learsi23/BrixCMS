using BrixCMS.Open.Models.BlazorBlocks;
using BrixCMS.Open.Models.Blocks;
using BrixCMS.Open.Models.Blocks.Email;
using BrixCMS.Open.Models.Blocks.Gallery;
using BrixCMS.Open.Models.ColumnBlocks;
using BrixCMS.Open.Services;

namespace BrixCMS.Open.Extensions
{
    public static class BlockRegistrationExtensions
    {
        public static IServiceCollection AddbrixBlocks(this IServiceCollection services)
        {
            var registry = new BlockRegistry();

            // Registro de bloques organizados por subcarpeta

            // layout/
            registry.Register<GridColumn>("layout");
            registry.Register<SpacerBlock>("layout");
            registry.Register<DividerBlock>("layout");
            registry.Register<BannerBlock>("layout");
            registry.Register<FullColumnBlock>("layout");

            // New blocks
            registry.Register<FAQBlock>("interactive");
            registry.Register<BeforeAfterBlock>("media");
            registry.Register<AudioBlock>("media");
            registry.Register<TableBlock>("content");
            registry.Register<FeatureListBlock>("content");
            registry.Register<CookieBannerBlock>("interactive");
            registry.Register<NewsletterBlock>("interactive");
            registry.Register<LottieBlock>("media");
            registry.Register<CodeBlock>("content");
            registry.Register<QRCodeBlock>("media");

            // content/
            registry.Register<HeroBlock>("content");
            registry.Register<HeroFlexibleBlock>("content");
            registry.Register<TextBlock>("content");
            registry.Register<MarkdownBlock>("content");
            registry.Register<ImageBlock>("content");
            registry.Register<CardBlock>("content");
            registry.Register<IconCardBlock>("content");
            registry.Register<StatsBlock>("content");
            registry.Register<CTABannerBlock>("content");
            registry.Register<LogoStripBlock>("content");
            registry.Register<TimelineBlock>("content");
            registry.Register<TimelineItemBlock>("content");
            registry.Register<TestimonialsBlock>("content");
            registry.Register<TestimonialItemBlock>("content");
            registry.Register<SocialProofBlock>("content");
            registry.Register<TeamBlock>("content");
            registry.Register<TeamMemberBlock>("content");
            registry.Register<FeatureGridBlock>("content");
            registry.Register<BrixGridDemoBlock>("content");
            registry.Register<MenuBlock>("content");
            registry.Register<OpeningHoursBlock>("content");

            // media/

            registry.Register<GalleryBlock>("media");
            registry.Register<FlexibleImageTextBlock>("media");
            registry.Register<VideoBlock>("media");
            registry.Register<MapBlock>("media");

            // interactive/
            registry.Register<ButtonLinkBlock>("interactive");
            registry.Register<DropdownBlock>("interactive");
            registry.Register<TextWithButtonBlock>("interactive");
            registry.Register<AccordionBlock>("interactive");
            registry.Register<AccordionItemBlock>("interactive");
            registry.Register<CountdownBlock>("interactive");
            registry.Register<TabsBlock>("interactive");
            registry.Register<TabItemBlock>("interactive");
            registry.Register<EmailButtonBlock>("interactive");

            // blazor/
            registry.Register<ChatBlock>("blazor");
            registry.Register<FloatingChatBlock>("blazor");
            registry.Register<ContactFormBlock>("blazor");
            registry.Register<StartHeroBlock>("blazor");
            registry.Register<LogoStartBlock>("blazor");
            registry.Register<MultiCard_TextButton>("content");

            services.AddSingleton(registry);

            return services;
        }
    }
}
