using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using house_renting.Data;
using house_renting.Models;

namespace house_renting.Controllers;

public class HouseController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _env;

    public HouseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
    {
        _context = context;
        _userManager = userManager;
        _env = env;
    }

    // GET: /House
   public async Task<IActionResult> Index(string? type, string? city, string? search)
    {
        var query = _context.Houses
            .Where(h => h.Status == "Available")
            .Include(h => h.Landlord)
            .AsQueryable();

        if (!string.IsNullOrEmpty(type))
            query = query.Where(h => h.HouseType == type);

        if (!string.IsNullOrEmpty(city))
            query = query.Where(h => h.City == city);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(h =>
                h.Title.Contains(search) ||
                h.City.Contains(search) ||
                h.Address.Contains(search) ||
                h.HouseType.Contains(search) ||
                h.Description.Contains(search));

        var cities = await _context.Houses
            .Where(h => h.Status == "Available")
            .Select(h => h.City)
            .Distinct()
            .ToListAsync();

        ViewBag.CurrentType = type;
        ViewBag.CurrentCity = city;
        ViewBag.CurrentSearch = search;
        ViewBag.Cities = cities;

        return View(await query.ToListAsync());
    }

    // GET: /House/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var house = await _context.Houses
            .Include(h => h.Landlord)
            .Include(h => h.Reviews)
            .Include(h => h.HouseImages)
            .FirstOrDefaultAsync(h => h.HouseId == id);

        if (house == null) return NotFound();
        return View(house);
    }

    // GET: /House/Create
    [Authorize(Roles = "Landlord")]
    public IActionResult Create() => View();

    // POST: /House/Create
    [Authorize(Roles = "Landlord")]
    [HttpPost]
    public async Task<IActionResult> Create(House house, IFormFile? imageFile, IFormFileCollection? galleryFiles)
    {
        var user = await _userManager.GetUserAsync(User);
        house.LandlordId = user!.Id;
        house.Status = "Available";
        house.CreatedAt = DateTime.Now;

        if (imageFile != null && imageFile.Length > 0)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);
            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);
            house.ImageUrl = "/uploads/" + fileName;
        }

        _context.Houses.Add(house);
        await _context.SaveChangesAsync();

        if (galleryFiles != null && galleryFiles.Count > 0)
        {
            foreach (var file in galleryFiles)
            {
                if (file.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder);
                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);

                    _context.HouseImages.Add(new HouseImage
                    {
                        HouseId = house.HouseId,
                        ImageUrl = "/uploads/" + fileName,
                        Caption = file.FileName
                    });
                }
            }
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: /House/Edit/5
    [Authorize(Roles = "Landlord")]
    public async Task<IActionResult> Edit(int id)
    {
        var house = await _context.Houses.FindAsync(id);
        if (house == null) return NotFound();
        return View(house);
    }

    // POST: /House/Edit/5
    [Authorize(Roles = "Landlord")]
    [HttpPost]
    public async Task<IActionResult> Edit(House house, IFormFile? imageFile)
    {
        if (imageFile != null && imageFile.Length > 0)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);
            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);
            house.ImageUrl = "/uploads/" + fileName;
        }

        _context.Houses.Update(house);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // POST: /House/Delete/5
    [Authorize(Roles = "Landlord")]
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var house = await _context.Houses.FindAsync(id);
        if (house != null)
        {
            _context.Houses.Remove(house);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
