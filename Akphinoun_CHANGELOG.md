# House Renting Project Changelog

## Sprint 1 - API Foundation

### Added

- DTO folder
- ApiResponse
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

--------------------------------------------

## Sprint 5

### Project Improvements

Added

- README
- API Documentation
- Swagger testing
- Better project structure
- DTO architecture

--------------------------------------------

## Sprint 6

### Home Page UI Redesign

Implemented

- Redesigned Home Page layout
- Added full-screen Hero Section
- Added modern background image
- Added dark overlay for better text readability
- Improved responsive navigation bar
- Redesigned Featured Properties section
- Improved spacing and typography
- Added modern call-to-action buttons
- Improved footer layout

Reason:

Modernize the landing page and improve the overall user experience.

--------------------------------------------

## Sprint 7

### Property Card Improvements

Implemented

- Redesigned property cards
- Added hover animation
- Added rounded corners
- Improved property image display
- Added status badges
- Improved price section
- Improved responsive grid layout

Reason:

Provide a cleaner and more professional property browsing experience.

--------------------------------------------

## Sprint 8

### Dashboard & Admin Improvements

Implemented

- Improved dashboard statistics layout
- Redesigned landlord dashboard
- Improved admin house management page
- Better table organization
- Improved dashboard cards
- Better responsive layout

Reason:

Improve usability for landlords and administrators.

--------------------------------------------

## Sprint 9

### Backend Improvements

Implemented

- Refactored API responses
- Added Dashboard DTO
- Added LandlordStats DTO
- Improved API consistency
- Improved controller organization
- Improved service structure
- Better JSON response formatting

Reason:

Improve backend maintainability and prepare the project for future frontend integration.

--------------------------------------------

## Sprint 10

### Demo Data Workflow

Implemented

- Automatic demo data generation
- Demo landlord generation
- Demo house generation
- Demo property image generation
- Random property information generation
- Optional CSV import fallback
- Automatic database seeding on application startup

Workflow

Application Start

↓

Program.cs

↓

DemoDataSeeder

↓

Generate Demo Data

↓

If database is empty

↓

CsvHouseImporter

↓

SeedData/houses.csv

Reason:

Allow the application to be tested immediately without manually inserting sample data.

--------------------------------------------

## Sprint 11

### API Documentation

Added

- Updated README
- API documentation
- API Controller Notes
- Swagger testing documentation
- Development changelog

Reason:

Improve project documentation and make the backend easier to understand.

--------------------------------------------

## Sprint 12

### Final Project Refinement

Implemented

- Removed unused DTO files
- Cleaned project folder structure
- Removed unnecessary development files
- Improved code organization
- Updated project documentation
- Final UI polishing
- Responsive layout improvements
- General bug fixes

Reason:

Prepare the project for final presentation and submission.

--------------------------------------------

