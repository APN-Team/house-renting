using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using house_renting.Data;
using house_renting.Models;

namespace house_renting.Controllers;

public class RentalController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public RentalController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: /Rental/Create/5 - Tenant requests to rent a house
    [Authorize(Roles = "Tenant")]
    public async Task<IActionResult> Create(int id)
    {
        var house = await _context.Houses.FindAsync(id);
        if (house == null) return NotFound();
        ViewBag.House = house;
        return View();
    }

    // POST: /Rental/Create/5
    [Authorize(Roles = "Tenant")]
    [HttpPost]
    public async Task<IActionResult> Create(int id, DateTime startDate, DateTime endDate, string message)
    {
        var user = await _userManager.GetUserAsync(User);

        var request = new RentalRequest
        {
            TenantId = user!.Id,
            HouseId = id,
            StartDate = startDate,
            EndDate = endDate,
            RequestDate = DateTime.Now,
            Status = "Pending",
            Message = message
        };

        _context.RentalRequests.Add(request);
        await _context.SaveChangesAsync();
        return RedirectToAction("MyRequests");
    }

    // GET: /Rental/MyRequests - Tenant sees their requests
    [Authorize(Roles = "Tenant")]
    public async Task<IActionResult> MyRequests()
    {
        var user = await _userManager.GetUserAsync(User);
        var requests = await _context.RentalRequests
            .Where(r => r.TenantId == user!.Id)
            .Include(r => r.House)
            .ToListAsync();
        return View(requests);
    }

    // GET: /Rental/Manage - Landlord sees requests for their houses
    [Authorize(Roles = "Landlord")]
    public async Task<IActionResult> Manage()
    {
        var user = await _userManager.GetUserAsync(User);
        var requests = await _context.RentalRequests
            .Where(r => r.House!.LandlordId == user!.Id)
            .Include(r => r.House)
            .Include(r => r.Tenant)
            .ToListAsync();
        return View(requests);
    }

    // POST: /Rental/Approve/5
    [Authorize(Roles = "Landlord")]
    [HttpPost]
    public async Task<IActionResult> Approve(int id)
    {
        var request = await _context.RentalRequests.FindAsync(id);
        if (request != null)
        {
            request.Status = "Approved";
            var house = await _context.Houses.FindAsync(request.HouseId);
            if (house != null) house.Status = "Rented";
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Manage");
    }

    // POST: /Rental/Reject/5
    [Authorize(Roles = "Landlord")]
    [HttpPost]
    public async Task<IActionResult> Reject(int id)
    {
        var request = await _context.RentalRequests.FindAsync(id);
        if (request != null)
        {
            request.Status = "Rejected";
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Manage");
    }
}
