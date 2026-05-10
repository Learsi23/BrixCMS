# Contributing to BrixCMS

Thanks for your interest in contributing! Here's how to get started.

## Getting Started

1. Fork the repository
2. Clone your fork
3. Run `dotnet build BrixCMS.Open/BrixCMS.Open.csproj` to verify everything builds

## Code Conventions

- All code comments must be in English
- Use .NET 10 (net10.0) and C# 13 features
- Follow existing patterns (file-scoped namespaces, primary constructors, etc.)
- No repository pattern — inject `BrixDbContext` directly
- Use `ILogger<T>` for logging, never `Console.WriteLine`

## Block Types

To add a new block type:

1. Create a model class in `Models/Blocks/` extending `BlockBase` or `BlockGroupBase`
2. Create a Razor view in `Views/Cms/Blocks/{category}/{BlockName}.cshtml`
3. Register it in `Extensions/BlockRegistrationExtensions.cs`
4. Add a seeder entry in `Services/BrixLandingSeeder.cs` (optional)

## Pull Request Process

1. Ensure your code builds with zero errors
2. Keep PRs focused on a single concern
3. Update the README if your change affects functionality
4. The PR title should summarize the change (e.g. "Add video background block")

## Reporting Issues

Include:
- BrixCMS version (or commit hash)
- .NET version (`dotnet --info`)
- Database provider (SQLite / SQL Server / PostgreSQL)
- Steps to reproduce
- Expected vs actual behavior
