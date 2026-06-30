using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using house_renting.Models;

namespace house_renting.Controllers;

public class AccountController : Controller
{
   private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IWebHostEnvironment _env;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IWebHostEnvironment env)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _env = env;
    }

    // GET: /Account/Register
    public IActionResult Register() => View();

    // POST: /Account/Register
    [HttpPost]
    public async Task<IActionResult> Register(string fullName, string email, string password, string role)
    {
        var user = new ApplicationUser
        {
            FullName = fullName,
            Email = email,
            UserName = email,
            Role = role,
            CreatedAt = DateTime.Now
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, role);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View();
    }

    // GET: /Account/Login
    public IActionResult Login() => View();

    // POST: /Account/Login
    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, false, false);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && !string.IsNullOrEmpty(user.ProfileImage))
            {
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("ProfileImage", user.ProfileImage));
            }
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Invalid email or password");
        return View();
    }

    // POST: /Account/Logout
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    // GET: /Account/Profile
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        return View(user);
    }
    // GET: /Account/EditProfile
    [Authorize]
    public async Task<IActionResult> EditProfile()
    {
        var user = await _userManager.GetUserAsync(User);
        return View(user);
    }

    // POST: /Account/EditProfile
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> EditProfile(string fullName, IFormFile? profileImage)
    {
        var user = await _userManager.GetUserAsync(User);
        user!.FullName = fullName;

        if (profileImage != null && profileImage.Length > 0)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);
            var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await profileImage.CopyToAsync(stream);
            user.ProfileImage = "/uploads/" + fileName;

            // Update claim
            var existingClaim = (await _userManager.GetClaimsAsync(user))
                .FirstOrDefault(c => c.Type == "ProfileImage");
            if (existingClaim != null)
                await _userManager.RemoveClaimAsync(user, existingClaim);
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("ProfileImage", user.ProfileImage));
        }

        await _userManager.UpdateAsync(user);
        await _signInManager.RefreshSignInAsync(user);
        return RedirectToAction("Profile");
    }
}
