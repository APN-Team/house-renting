using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using house_renting.Data;
using house_renting.Models;

namespace house_renting.Controllers;

public class ReviewController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReviewController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: /Review/Create/5 (HouseId)
    [Authorize(Roles = "Tenant")]
    public async Task<IActionResult> Create(int id)
    {
        var house = await _context.Houses.FindAsync(id);
        if (house == null) return NotFound();
        ViewBag.House = house;
        return View();
    }

    // POST: /Review/Create/5
    [Authorize(Roles = "Tenant")]
    [HttpPost]
    public async Task<IActionResult> Create(int id, int rating, string comment)
    {
        var user = await _userManager.GetUserAsync(User);

        var review = new Review
        {
            HouseId = id,
            TenantId = user!.Id,
            Rating = rating,
            Comment = comment,
            CreatedAt = DateTime.Now
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
        return RedirectToAction("Details", "House", new { id });
    }
}
