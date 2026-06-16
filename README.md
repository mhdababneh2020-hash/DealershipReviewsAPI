# Dealership Reviews API

A RESTful API for managing car dealerships and customer reviews, built with ASP.NET Core and Entity Framework.

## Tech Stack

- Backend: C# + ASP.NET Core Web API
- ORM: Entity Framework Core
- Database: SQLite
- Documentation: Swagger UI

## Features

- Full CRUD for Dealerships
- Full CRUD for Reviews
- Filter reviews by dealership
- Auto-generated Swagger documentation
- CORS enabled

## API Endpoints

### Dealerships
- GET /api/Dealerships
- GET /api/Dealerships/{id}
- POST /api/Dealerships
- PUT /api/Dealerships/{id}
- DELETE /api/Dealerships/{id}

### Reviews
- GET /api/Reviews
- GET /api/Reviews/dealer/{dealershipId}
- POST /api/Reviews
- DELETE /api/Reviews/{id}

## Run Locally

1. Clone the repo
2. Run: dotnet run
3. Open: http://localhost:5083/swagger

## Live Demo
https://dealershipreviewsapi.onrender.com/swagger

## License
MIT
