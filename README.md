# 🏠 House Renting System

A full-stack House Renting System developed using ASP.NET Core MVC, Entity Framework Core, ASP.NET Identity, SQLite, and REST API.

This project allows tenants to search rental properties, landlords to manage listings, and administrators to monitor the platform.

---

# Team Members

| Name | Responsibilities |
|------|-------------------|
| **Rol Akphinoun** | Frontend UI Design, REST API Development, DTO Architecture, Dashboard API, Documentation |
| **Chhun Menghour** | Backend MVC Development, Authentication, CRUD Features, Database Models |

---

# 🛠 Technology Stack

## Frontend

- HTML5
- CSS3
- Bootstrap 5
- JavaScript
- Razor Views

## Backend

- ASP.NET Core MVC
- C#
- Entity Framework Core
- ASP.NET Identity

## Database

- SQLite

## API

- RESTful API
- DTO Architecture
- Swagger Documentation

## Development Tools

- Visual Studio
- Visual Studio Code
- Git
- GitHub

---

# Main Features

## Guest

- Browse houses
- Search houses
- Filter houses
- View house details
- Contact page

## Tenant

- Register
- Login
- Submit rental requests
- View rental requests
- Leave reviews

## Landlord

- Add houses
- Edit houses
- Delete houses
- View rental requests

## Admin

- Dashboard
- User Management
- Rental Management
- House Management

---

# REST API

Current API endpoints

```
GET    /api/houses
GET    /api/houses/{id}
GET    /api/rentals
GET    /api/dashboard
```

Swagger UI

``` 
https://localhost:5155/swagger
```

# 🚀 Running the Project

Restore packages

```bash
dotnet restore
```

Apply migrations (if needed)

```bash
dotnet ef database update
```

Run the project

```bash
dotnet run
```

---

# 🔐 Default Administrator Account

Email

```
admin@gmail.com
```

Password

```
Admin@123
```

## Final Project Summary

### Frontend

- Modern responsive UI
- Hero landing page
- Property cards redesign
- Dashboard redesign
- Responsive navigation
- Improved user experience

### Backend

- RESTful APIs
- DTO Architecture
- Dashboard API
- Rental API
- House API
- Standard API responses

### Database

- Entity Framework Core
- SQLite
- Automatic Demo Data Seeder
- CSV Import Support

### Documentation

- README
- API Notes
- Changelog
- Swagger Documentation

