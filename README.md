# BrixCMS

> Open-source, block-based CMS for .NET 10. Build pages visually, ship AI-powered chatbots, and run everything on a single SQLite file.

[![NuGet](https://img.shields.io/nuget/v/BrixCMS.Open.Templates?label=nuget)](https://www.nuget.org/packages/BrixCMS.Open.Templates)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

---

## What is BrixCMS?

BrixCMS is a free, self-hosted CMS built on ASP.NET Core 10 + Blazor. No Docker required, no managed cloud, no SaaS subscription. Drop it on any server with .NET installed and you're live in minutes.

**Who is it for?**
- .NET developers who don't want to reach for WordPress
- Agencies building client sites on a familiar stack
- Teams that need GDPR-compliant hosting (no data leaves your server by default)

---

## Features

| | |
|---|---|
| **40+ content blocks** | Hero, cards, testimonials, FAQs, galleries, maps, video, countdown, tabs, and more |
| **AI chatbot** | Embed a PDF-trained chatbot on any page — runs locally via Ollama (free, private, no API key needed) |
| **Visual editor** | Drag-and-drop block builder in the admin panel — no code required |
| **Multi-admin** | Invite team members; the owner manages access |
| **PDF semantic search** | Drop PDFs in `wwwroot/Data/` and the chatbot answers questions about them |
| **SEO ready** | Meta, OG tags, canonical URLs, XML sitemap |
| **GDPR-friendly** | AI defaults to local Ollama — zero data to external services |
| **Single SQLite file** | No Postgres, no Redis, no infra overhead |
| **Headless API** | `GET /api/content/pages/{slug}` — serve content to Next.js, Nuxt or any frontend |
| **Newsletter** | Built-in subscriber capture and API endpoint |
| **Cookie banner** | GDPR cookie consent block out of the box |

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 10 |
| UI | Blazor Server + Razor Views |
| Database | SQLite via EF Core 10 |
| AI | Microsoft.Extensions.AI (Ollama · Gemini · DeepSeek · Mistral) |
| Semantic search | In-memory vector store (Semantic Kernel) |
| Styling | Tailwind CSS (CDN) + Alpine.js |
| Rich text | TinyMCE |

---

## Quick Start

**Prerequisites:** [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Option A — dotnet new (recommended)
```bash
dotnet new install BrixCMS.Open.Templates
dotnet new brixcms -n MyWebsite
cd MyWebsite
dotnet run
```

### Option B — clone the repo
```bash
git clone https://github.com/Learsi23/BrixCMS
cd BrixCMS/BrixCMS.Open
dotnet run
```

Open `https://localhost:5001` — your site is running.

Admin panel: `https://localhost:5001/Manager`  
Default credentials: `admin@brix.com` / `admin123` — **change these immediately.**

---

## AI Chatbot Setup

BrixCMS.Open ships with Ollama — local, free, and fully private.

**Install Ollama and pull models:**
```bash
# Install Ollama from https://ollama.com, then:
ollama pull llama3.1:8b   # chat model
ollama pull all-minilm    # for PDF embeddings
```
No configuration needed — BrixCMS connects to `localhost:11434` automatically.

**PDF-trained chatbot:**  
Drop any PDF into `wwwroot/Data/`. On startup, BrixCMS ingests it into the local vector store. The `ChatBlock` answers questions about it with source citations.

> **Need cloud AI (Gemini, DeepSeek, Mistral)?** That's available in [BrixCMS Pro](https://brixcms.se) via BYOK — you connect your own API keys.

---

## Block Reference

### Layout
`GridColumn` · `FullColumnBlock` · `SpacerBlock` · `DividerBlock` · `BannerBlock`

### Content
`HeroBlock` · `HeroFlexibleBlock` · `TextBlock` · `MarkdownBlock` · `ImageBlock` · `CardBlock` · `IconCardBlock` · `StatsBlock` · `CTABannerBlock` · `LogoStripBlock` · `TimelineBlock` · `TestimonialsBlock` · `SocialProofBlock` · `TeamBlock` · `FeatureGridBlock` · `FeatureListBlock` · `MenuBlock` · `OpeningHoursBlock` · `TableBlock` · `CodeBlock`

### Media
`GalleryBlock` · `VideoBlock` · `FlexibleImageTextBlock` · `MapBlock` · `BeforeAfterBlock` · `AudioBlock` · `LottieBlock` · `QRCodeBlock`

### Interactive
`ButtonLinkBlock` · `AccordionBlock` · `TabsBlock` · `FAQBlock` · `CountdownBlock` · `NewsletterBlock` · `CookieBannerBlock` · `DropdownBlock`

### AI / Blazor
`ChatBlock` · `FloatingChatBlock` · `ContactFormBlock`

---

## Configuration

**`appsettings.json`** — set before deploying:

```json
{
  "EncryptionKey": "your-32-char-minimum-secret-key!!",
  "SmtpSettings": {
    "SmtpServer": "smtp.resend.com",
    "SmtpPort": 465,
    "SmtpUsername": "resend",
    "SmtpPassword": "YOUR_API_KEY",
    "FromEmail": "hello@yourdomain.com"
  },
  "Ollama": {
    "ChatModel": "llama3.1:8b",
    "EmbeddingModel": "all-minilm:latest"
  }
}
```

Use [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) or environment variables for all sensitive values in production.

---

## Deployment

BrixCMS runs on any server with .NET 10. No Docker required.

```bash
dotnet publish -c Release -o ./publish
# Copy ./publish to your server and run:
dotnet BrixCMS.Open.dll
```

Works on: **Azure App Service** · **DigitalOcean** · **Hetzner** · **VPS with systemd**

For Azure Sweden North (recommended for Swedish GDPR compliance), use the standard App Service Linux plan.

---

## Project Structure

```
BrixCMS.Open/
├── Areas/Manager/          # Admin panel (login, pages, media, team, backup)
├── Components/             # Blazor components (chat UI, contact form, interactive blocks)
├── Controllers/            # CMS, API, sitemap, landing
├── Data/                   # EF Core context + entity models
├── Models/                 # 40+ block type definitions
├── Services/               # Auth, AI, PDF ingestion, email, encryption
├── Views/                  # Razor views (blocks, layouts, admin)
├── wwwroot/                # Static assets + PDF data folder
├── Program.cs              # App startup
└── appsettings.json        # Configuration
```

---

## vs BrixCMS Pro

| Feature | Open | Pro |
|---|---|---|
| All 40+ blocks | ✅ | ✅ |
| AI chatbot | ✅ | ✅ |
| Multi-admin | ✅ | ✅ |
| Visual page editor | ✅ | ✅ |
| Figma import | — | ✅ |
| AI page generator | — | ✅ |
| E-commerce (Stripe) | — | ✅ |
| White-label (remove attribution) | — | ✅ |
| Priority support | — | ✅ |

[BrixCMS Pro →](https://brixcms.se)

---

## Security Notes

- Passwords: PBKDF2-SHA256, 100,000 iterations
- API keys: AES-256-GCM encrypted at rest
- 2FA: TOTP (RFC 6238) — works with Google Authenticator, Authy, Microsoft Authenticator
- Rate limiting: 5 login attempts/min, 20 AI requests/min
- Security headers: X-Frame-Options, X-Content-Type-Options, Referrer-Policy

---

## License

MIT — free to use, modify, and distribute. Attribution required in the OSS version (footer bar). Remove it by upgrading to [BrixCMS Pro](https://brixcms.se).
