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