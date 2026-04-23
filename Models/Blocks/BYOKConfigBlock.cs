using BrixCMS.Open.Data.Fields;
using BrixCMS.Open.Models.Base;

namespace BrixCMS.Open.Models.Blocks
{
    [BlockType(Name = "BYOK Config", Category = "Content", Icon = "fas fa-key",
        Description = "Display API key configuration panel with status indicators. Shows OpenAI, Stripe, and other key connections.")]
    public class BYOKConfigBlock : BlockBase
    {
        [Header("Header")]
        [Field(Title = "Section Title")]
        public StringField Title { get; set; } = new();

        [Field(Title = "Title Color")]
        public ColorField TitleColor { get; set; } = new() { Value = "#ffffff" };

        [Field(Title = "Title Size", Placeholder = "E.g.: 2rem")]
        public StringField TitleSize { get; set; } = new() { Value = "2rem" };

        [Field(Title = "Subtitle")]
        public StringField Subtitle { get; set; } = new();

        [Field(Title = "Subtitle Color")]
        public ColorField SubtitleColor { get; set; } = new() { Value = "#9ca3af" };

        [Header("Layout")]
        [Field(Title = "Layout", Placeholder = "left-right (text left, config right)")]
        public StringField Layout { get; set; } = new() { Value = "left-right" };

        [Field(Title = "Background Color")]
        public ColorField BackgroundColor { get; set; } = new();

        [Header("Feature Cards (Left Side)")]
        [Field(Title = "Card 1 - Icon (FA)", Placeholder = "E.g.: fas fa-brain")]
        public StringField Card1Icon { get; set; } = new();

        [Field(Title = "Card 1 - Title")]
        public StringField Card1Title { get; set; } = new();

        [Field(Title = "Card 1 - Description")]
        public StringField Card1Desc { get; set; } = new();

        [Field(Title = "Card 2 - Icon (FA)", Placeholder = "E.g.: fas fa-money-bill-wave")]
        public StringField Card2Icon { get; set; } = new();

        [Field(Title = "Card 2 - Title")]
        public StringField Card2Title { get; set; } = new();

        [Field(Title = "Card 2 - Description")]
        public StringField Card2Desc { get; set; } = new();

        [Header("API Keys (Right Side - Display Only)")]
        [Field(Title = "Show API Keys Section", Description = "Display mock API key configuration")]
        public BoolField ShowApiKeys { get; set; } = new() { Value = "true" };

        [Field(Title = "OpenAI Key Label")]
        public StringField OpenAILabel { get; set; } = new() { Value = "OpenAI Key" };

        [Field(Title = "OpenAI Key Value (masked)", Placeholder = "sk-proj-...")]
        public StringField OpenAIKey { get; set; } = new();

        [Field(Title = "OpenAI Status")]
        public StringField OpenAIStatus { get; set; } = new() { Value = "Active" };

        [Field(Title = "Stripe Key Label")]
        public StringField StripeLabel { get; set; } = new() { Value = "Stripe Key" };

        [Field(Title = "Stripe Key Value (masked)", Placeholder = "sk_live_...")]
        public StringField StripeKey { get; set; } = new();

        [Field(Title = "Stripe Status")]
        public StringField StripeStatus { get; set; } = new() { Value = "Active" };

        [Field(Title = "Gemini Key Label")]
        public StringField GeminiLabel { get; set; } = new() { Value = "Gemini Key" };

        [Field(Title = "Gemini Key Value (masked)", Placeholder = "AIzaSy...")]
        public StringField GeminiKey { get; set; } = new();

        [Field(Title = "Gemini Status")]
        public StringField GeminiStatus { get; set; } = new() { Value = "Active" };

        [Header("Stats Summary")]
        [Field(Title = "Show Sales Stats")]
        public BoolField ShowSalesStats { get; set; } = new() { Value = "true" };

        [Field(Title = "Sales Label", Placeholder = "Monthly Sales")]
        public StringField SalesLabel { get; set; } = new() { Value = "Monthly Sales" };

        [Field(Title = "Sales Value", Placeholder = "42,850 SEK")]
        public StringField SalesValue { get; set; } = new();

        [Field(Title = "Fee Label", Placeholder = "Service Fee (1%)")]
        public StringField FeeLabel { get; set; } = new() { Value = "Service Fee (1%)" };

        [Field(Title = "Fee Value", Placeholder = "428 SEK")]
        public StringField FeeValue { get; set; } = new();
    }
}
