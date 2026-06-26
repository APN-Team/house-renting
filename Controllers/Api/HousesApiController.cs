using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using house_renting.Data;

namespace house_renting.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class HousesApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public HousesApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/houses
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var houses = await _context.Houses
            .Where(h => h.Status == "Available")
            .Select(h => new {
                h.HouseId,
                h.Title,
                h.Address,
                h.City,
                h.Price,
                h.Bedrooms,
                h.Bathrooms,
                h.Status
            })
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
                h.Bedrooms,
                h.Bathrooms,
                h.Status
            })
            .FirstOrDefaultAsync();

        if (house == null) return NotFound();
        return Ok(house);
    }
}
