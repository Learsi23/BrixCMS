# BrixCMS Page Templates

Templates are **real JSON examples** that the AI uses as structural references when generating pages.
Unlike the block documentation in `PromptsService.cs` (which describes fields in text), these files
show the AI *exactly* what valid output looks like — correct field names, correct nesting, correct value format.

---

## How it works

1. At startup, `PromptsService` scans `Templates/pages/*.json`
2. Each file is read and injected into the AI system prompt under **"ADDITIONAL PAGE TEMPLATES"**
3. The AI picks the closest template to the request and adapts it with the user's content and colors
4. More templates = better AI output for that page type

---

## How to add a new template

1. Create a new `.json` file in `Templates/pages/`
2. Name it descriptively: `restaurant.json`, `gym-landing.json`, `saas-pricing.json`
3. Follow the **exact JSON structure** below — field format is always `{ "Value": "string" }`
4. Add a `_meta` object at the top (used for logging, not sent to AI)
5. Restart the app (templates are loaded at startup via `GeneratePageSystemPrompt`)

### Minimal valid template

```json
{
  "_meta": {
    "name": "My Template",
    "description": "One-line description of when to use this",
    "tags": ["landing", "services", "dark"]
  },
  "title": "Page Title Here",
  "slug": "page-slug",
  "blocks": [
    {
      "type": "HeroBlock",
      "data": {
        "Title":         { "Value": "Big Headline" },
        "TitleColor":    { "Value": "#ffffff" },
        "TitleSize":     { "Value": "64px" },
        "Subtitle":      { "Value": "Supporting line" },
        "SubtitleColor": { "Value": "#e2e8f0" },
        "Background":    { "Value": "" }
      }
    },
    {
      "type": "CTABannerBlock",
      "data": {
        "Title":           { "Value": "Ready to start?" },
        "TitleColor":      { "Value": "#ffffff" },
        "Btn1Text":        { "Value": "Get in touch" },
        "Btn1Url":         { "Value": "/contact" },
        "Btn1BgColor":     { "Value": "#10b981" },
        "Btn1TextColor":   { "Value": "#ffffff" },
        "BackgroundColor": { "Value": "#0f172a" },
        "PaddingY":        { "Value": "5rem" },
        "TextAlign":       { "Value": "center" }
      }
    }
  ]
}
```

---

## Rules

| Rule | Example |
|------|---------|
| All values are strings | `"TitleSize": { "Value": "36px" }` not `36` |
| Never use Tailwind class names for sizes | `"56px"` not `"text-5xl"` |
| ColumnBlock / GridColumn must have `"children": [...]` | See `landing-generic.json` |
| Leaf blocks must NOT have `"children"` | HeroBlock, TextBlock, etc. |
| Image fields: leave `""` if no real asset | `"Image": { "Value": "" }` |
| ProductId always `""` | `"ProductId": { "Value": "" }` |
| Slug: lowercase, hyphens, no accents | `"my-services"` not `"My Services"` |
| Navbar and Footer are automatic | Never add NavbarBlock or FooterBlock |

---

## Available block types

**Containers** (must have `children`): `ColumnBlock`, `GridColumn`

**Leaf blocks**: `HeroBlock`, `StartHero`, `TextBlock`, `MarkdownBlock`, `ImageBlock`,
`CardBlock`, `IconCardBlock`, `StatsBlock`, `CTABannerBlock`, `LogoStripBlock`,
`SpacerBlock`, `DividerBlock`, `GalleryBlock`, `FlexibleImageTextBlock`, `VideoBlock`,
`MapBlock`, `ButtonLinkBlock`, `DropdownBlock`, `TextWithButtonBlock`, `EmailButtonBlock`,
`ProductCardBlock`, `CatalogItemBlock`, `ProductsGalleryBlock`, `ProductColumnBlock`,
`ContactFormBlock`, `ChatBlock`

---

## Existing templates

| File | Use case |
|------|----------|
| `landing-generic.json` | Generic business landing / homepage |
| `services.json` | Services or solutions page |
| `about.json` | About us / company story |
| `contact.json` | Contact page with form and map |
| `faq.json` | FAQ / help center page |
| `ecommerce-catalog.json` | Product catalog / shop page |
| `saas-pricing.json` | SaaS pricing tiers page |
| `restaurant.json` | Restaurant / food business page |
| `figma-vectoronly.json` | Figma import — no photo assets (vectors/illustrations only) |
