# BrixCMS

> Open-source, block-based CMS for .NET 10. Build pages visually, ship AI-powered chatbots, and run everything on a single SQLite file.

[![NuGet](https://img.shields.io/nuget/v/BrixCMS.Open.Templates?label=nuget)](https://www.nuget.org/packages/BrixCMS.Open.Templates)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![GitHub Stars](https://img.shields.io/github/stars/Learsi23/BrixCMS?style=social)](https://github.com/Learsi23/BrixCMS/stargazers)

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
| **45+ content blocks** | Hero, cards, testimonials, FAQs, galleries, maps, video, countdown, tabs, and more |
| **AI chatbot** | Embed a PDF-trained chatbot on any page — Ollama (local, free, private) or Gemini (cloud, free tier) |
| **Visual editor** | Drag-and-drop block builder in the admin panel — no code required |
| **Live text editing** | Double-click any highlighted text directly on the live preview to edit it in place — saves in the background, no reload |
| **Device preview** | Toggle Desktop / Tablet / Mobile on the preview page — a real iframe, so Tailwind's responsive breakpoints actually re-trigger |
| **Page history** | Every publish is snapshotted automatically; roll back to any prior version from the editor in one click |
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
| AI | Microsoft.Extensions.AI (Ollama local · Gemini cloud) |
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
Option B — clone the repo
git clone https://github.com/Learsi23/BrixCMS
cd BrixCMS/BrixCMS.Open
dotnet run
Open https://localhost:5001 — your site is running.

Admin panel: https://localhost:5001/Manager
Default credentials: admin@brix.com / admin123 — change these immediately.

AI Chatbot Setup
BrixCMS.Open supports two AI modes: local (Ollama, free, zero data leaves your server) and cloud (Gemini — BYOK). You can switch between them from the admin panel at Configuration → Chatbot & Security, no code changes needed.

Option A — Ollama (local, default)
# Install Ollama from https://ollama.com, then:
ollama pull llama3.1:8b   # chat model
ollama pull all-minilm    # embeddings for PDF search
No configuration needed — BrixCMS connects to http://localhost:11434 automatically. You can also download models directly from the admin panel.

Option B — Gemini (cloud, free tier available)
Get a free API key at aistudio.google.com/app/apikey — Gemini 2.5 Flash Lite is free (60 req/min, 1 M tokens/day).
Open the admin panel → Configuration → Chatbot & Security.
Paste your key in the Gemini section and click Save.
Pick a model (gemini-2.5-flash-lite / gemini-2.5-flash / gemini-2.5-pro) and click Set Active.
The key is stored encrypted at rest (AES-256-GCM) and never leaves your server. If Gemini is unavailable the system falls back to Ollama automatically.

PDF-trained chatbot
Drop any PDF into wwwroot/Data/ (or upload via the admin panel). On startup BrixCMS ingests it into the local vector store and the ChatBlock answers questions about it with source citations. Works with both Ollama and Gemini.

Block Reference
Layout
GridColumn · FullColumnBlock · SpacerBlock · DividerBlock · BannerBlock

Content
HeroBlock · HeroFlexibleBlock · TextBlock · MarkdownBlock · ImageBlock · CardBlock · IconCardBlock · StatsBlock · CTABannerBlock · LogoStripBlock · TimelineBlock · TestimonialsBlock · SocialProofBlock · TeamBlock · FeatureGridBlock · FeatureListBlock · MenuBlock · OpeningHoursBlock · TableBlock · CodeBlock

Media
GalleryBlock · VideoBlock · FlexibleImageTextBlock · MapBlock · BeforeAfterBlock · AudioBlock · LottieBlock · QRCodeBlock

Interactive
ButtonLinkBlock · AccordionBlock · TabsBlock · FAQBlock · CountdownBlock · NewsletterBlock · CookieBannerBlock · DropdownBlock

AI / Blazor
ChatBlock · FloatingChatBlock · ContactFormBlock

Headless API
BrixCMS exposes a read-only JSON API so you can use it as a backend for any frontend — Next.js, Nuxt, React, Vue, Flutter, or anything that speaks HTTP.

All endpoints return JSON. No authentication required for published content.

GET /api/content/pages
Returns all published pages with their SEO metadata. Use this to build navigation, sitemaps, or a page list in your frontend.

Response

[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "Home",
    "slug": "",
    "metaDescription": "Welcome to my site — built with BrixCMS.",
    "ogImage": "/images/og-home.jpg",
    "metaKeywords": "cms, dotnet, open source",
    "publishedAt": "2025-01-15T10:30:00Z",
    "sortOrder": 0,
    "pageType": "standard"
  },
  {
    "id": "7c9e6679-7425-40de-944b-e07fc1f90ae7",
    "title": "About",
    "slug": "about",
    "metaDescription": "Learn more about us.",
    "ogImage": null,
    "metaKeywords": null,
    "publishedAt": "2025-01-16T08:00:00Z",
    "sortOrder": 1,
    "pageType": "standard"
  }
]
GET /api/content/pages/{slug}
Returns a single published page with its full nested block tree. The home page has an empty slug ("").

GET /api/content/pages/about
GET /api/content/pages/        ← home page
Response

{
  "id": "7c9e6679-7425-40de-944b-e07fc1f90ae7",
  "title": "About",
  "slug": "about",
  "metaDescription": "Learn more about us.",
  "ogImage": "/images/about-og.jpg",
  "metaKeywords": "about, team",
  "publishedAt": "2025-01-16T08:00:00Z",
  "pageType": "standard",
  "blocks": [
    {
      "id": "a1b2c3d4-0000-0000-0000-000000000001",
      "parentId": null,
      "type": "HeroBlock",
      "sortOrder": 0,
      "data": {
        "title": "We build great products",
        "subtitle": "A small team with big ambitions.",
        "buttonText": "Get started",
        "buttonUrl": "/contact",
        "backgroundImage": "/images/hero-bg.jpg"
      },
      "children": []
    },
    {
      "id": "a1b2c3d4-0000-0000-0000-000000000002",
      "parentId": null,
      "type": "GridColumn",
      "sortOrder": 1,
      "data": { "columns": 2 },
      "children": [
        {
          "id": "a1b2c3d4-0000-0000-0000-000000000003",
          "parentId": "a1b2c3d4-0000-0000-0000-000000000002",
          "type": "TextBlock",
          "sortOrder": 0,
          "data": { "content": "<p>Our mission is...</p>" },
          "children": []
        },
        {
          "id": "a1b2c3d4-0000-0000-0000-000000000004",
          "parentId": "a1b2c3d4-0000-0000-0000-000000000002",
          "type": "ImageBlock",
          "sortOrder": 1,
          "data": { "src": "/images/team.jpg", "alt": "Our team" },
          "children": []
        }
      ]
    }
  ]
}
Blocks that act as containers (e.g. GridColumn) nest their child blocks under children. Leaf blocks have an empty children array.

GET /api/content/pages/{slug}/blocks
Returns a flat list of all blocks for a page, without nesting. Use this when you want to build your own tree or render blocks in a custom order.

[
  {
    "id": "a1b2c3d4-0000-0000-0000-000000000001",
    "parentId": null,
    "type": "HeroBlock",
    "sortOrder": 0,
    "data": {
      "title": "We build great products",
      "buttonText": "Get started",
      "buttonUrl": "/contact"
    }
  },
  {
    "id": "a1b2c3d4-0000-0000-0000-000000000002",
    "parentId": null,
    "type": "GridColumn",
    "sortOrder": 1,
    "data": { "columns": 2 }
  },
  {
    "id": "a1b2c3d4-0000-0000-0000-000000000003",
    "parentId": "a1b2c3d4-0000-0000-0000-000000000002",
    "type": "TextBlock",
    "sortOrder": 0,
    "data": { "content": "<p>Our mission is...</p>" }
  }
]
POST /api/newsletter/subscribe
Subscribe an email address to the newsletter.

Request body

{ "email": "user@example.com", "name": "Alice" }
Responses

Status	Body
200 OK	{ "ok": true }
400 Bad Request	{ "error": "Invalid email address." }
409 Conflict	{ "error": "You are already subscribed." }
Error responses
All error responses follow the same shape:

{ "error": "Page not found." }
404 is returned when a page does not exist or is unpublished.

Consuming the API from Next.js
// lib/brixcms.ts
const BASE = process.env.BRIX_URL ?? 'https://your-brixcms-host.com';

export async function getPage(slug: string) {
  const res = await fetch(`${BASE}/api/content/pages/${slug}`, {
    next: { revalidate: 60 },   // ISR — revalidate every 60 seconds
  });
  if (!res.ok) return null;
  return res.json();
}

export async function getAllPages() {
  const res = await fetch(`${BASE}/api/content/pages`);
  return res.json();
}
// app/[slug]/page.tsx
import { getPage, getAllPages } from '@/lib/brixcms';

export async function generateStaticParams() {
  const pages = await getAllPages();
  return pages.map((p: any) => ({ slug: p.slug || '' }));
}

export default async function CmsPage({ params }: { params: { slug: string } }) {
  const page = await getPage(params.slug);
  if (!page) notFound();
  // render page.blocks however you like
}
Configuration
appsettings.json — set before deploying:

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
Use User Secrets or environment variables for all sensitive values in production.

Deployment
BrixCMS runs on any server with .NET 10. No Docker required.

dotnet publish -c Release -o ./publish
# Copy ./publish to your server and run:
dotnet BrixCMS.Open.dll
Works on: Azure App Service · DigitalOcean · Hetzner · VPS with systemd

For Azure Sweden North (recommended for Swedish GDPR compliance), use the standard App Service Linux plan.

Project Structure
BrixCMS.Open/
├── Areas/Manager/          # Admin panel (login, pages, media, team, backup)
├── Components/             # Blazor components (chat UI, contact form, interactive blocks)
├── Controllers/            # CMS, API, sitemap, landing
├── Data/                   # EF Core context + entity models
├── Models/                 # 45+ block type definitions
├── Services/               # Auth, AI, PDF ingestion, email, encryption
├── Views/                  # Razor views (blocks, layouts, admin)
├── wwwroot/                # Static assets + PDF data folder
├── Program.cs              # App startup
└── appsettings.json        # Configuration
vs BrixCMS Pro
Feature	Open	Pro
All 45+ blocks	✅	✅
AI chatbot (Ollama local)	✅	✅
Cloud AI — Gemini (BYOK, free tier)	✅	✅
Multi-admin with role-based permissions	✅	✅
Visual drag-and-drop editor	✅	✅
Headless REST API	✅	✅
PDF semantic search	✅	✅
SQLite / SQL Server / PostgreSQL	✅	✅
Figma import	—	✅
AI page generator	—	✅
E-commerce (Stripe)	—	✅
White-label (remove attribution)	—	✅
Priority support	—	✅
BrixCMS Pro →

Security Notes
Passwords: PBKDF2-SHA256, 100,000 iterations
API keys: AES-256-GCM encrypted at rest
2FA: TOTP (RFC 6238) — works with Google Authenticator, Authy, Microsoft Authenticator
Rate limiting: 5 login attempts/min, 20 AI requests/min
Security headers: X-Frame-Options, X-Content-Type-Options, Referrer-Policy
## Support the Project

If BrixCMS saves you time, please **[star us on GitHub](https://github.com/Learsi23/BrixCMS/stargazers)** ⭐ — it takes two seconds and helps others discover it.

[![Star on GitHub](https://img.shields.io/github/stars/Learsi23/BrixCMS?style=for-the-badge&logo=github&label=Star%20on%20GitHub&color=22c55e)](https://github.com/Learsi23/BrixCMS/stargazers)

## License
MIT — free to use, modify, and distribute. Attribution required in the OSS version (footer bar). Remove it by upgrading to BrixCMS Pro.
