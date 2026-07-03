# 🏠 House Renting System

A full-stack House Renting System developed using ASP.NET Core MVC, Entity Framework Core, ASP.NET Identity, SQLite, and REST API.

This project allows tenants to search rental properties, landlords to manage listings, and administrators to monitor the platform.

---

# Team Members

| Name | Role |
|------|------|
| Rol Akphinoun | Frontend & API Development |
| Chhun Menghour | Backend & MVC Features |

---

# Technology Stack

- User Authentication
- House Listings
- Rental Requests
- Reviews
- Admin Dashboard
- REST API
- Swagger Documentation

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
Default Admin

Email:
admin@gmail.com

Password:
Admin@123

---

# Project Structure

```
Controllers/
Models/
Views/
Data/
DTOs/
wwwroot/
```

---

# Database

SQLite

```
houserenting.db
```

---

## Running

dotnet restore

dotnet run

Open:

https://localhost:5155