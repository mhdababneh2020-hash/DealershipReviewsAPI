# Dealership Reviews API

[![CI](https://github.com/mhdababneh2020-hash/DealershipReviewsAPI/actions/workflows/ci.yml/badge.svg)](https://github.com/mhdababneh2020-hash/DealershipReviewsAPI/actions/workflows/ci.yml)

A RESTful API for managing car dealerships and customer reviews, built with ASP.NET Core using a layered architecture (Controllers → Services → EF Core).

## Tech Stack

- Backend: C# + ASP.NET Core Web API
- ORM: Entity Framework Core (code-first with migrations)
- Database: SQL Server (Express)
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
