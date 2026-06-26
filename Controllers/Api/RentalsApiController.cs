using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using house_renting.Data;

namespace house_renting.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class RentalsApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RentalsApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/rentals
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var requests = await _context.RentalRequests
            .Select(r => new {
                r.RequestId,
                r.HouseId,
                r.TenantId,
                r.StartDate,
                r.EndDate,
                r.Status,
                r.RequestDate
            })
            .ToListAsync();
        return Ok(requests);
    }
}
