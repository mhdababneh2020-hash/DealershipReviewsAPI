# Dealership Reviews API

[![CI](https://github.com/mhdababneh2020-hash/DealershipReviewsAPI/actions/workflows/ci.yml/badge.svg)](https://github.com/mhdababneh2020-hash/DealershipReviewsAPI/actions/workflows/ci.yml)

A RESTful API for managing car dealerships and customer reviews, built with ASP.NET Core Web API using a layered architecture (Controllers → Services → EF Core).

## Tech Stack

- Backend: C# + ASP.NET Core Web API
- ORM: Entity Framework Core (code-first with migrations)
- Database: SQL Server / SQL Server Express
- Auth: JWT Bearer tokens, BCrypt password hashing
- Testing: xUnit + Moq + EF Core InMemory (23 tests)
- Documentation: Swagger UI with Bearer token support
- CI: GitHub Actions

## Architecture

```text
Controllers/   HTTP layer — routing, status codes, [Authorize]
Services/      Business logic behind interfaces (IReviewService, ...)
Dtos/          API contracts — validation, no entity/PasswordHash leakage
Models/        EF Core entities
Data/          AppDbContext
Migrations/    EF Core schema history
DealershipReviewsAPI.Tests/   Unit tests
```

- Controllers depend on service **interfaces** only (DI, testable, SOLID).
- Entities never leave the API — every endpoint returns DTOs.
- Input is validated with DataAnnotations, such as rating validation from 1 to 5.
- Database schema is managed by EF Core migrations and applied on startup.

## Features

- Full CRUD operations for dealerships
- Create, list, and delete operations for reviews
- JWT register/login; write operations require a valid token
- Search dealerships by city/state
- Average rating and review count calculated from review data
- Auto-generated Swagger documentation
- GitHub Actions CI for build and test validation

## API Endpoints

### Auth

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

Requirements:

- .NET SDK
- SQL Server or SQL Server Express

1. Copy `appsettings.example.json` to `appsettings.json`
2. Set your connection string and a strong JWT key
3. Run the API:

```bash
dotnet run
```

Open Swagger:

```text
http://localhost:5083/swagger
```

The database is created or updated automatically through EF Core migrations. Sample requests are available in `DealershipReviewsAPI.http`.

## Run Tests

```bash
dotnet test DealershipReviewsAPI.Tests
```

## Live Demo

```text
https://dealershipreviewsapi.onrender.com/swagger
```

Note: the live demo runs an earlier SQLite build; the current version targets SQL Server.

## Documentation

```text
https://mhdababneh2020-hash.github.io/DealershipReviewsAPI
```

## License

MIT
