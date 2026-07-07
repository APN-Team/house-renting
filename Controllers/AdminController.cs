using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using house_renting.Data;
using house_renting.Models;
using house_renting.DTOs.Dashboard;

namespace house_renting.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: /Admin
    public async Task<IActionResult> Index()
    {
        ViewBag.TotalUsers = await _userManager.Users.CountAsync();
        ViewBag.TotalHouses = await _context.Houses.CountAsync();
        ViewBag.TotalRequests = await _context.RentalRequests.CountAsync();
        ViewBag.PendingRequests = await _context.RentalRequests.CountAsync(r => r.Status == "Pending");
        ViewBag.PendingApprovals = await _context.Houses.CountAsync(h => !h.IsApproved);

        // ── House type distribution (for the dashboard chart) ──
        ViewBag.HouseTypeStats = await _context.Houses
            .GroupBy(h => h.HouseType)
            .Select(g => new { Type = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .ToDictionaryAsync(g => g.Type, g => g.Count);

        // ── Landlord management: every landlord + how many properties they own,
        //    sorted so the landlord with the MOST properties is on top ──
        var landlordUsers = await _userManager.GetUsersInRoleAsync("Landlord");

        var houseCounts = await _context.Houses
            .Where(h => h.LandlordId != null)
            .GroupBy(h => h.LandlordId!)
            .Select(g => new
            {
                LandlordId = g.Key,
                Total = g.Count(),
                Available = g.Count(h => h.Status == "Available"),
                Pending = g.Count(h => !h.IsApproved)
            })
            .ToDictionaryAsync(g => g.LandlordId);

        var landlordStats = landlordUsers
            .Select(l => new LandlordStatsDto
            {
                Id = l.Id,
                FullName = l.FullName,
                Email = l.Email,
                ProfileImage = l.ProfileImage,
                CreatedAt = l.CreatedAt,
                HouseCount = houseCounts.TryGetValue(l.Id, out var c) ? c.Total : 0,
                AvailableCount = houseCounts.TryGetValue(l.Id, out var a) ? a.Available : 0,
                PendingCount = houseCounts.TryGetValue(l.Id, out var pnd) ? pnd.Pending : 0
            })
            .OrderByDescending(l => l.HouseCount)
            .ThenBy(l => l.FullName)
            .ToList();

        ViewBag.TotalLandlords = landlordStats.Count;
        ViewBag.TopLandlord = landlordStats.FirstOrDefault(l => l.HouseCount > 0);

        return View(landlordStats);
    }

    // GET: /Admin/LandlordHouses/{id}
    // Shows exactly which properties a specific landlord owns.
    public async Task<IActionResult> LandlordHouses(string id)
    {
        var landlord = await _userManager.FindByIdAsync(id);
        if (landlord == null) return NotFound();

        var houses = await _context.Houses
            .Where(h => h.LandlordId == id)
            .OrderByDescending(h => h.CreatedAt)
            .ToListAsync();

        ViewBag.Landlord = landlord;
        return View(houses);
    }

    // GET: /Admin/Users
    public async Task<IActionResult> Users()
    {
        var users = await _userManager.Users.ToListAsync();
        return View(users);
    }

    // POST: /Admin/DeleteUser
    [HttpPost]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
            await _userManager.DeleteAsync(user);
        return RedirectToAction("Users");
    }

    // GET: /Admin/Houses
    public async Task<IActionResult> Houses()
    {
        var houses = await _context.Houses
            .Include(h => h.Landlord)
            .ToListAsync();
        return View(houses);
    }

    // POST: /Admin/DeleteHouse
    [HttpPost]
    public async Task<IActionResult> DeleteHouse(int id)
    {
        var house = await _context.Houses.FindAsync(id);
        if (house != null)
        {
            _context.Houses.Remove(house);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Houses");
    }

    // GET: /Admin/Requests
    public async Task<IActionResult> Requests()
    {
        var requests = await _context.RentalRequests
            .Include(r => r.House)
            .Include(r => r.Tenant)
            .ToListAsync();
        return View(requests);
    }
}
