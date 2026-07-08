# Dealership Reviews API

[![CI](https://github.com/mhdababneh2020-hash/DealershipReviewsAPI/actions/workflows/ci.yml/badge.svg)](https://github.com/mhdababneh2020-hash/DealershipReviewsAPI/actions/workflows/ci.yml)

A RESTful API for managing car dealerships and customer reviews, built with ASP.NET Core 9 using a layered architecture (Controllers → Services → EF Core).

## Tech Stack

- Backend: C# + ASP.NET Core 9 Web API
- ORM: Entity Framework Core 9 (code-first with Migrations)
- Database: SQL Server (Express)
- Auth: JWT Bearer tokens, BCrypt password hashing
- Testing: xUnit + Moq + EF Core InMemory (18 tests)
- Documentation: Swagger UI with Bearer token support

## Architecture

```
Controllers/   HTTP layer — routing, status codes, [Authorize]
Services/      Business logic behind interfaces (IReviewService, ...)
Dtos/          API contracts — validation, no entity/PasswordHash leakage
Models/        EF Core entities
Data/          AppDbContext
Migrations/    EF Core schema history
DealershipReviewsAPI.Tests/   Unit tests
```

- Controllers depend on service **interfaces** only (DI, testable, SOLID)
- Entities never leave the API — every endpoint returns DTOs
- Input validated with DataAnnotations (e.g. rating must be 1–5)
- Database schema managed by EF Migrations, applied on startup

## Features

- Full CRUD for Dealerships and Reviews
- JWT register/login; write operations require a valid token
- Search dealerships by city/state
- Average rating + review count computed in SQL
- Auto-generated Swagger documentation, CORS enabled

## API Endpoints

### Auth (public)
- POST /api/Auth/register
- POST /api/Auth/login → returns JWT token

### Health
- GET /health

### Dealerships
- GET /api/Dealerships?city=&state= (public)
- GET /api/Dealerships/{id} (public)
- POST /api/Dealerships 🔒
- PUT /api/Dealerships/{id} 🔒
- DELETE /api/Dealerships/{id} 🔒

### Reviews
- GET /api/Reviews (public)
- GET /api/Reviews/dealer/{dealershipId} (public)
- POST /api/Reviews 🔒
- DELETE /api/Reviews/{id} 🔒

🔒 = requires `Authorization: Bearer <token>` header

## Run Locally

Requires .NET 9 SDK and SQL Server (Express).

1. Copy `appsettings.example.json` to `appsettings.json`
2. Set your connection string and a strong JWT key
3. Run:

```
dotnet run
```

Open http://localhost:5083/swagger — the database is created/updated automatically via migrations. Sample requests are also available in `DealershipReviewsAPI.http`.

## Run Tests

```
dotnet test DealershipReviewsAPI.Tests
```

## Live Demo
https://dealershipreviewsapi.onrender.com/swagger

Note: the live demo runs an earlier SQLite build; the current version targets SQL Server, which Render's free tier does not host.

## Documentation
https://mhdababneh2020-hash.github.io/DealershipReviewsAPI

## License
MIT
