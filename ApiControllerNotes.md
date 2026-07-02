### HousesApiController Improvements

Original:
- Returned anonymous objects
- Hardcoded Take(6)

Improved:
- Uses HouseDto
- Uses ApiResponse<T>
- Supports pagination
- Supports filtering
- Better Swagger documentation

Reason:
To make the API reusable, consistent, and easier for future clients such as a Telegram Mini App or other frontend applications.