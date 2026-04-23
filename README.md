# BrixCMS.Open

Simplified version of BrixCMS.Pro without product features, Stripe, or Figma.

## Changes Made

### Removed:
- **Controllers:** ProductController, ProductPageController, CheckoutController, CartController, OrdersController, PdfProductsController, FigmaController, StripeConnectController, AiGeneratorController, AdminAssistantController, ConfiguracionController
- **Services:** ProductService, StripeService, FigmaDownloadService, FigmaPromptService, Notifications
- **Models:** ProductEntity, CartItem, Order
- **DTOs:** ProductDto, ProductDetailDto, RemoveCartItemDto
- **Views:** Products, Orders, PdfProducts, Figma, AiGenerator, AdminAssistant, Configuracion (Stripe)
- **Seeders:** RestaurantSeeder (product-related)

### Kept:
- AI for chatbot with PDF search (ChatBlock, FloatingChatBlock)
- Block system (blocks) identical to the Next.js version
- Page generator and visual editor
- Admin panel (/Manager/)
- Semantic search and PDF ingestion
- AI provider configuration (Gemini, DeepSeek, Mistral, Ollama)

### Modified Files:
- `BrixCMS.Open.csproj` – Removed Stripe.net reference
- `PromptsService.cs` – Simplified for PDF chatbot only
- `BrixDbContext.cs` – Removed Products, CartItems, Orders tables
- `BlockRegistrationExtensions.cs` – Removed e-commerce blocks
- `ManagerController.cs` – Cleaned up obsolete references
- `Program.cs` – Removed Stripe/Orders services and configurations
- Various `.razor` files – Updated to `BrixCMS.Open`

## Block Structure
The version includes all content, design, marketing, interactive, and multimedia blocks available in the Next.js version.

## How to Run

```bash
cd C:\Source\MyProjects\BrixCMS.Open\BrixCMS.Open
dotnet run --urls=https://localhost:5001