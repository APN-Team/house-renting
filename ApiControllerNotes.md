# API Controller Documentation

This document explains the architecture of the REST API used in the House Renting System, including the purpose of DTOs, API Controllers, Swagger, and the complete request flow.

---

# API Architecture

The project follows a layered architecture to separate presentation, business logic, and data access.

```
                Client
      (Browser / Swagger / Mobile)

                    │
                    ▼

           API Controller

                    │
                    ▼

           DTO Validation

                    │
                    ▼

        Entity Framework Core

                    │
                    ▼

             SQLite Database

                    │
                    ▼

          Standard API Response

                    │
                    ▼

                JSON Result
```

This architecture prevents the client from directly accessing Entity Framework models while making the backend easier to maintain and extend.

---

# API Controller

API Controllers expose backend services through REST endpoints.

Location

```
Controllers/
    Api/
        HousesApiController.cs
        RentalsApiController.cs
```

Responsibilities

- Receive HTTP Requests
- Validate Input
- Process Business Logic
- Query Database
- Return JSON Responses

Unlike MVC Controllers, API Controllers do not return Razor Views.

They return JSON data.

Example

```
GET /api/houses
```

returns

```json
{
    "success": true,
    "message": "Houses loaded successfully.",
    "data": [...]
}
```

---

# Why DTO?

DTO (Data Transfer Object) is used to separate Entity Framework Models from API responses.

Without DTO

```
Database Entity

↓

API

↓

Client
```

The client receives the entire database object.

Problems

- Sensitive fields may be exposed.
- Large payload size.
- Tight coupling with database models.
- Difficult to maintain.

---

With DTO

```
Database Entity

↓

DTO

↓

API

↓

Client
```

Only the required data is returned.

Example

Entity

```
House

Id
Title
Price
OwnerId
CreatedAt
UpdatedAt
InternalNotes
```

HouseDto

```
Id
Title
Price
City
HouseType
ImageUrl
```

Sensitive information remains inside the server.

---

# Current DTO Structure

```
DTOs

Common
    ApiResponse.cs

House
    HouseDto.cs
    CreateHouseDto.cs
    UpdateHouseDto.cs

Rental
    RentalDto.cs

Dashboard
    DashboardDto.cs
    LandlordStatsDto.cs
```

Each DTO has a specific responsibility.

HouseDto

Used when displaying house information.

CreateHouseDto

Used when creating a house.

UpdateHouseDto

Used when editing a house.

DashboardDto

Returns dashboard statistics.

RentalDto

Returns rental information.

ApiResponse<T>

Provides a consistent response format for every API.

---

# Standard API Response

Every API returns the same response structure.

```
Success

Message

Data
```

Example

```json
{
    "success": true,
    "message": "House created successfully.",
    "data":
    {
        ...
    }
}
```

Advantages

- Easy error handling
- Consistent frontend integration
- Predictable API structure

---

# CRUD Operations

The House API supports full CRUD operations.

Create

```
POST /api/houses
```

Read

```
GET /api/houses
GET /api/houses/{id}
```

Update

```
PUT /api/houses/{id}
```

Delete

```
DELETE /api/houses/{id}
```

These endpoints can be tested directly using Swagger.

---

# Search & Filtering

The API supports searching and filtering.

Supported filters

- Keyword
- City
- House Type
- Price Range
- Bedrooms
- Bathrooms

Example

```
GET /api/houses?city=Phnom Penh
```

Example

```
GET /api/houses?search=villa
```

Example

```
GET /api/houses?minPrice=100&maxPrice=500
```

---

# Pagination

Pagination is used to improve performance when large numbers of houses exist.

Example

```
GET /api/houses?page=1&pageSize=10
```

Benefits

- Faster response
- Lower memory usage
- Better user experience

---

# Swagger

Swagger is integrated into the project for API testing.

Location

```
/swagger
```

Swagger automatically generates

- API documentation
- Endpoint list
- Request examples
- Response examples

Developers can test every endpoint without creating a frontend.

Example workflow

```
Swagger

↓

Choose Endpoint

↓

Fill Parameters

↓

Execute

↓

JSON Response
```

This greatly speeds up backend testing.

---

# API Request Workflow

Every API request follows the same lifecycle.

```
Client

↓

API Controller

↓

Validate Request

↓

DTO

↓

Entity Framework Core

↓

SQLite Database

↓

Entity Framework Model

↓

DTO

↓

ApiResponse<T>

↓

JSON Response
```

This workflow keeps database models isolated from external clients.

---

# Database Workflow

The project uses Entity Framework Core.

```
API

↓

DbContext

↓

SQLite

↓

House

RentalRequest

Review

Payment
```

Entity Framework handles

- SQL generation
- CRUD operations
- Relationships
- Data tracking

---

# Automatic Demo Data Workflow

When the application starts

```
Program.cs

↓

DemoDataSeeder

↓

Generate Demo Houses

↓

Generate Demo Landlords

↓

Generate Images

↓

Database Ready
```

If the database is empty and a CSV file is available

```
Program.cs

↓

CsvHouseImporter

↓

SeedData/houses.csv

↓

Import Houses

↓

Database Ready
```

This allows the application to be demonstrated immediately without manually entering data.

---

# API Benefits

The API architecture provides several advantages.

- Clean separation between frontend and backend
- Standardized JSON responses
- Better security using DTOs
- Easier frontend integration
- Simpler maintenance
- Scalable architecture
- Swagger testing support
- Future support for React, Flutter, and Telegram Mini Apps

---

# Future Improvements

Possible future enhancements include

- JWT Authentication
- Refresh Tokens
- API Versioning
- Rate Limiting
- Role-based API Authorization
- Cloud Database
- Mobile Application Integration
- Telegram Mini App Integration