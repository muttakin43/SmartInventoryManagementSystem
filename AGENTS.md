# AGENTS.md

## Solution Structure

```
SmartInventoryManagementSystem.sln (6 projects)
├── SmartInventory.web         # MVC app (entry point: Program.cs)
├── SmartInventory.API         # Web API with Swagger
├── SmartInventory.BLL         # Business logic layer
├── SmartInventory.DAL         # EF Core data access
├── SmartInventory.Model       # Domain models + Identity
└── SmartInventory.Contract   # Interfaces + DTOs
```

## Run Commands

```powershell
# Build entire solution
dotnet build

# Run web app (MVC)
dotnet run --project SmartInventory.web

# Run API (Swagger available at /swagger)
dotnet run --project SmartInventory.API
```

## Database

- **SQL Server**: `DESKTOP-1D9CB4J\/database=SmartInventory`
- **Connection**: `appsettings.json` in web/API projects
- **Seeding**: `SmartInventory.web/Data/DbInitializer.cs` runs on app startup

## Key Patterns

- `AddRepositories()` and `AddServices()` in `Program.cs` register DI
- DAL uses EF Core with Repository pattern
- BLL contains business logic and depends on DAL, Contract, Model
- Model uses ASP.NET Core Identity (`ApplicationUser`)