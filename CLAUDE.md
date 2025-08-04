# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Common Development Commands

### Build and Run
- `dotnet build` - Build the application
- `dotnet run --project GestorPresupuestosAPI` - Run the application in development mode
- `dotnet watch --project GestorPresupuestosAPI` - Run with hot reload for development

### Development URLs
- Development: http://localhost:5108 (HTTP) or https://localhost:7263 (HTTPS)
- Swagger UI: Available at `/swagger` endpoint when running in development

## Architecture Overview

This is a **Budget Management System API** (Gestor de Presupuestos) built with ASP.NET Core 8.0 using a clean layered architecture pattern.

### Project Structure
```
GestorPresupuestosAPI/
├── Features/                    # Feature-based organization
│   ├── Controllers/            # API Controllers (14 controllers)
│   ├── Repository/             # Data access layer
│   ├── Services/               # Business logic layer
│   └── Utility/                # Shared utilities (ApiResponse, Security)
├── Infrastructure/
│   ├── DataBases/              # Entity Framework DbContext
│   ├── Entities/               # Database entities (18 entities)
│   └── Modelos/                # DTOs and view models
├── Properties/                 # Launch settings and publish profiles
└── Program.cs                  # Application entry point
```

### Architecture Pattern
The application follows a **Repository-Service-Controller** pattern:
- **Controllers**: Handle HTTP requests and responses using standardized `ApiResponse`
- **Services**: Contain business logic and coordinate between controllers and repositories
- **Repositories**: Handle data access using Entity Framework Core
- **Entities**: Database models representing the budget management domain
- **DTOs/Models**: Data transfer objects for API communication

### Key Domain Areas
The system manages budgets across multiple dimensions:
- **Users & Departments**: User management with role-based access
- **Budget Structure**: Categories → Subcategories → Accounts → Budget Accounts
- **Financial Tracking**: Budget execution, partial executions, and reporting
- **Notifications**: System notifications for budget-related events
- **Reporting**: Integration with Microsoft Reporting Services

### Database
- Uses **SQL Server** with Entity Framework Core
- Connection configured in `appsettings.json`
- DbContext: `GestorPresupuestosAHM` with 18+ entity sets
- Main entities include: Usuarios, Presupuesto, Cuenta, Departamentos, Categorias, etc.

### Key Dependencies
- **Microsoft.EntityFrameworkCore.SqlServer** (8.0.10) - Database ORM
- **Microsoft.ReportingServices.ReportViewerControl.WebForms** (150.1652.0) - Reporting
- **MailKit/MimeKit** (4.11.0) - Email functionality
- **Swashbuckle.AspNetCore** (6.9.0) - API documentation

### CORS Configuration
- Allows frontend access from `http://localhost:5173` (likely React/Vue dev server)
- Production domain: `https://www.ahm-honduras.com`

### Development Notes
- All services and repositories are registered with **Scoped** lifetime in DI container
- Uses **InvariantCulture** for consistent number/date formatting
- Swagger UI enabled in development environment
- HTTP request file available at `GestorPresupuestosAPI.http` for API testing

### Service Registration Pattern
Both repositories and services follow consistent naming and registration:
```csharp
builder.Services.AddScoped<EntityNameRepository>();
builder.Services.AddScoped<EntityNameService>();
```

When adding new features, follow the established pattern of creating matching Repository, Service, and Controller classes for each domain entity.