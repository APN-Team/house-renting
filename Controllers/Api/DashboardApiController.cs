using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using house_renting.Data;
using house_renting.DTOs.Common;
using house_renting.DTOs.Dashboard;

namespace house_renting.Controllers.Api;

/// <summary>
/// Get dashboard statistics.
/// </summary>
/// <returns>
/// Houses, rentals, users and reviews summary.
/// </returns>

[ApiController]
[Route("api/dashboard")]
public class DashboardApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DashboardApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboard()
    {
        var dashboard = new DashboardDto
        {
            TotalHouses = await _context.Houses.CountAsync(),

            AvailableHouses = await _context.Houses
                .CountAsync(h => h.Status == "Available"),

            RentedHouses = await _context.Houses
                .CountAsync(h => h.Status != "Available"),

            TotalRentalRequests = await _context.RentalRequests.CountAsync(),

            PendingRequests = await _context.RentalRequests
                .CountAsync(r => r.Status == "Pending"),

            ApprovedRequests = await _context.RentalRequests
                .CountAsync(r => r.Status == "Approved"),

            RejectedRequests = await _context.RentalRequests
                .CountAsync(r => r.Status == "Rejected"),

            TotalReviews = await _context.Reviews.CountAsync(),

            AverageHousePrice = await _context.Houses.AnyAsync()
                ? await _context.Houses.AverageAsync(h => h.Price)
                : 0
        };

        return Ok(new ApiResponse<DashboardDto>
        {
            Success = true,
            Message = "Dashboard loaded successfully.",
            Data = dashboard
        });
    }
}