# BrixCMS.Open

> A lightweight, open-source CMS built with ASP.NET Core and Blazor. Simplified version of BrixCMS.Pro — without e-commerce, Stripe, or Figma integrations.

## Features

- **AI-Powered Chatbot** — Integrated chatbot with PDF search and semantic retrieval
- **Visual Page Editor** — Drag-and-drop block system for building pages
- **Block Architecture** — Content, design, marketing, interactive, and multimedia blocks (compatible with Next.js version)
- **Admin Panel** — Full management interface at `/Manager/`
- **Multi-Provider AI** — Configure Gemini, DeepSeek, Mistral, or Ollama
- **PDF Ingestion** — Upload and index PDF documents for semantic search

## Tech Stack

| Layer       | Technology          |
|-------------|---------------------|
| Framework   | ASP.NET Core 9      |
| UI          | Blazor + Razor Pages|
| Database    | SQLite (EF Core)    |
| AI          | Multi-provider SDK  |
| Search      | Semantic search     |

## Quick Start

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQLite (included)

### Run

```bash
cd BrixCMS.Open
dotnet run --urls=https://localhost:5001
```

Then open `https://localhost:5001` in your browser.

## What's Included vs Removed

### Kept
| Feature          | Description                                |
|------------------|--------------------------------------------|
| AI Chatbot       | ChatBlock + FloatingChatBlock with PDF search |
| Block System     | Full block architecture (Next.js compatible) |
| Page Generator   | Visual page editor and builder             |
| Admin Panel      | Complete `/Manager/` dashboard             |
| Semantic Search  | PDF ingestion and semantic retrieval       |
| AI Providers     | Gemini, DeepSeek, Mistral, Ollama          |

### Removed
| Category    | Items                                                          |
|-------------|----------------------------------------------------------------|
| Controllers | Product, ProductPage, Checkout, Cart, Orders, PdfProducts, Figma, StripeConnect, AiGenerator, AdminAssistant, Configuracion |
| Services    | ProductService, StripeService, FigmaDownloadService, FigmaPromptService, Notifications |
| Models      | ProductEntity, CartItem, Order                                 |
| Views       | Products, Orders, PdfProducts, Figma, AiGenerator, AdminAssistant, Configuracion |
| Seeders     | RestaurantSeeder (product-related)                             |

## Project Structure

```
BrixCMS.Open/
├── Areas/            # Admin panel areas
├── Components/       # Blazor components & blocks
├── Controllers/      # MVC controllers
├── Data/             # Database context & migrations
├── DTOs/             # Data transfer objects
├── Models/           # Entity models
├── Services/         # Business logic services
├── Views/            # Razor views
├── wwwroot/          # Static assets
├── Program.cs        # Application entry point
└── appsettings.json  # Configuration
```

## Configuration

AI providers are configured in `appsettings.json`. Supported providers:

- **Gemini** (Google)
- **DeepSeek**
- **Mistral**
- **Ollama** (local/self-hosted)

## License

Open source. See LICENSE for details.
