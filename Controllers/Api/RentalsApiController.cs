using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using house_renting.Data;
using house_renting.DTOs.Common;
using house_renting.DTOs.Rental;

namespace house_renting.Controllers.Api;

[ApiController]
[Route("api/rentals")]
public class RentalsApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RentalsApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/rentals
    /// <summary>
    /// Get rental requests.
    /// </summary>
    /// <param name="status">
    /// Optional status filter.
    /// </param>
    /// <param name="page">
    /// Current page.
    /// </param>
    /// <param name="pageSize">
    /// Items per page.
    /// </param>
    /// <returns>
    /// Rental request list.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        string? status,
        int page = 1,
        int pageSize = 10)
    {
        var query = _context.RentalRequests.AsQueryable();

        // Filter by rental status
        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(r => r.Status == status);
        }

        var totalRecords = await query.CountAsync();

        var rentals = await query
            .OrderByDescending(r => r.RequestDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new RentalDto
            {
                RequestId = r.RequestId,
                HouseId = r.HouseId,
                TenantId = r.TenantId,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                Status = r.Status,
                RequestDate = r.RequestDate
            })
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Rental requests retrieved successfully.",
            Data = new
            {
                TotalRecords = totalRecords,
                CurrentPage = page,
                PageSize = pageSize,
                Rentals = rentals
            }
        });
    }
}