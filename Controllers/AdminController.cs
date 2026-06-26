using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using house_renting.Data;
using house_renting.Models;

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
        return View();
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
