using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using house_renting.Data;
using house_renting.Models;
using house_renting.DTOs.Common;
using house_renting.DTOs.House;

namespace house_renting.Controllers.Api;

[ApiController]
[Route("api/houses")]
public class HousesApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public HousesApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all available houses.
    /// </summary>
    /// <remarks>
    /// Supports:
    /// - Search
    /// - City filter
    /// - Price filter
    /// - Bedrooms filter
    /// - Bathrooms filter
    /// - House type filter
    /// - Pagination
    /// </remarks>
    /// <param name="search">Search by title, city, address or description.</param>
    /// <param name="city">Filter by city.</param>
    /// <param name="minPrice">Minimum rental price.</param>
    /// <param name="maxPrice">Maximum rental price.</param>
    /// <param name="bedrooms">Minimum bedrooms.</param>
    /// <param name="bathrooms">Minimum bathrooms.</param>
    /// <param name="houseType">House type.</param>
    /// <param name="page">Current page.</param>
    /// <param name="pageSize">Items per page.</param>
    /// <returns>List of available houses.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        string? search,
        string? city,
        decimal? minPrice,
        decimal? maxPrice,
        int? bedrooms,
        int? bathrooms,
        string? houseType,
        int page = 1,
        int pageSize = 10)
    {
        var query = _context.Houses
            .Include(h => h.Landlord)
            .Where(h => h.Status == "Available")
            .AsQueryable();

        // Search
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(h =>
                h.Title.Contains(search) ||
                h.City.Contains(search) ||
                h.Address.Contains(search) ||
                h.Description.Contains(search) ||
                h.HouseType.Contains(search));
        }

        // Filters
        if (!string.IsNullOrWhiteSpace(city))
            query = query.Where(h => h.City == city);

        if (minPrice.HasValue)
            query = query.Where(h => h.MonthlyPrice >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(h => h.MonthlyPrice <= maxPrice.Value);

        if (bedrooms.HasValue)
            query = query.Where(h => h.Bedrooms == bedrooms.Value);

        if (bathrooms.HasValue)
            query = query.Where(h => h.Bathrooms == bathrooms.Value);

        if (!string.IsNullOrWhiteSpace(houseType))
            query = query.Where(h => h.HouseType == houseType);

        var totalRecords = await query.CountAsync();

        var houses = await query
            .OrderByDescending(h => h.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(h => new HouseDto
            {
                HouseId = h.HouseId,
                Title = h.Title,
                Description = h.Description,
                Address = h.Address,
                City = h.City,
                Price = h.MonthlyPrice,
                Bedrooms = h.Bedrooms,
                Bathrooms = h.Bathrooms,
                Status = h.Status,
                HouseType = h.HouseType,
                ImageUrl = h.ImageUrl,
                CreatedAt = h.CreatedAt,
                LandlordName = h.Landlord != null
                    ? h.Landlord.FullName
                    : null
            })
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Houses retrieved successfully.",
            Data = new
            {
                TotalRecords = totalRecords,
                CurrentPage = page,
                PageSize = pageSize,
                Houses = houses
            }
        });
    }

    // GET: api/houses/5
    /// <summary>
    /// Get a single house.
    /// </summary>
    /// <param name="id">House ID.</param>
    /// <returns>House details.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var house = await _context.Houses
            .Include(h => h.Landlord)
            .Where(h => h.HouseId == id)
            .Select(h => new HouseDto
            {
                HouseId = h.HouseId,
                Title = h.Title,
                Description = h.Description,
                Address = h.Address,
                City = h.City,
                Price = h.MonthlyPrice,
                Bedrooms = h.Bedrooms,
                Bathrooms = h.Bathrooms,
                Status = h.Status,
                HouseType = h.HouseType,
                ImageUrl = h.ImageUrl,
                CreatedAt = h.CreatedAt,
                LandlordName = h.Landlord != null
                    ? h.Landlord.FullName
                    : null
            })
            .FirstOrDefaultAsync();

        if (house == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "House not found."
            });
        }

        return Ok(new ApiResponse<HouseDto>
        {
            Success = true,
            Message = "House retrieved successfully.",
            Data = house
        });
    }
}