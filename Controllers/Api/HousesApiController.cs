using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using house_renting.Data;

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

    // GET: api/houses?search=condo
    [HttpGet]
    public async Task<IActionResult> GetAll(string? search)
    {
        var query = _context.Houses
            .Where(h => h.Status == "Available")
            .AsQueryable();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(h =>
                h.Title.Contains(search) ||
                h.City.Contains(search) ||
                h.Address.Contains(search) ||
                h.HouseType.Contains(search) ||
                h.Description.Contains(search));

        var houses = await query
            .Select(h => new {
                h.HouseId,
                h.Title,
                h.Address,
                h.City,
                h.Price,
                h.HouseType,
                h.ImageUrl,
                h.Bedrooms,
                h.Bathrooms,
                h.Status
            })
            .Take(6)
            .ToListAsync();

        return Ok(houses);
    }

    // GET: api/houses/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var house = await _context.Houses
            .Where(h => h.HouseId == id)
            .Select(h => new {
                h.HouseId,
                h.Title,
                h.Description,
                h.Address,
                h.City,
                h.Price,
                h.HouseType,
                h.ImageUrl,
                h.Bedrooms,
                h.Bathrooms,
                h.Status
            })
            .FirstOrDefaultAsync();

        if (house == null) return NotFound();
        return Ok(house);
    }
}
