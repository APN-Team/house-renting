# API Controller Notes

## Why DTO?

DTO (Data Transfer Object) is used to separate database models from API responses.

Advantages

- Hide sensitive fields
- Reduce payload size
- Better maintainability
- Easier frontend integration

---

## Standard API Response

Every API should return

```

Success
Message
Data

```

instead of raw Entity Framework objects.

---

## Pagination

Large datasets should be returned page by page.

Example

```

GET /api/houses?page=1&pageSize=10

```

---

## Filtering

Supported filters

- Search
- City
- House Type
- Price Range
- Bedrooms
- Bathrooms

---

## API Testing

Swagger

```

/swagger

```

can be used for testing every endpoint.