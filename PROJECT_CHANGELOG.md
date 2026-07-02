# House Renting Project Changelog

## Sprint 1 - API Foundation

### Added

- DTO folder
- ApiResponse<T>
- HouseDto
- CreateHouseDto
- UpdateHouseDto

Reason:
Separate API responses from Entity Framework models and improve API maintainability.

--------------------------------------------

## Sprint 2

### Houses API

Added

- GET Houses
- GET House by Id
- POST House
- PUT House
- DELETE House

Added search

Added pagination

Added filtering

Reason:
Allow external clients such as Swagger and future Telegram Mini App to consume the backend.

--------------------------------------------

## Sprint 3

### Rental API

Added

- RentalDto
- Rental API
- Rental pagination
- Rental status filter
- Standard API responses

Reason:

Prepare backend services for Tenant Dashboard.

--------------------------------------------

## Sprint 4

### Dashboard API

Added

- DashboardDto
- Dashboard API
- Statistics endpoint

Dashboard returns

- Total Houses
- Available Houses
- Pending Rentals
- Approved Rentals

Reason:

Provide dashboard statistics for administrators.

---------------------------------------------

## Sprint 5

### Project Improvements

Added

- README
- API Documentation
- Swagger testing
- Better project structure
- DTO architecture