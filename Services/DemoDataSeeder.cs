using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using house_renting.Data;
using house_renting.Models;

namespace house_renting.Services;

/// <summary>
/// Seeds the database with a realistic demo dataset: a pool of landlord accounts
/// and ~100 houses, each with a full 5-photo gallery and an assigned landlord.
/// Safe to run on every startup - it skips itself once houses already exist.
/// </summary>
public class DemoDataSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public DemoDataSeeder(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Each property (1-100) has its own folder under wwwroot/images/properties with
    // 4 real photos of one actual house: frontal (exterior), kitchen, bedroom, bathroom.
    private static readonly string[] PhotoCategories = { "frontal", "kitchen", "bedroom", "bathroom" };

    private static string PhotoUrl(int houseNumber, int slot) =>
        $"/images/properties/{houseNumber}/{PhotoCategories[slot]}.jpg";

    private static readonly string[] GalleryCaptions =
        { "Exterior View", "Kitchen", "Bedroom", "Bathroom", };

    private static readonly (string Type, int Weight)[] TypeWeights =
    {
        ("House", 30), ("Apartment", 25), ("Condo", 20), ("Villa", 15), ("Studio", 10)
    };

    private static readonly string[] Cities =
    {
        "Phnom Penh", "Siem Reap", "Battambang", "Sihanoukville",
        "Kampot", "Kep", "Kampong Cham", "Poipet", "Pursat", "Kratie"
    };

    private static readonly string[] StreetNames =
    {
        "St. 51", "St. 63", "St. 108", "St. 155", "St. 178", "St. 271",
        "St. 288", "St. 310", "St. 371", "Mao Tse Toung Blvd", "Norodom Blvd",
        "Monivong Blvd", "Sisowath Quay", "Riverside Rd", "Airport Rd"
    };

    private static readonly string[] Adjectives =
    {
        "Cozy", "Modern", "Spacious", "Charming", "Elegant", "Bright",
        "Peaceful", "Stylish", "Luxurious", "Sunny", "Newly Renovated", "Quiet"
    };

    private static readonly (string First, string Last)[] LandlordNames =
    {
        ("Sok", "Dara"), ("Chan", "Sopheak"), ("Ly", "Vannak"), ("Heng", "Sreymom"),
        ("Pich", "Chandara"), ("Kim", "Sreynich"), ("Vong", "Chenda"), ("Meas", "Sovanna"),
        ("Ros", "Pisey"), ("Chea", "Rithy"), ("Sam", "Oudom"), ("Nou", "Kunthea"),
        ("David", "Chen"), ("Sarah", "Thompson"), ("Michael", "Lee"), ("Ana", "Kim"),
        ("John", "Mitchell"), ("Emily", "Carter")
    };

    public async Task SeedAsync()
    {
        if (await _context.Houses.AnyAsync())
        {
            Console.WriteLine("Database already contains houses. Demo seeding skipped.");
            return;
        }

        var landlords = await EnsureLandlordsAsync();
        var random = new Random(42);

        for (int i = 0; i < 100; i++)
        {
            var houseType = PickWeightedType(random);
            var landlord = landlords[random.Next(landlords.Count)];
            var city = Cities[random.Next(Cities.Length)];
            var adjective = Adjectives[random.Next(Adjectives.Length)];
            var street = StreetNames[random.Next(StreetNames.Length)];
            var houseNumber = random.Next(1, 250);

            var (minPrice, maxPrice, minBed, maxBed, minBath, maxBath, minArea, maxArea) = houseType switch
            {
                "Studio" => (150, 350, 1, 1, 1, 1, 25, 45),
                "Apartment" => (250, 600, 1, 2, 1, 2, 45, 90),
                "Condo" => (400, 1200, 1, 3, 1, 2, 55, 120),
                "House" => (400, 1500, 2, 5, 1, 3, 90, 220),
                "Villa" => (900, 3000, 4, 7, 3, 5, 220, 500),
                _ => (300, 800, 1, 3, 1, 2, 50, 120)
            };

            var status = PickStatus(random);
            var propertyNumber = i + 1; // matches the properties/1 ... properties/100 folders
            var coverImage = PhotoUrl(propertyNumber, 0);

            var house = new House
            {
                LandlordId = landlord.Id,
                Title = $"{adjective} {houseType} in {city}",
                Description = BuildDescription(adjective, houseType, city, random),
                Address = $"#{houseNumber}, {street}",
                City = city,
                HouseType = houseType,
                MonthlyPrice = random.Next(minPrice, maxPrice + 1),
                AllowYearlyRental = random.NextDouble() > 0.15,
                YearlyDiscountPercent = random.Next(5, 21),
                Bedrooms = random.Next(minBed, maxBed + 1),
                Bathrooms = random.Next(minBath, maxBath + 1),
                Area = random.Next(minArea, maxArea + 1),
                Status = status,
                ImageUrl = coverImage,
                IsApproved = status != "Pending Approval",
                ApprovedAt = status != "Pending Approval" ? DateTime.Now.AddDays(-random.Next(1, 180)) : null,
                ApprovedBy = status != "Pending Approval" ? "admin@gmail.com" : null,
                CreatedAt = DateTime.Now.AddDays(-random.Next(1, 365))
            };

            _context.Houses.Add(house);
            await _context.SaveChangesAsync(); // get HouseId for gallery rows

            for (int g = 0; g < 4; g++)
            {
                _context.HouseImages.Add(new HouseImage
                {
                    HouseId = house.HouseId,
                    ImageUrl = PhotoUrl(propertyNumber, g),
                    Caption = GalleryCaptions[g]
                });
            }

            await _context.SaveChangesAsync();
        }

        Console.WriteLine();
        Console.WriteLine("====================================");
        Console.WriteLine($" Seeded {landlords.Count} landlords and 100 demo houses (4 photos each)");
        Console.WriteLine("====================================");
        Console.WriteLine();
    }

    private async Task<List<ApplicationUser>> EnsureLandlordsAsync()
    {
        var landlords = new List<ApplicationUser>();

        foreach (var (first, last) in LandlordNames)
        {
            var email = $"{first.ToLower()}.{last.ToLower()}@landlord.com";
            var existing = await _userManager.FindByEmailAsync(email);

            if (existing != null)
            {
                landlords.Add(existing);
                continue;
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FullName = $"{first} {last}",
                Role = "Landlord",
                CreatedAt = DateTime.Now.AddDays(-new Random().Next(30, 700))
            };

            var result = await _userManager.CreateAsync(user, "Landlord@123");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Landlord");
                landlords.Add(user);
            }
            else
            {
                Console.WriteLine($"Failed to create landlord {email}: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        return landlords;
    }

    private static string PickWeightedType(Random random)
    {
        var total = TypeWeights.Sum(t => t.Weight);
        var roll = random.Next(total);
        var cumulative = 0;

        foreach (var (type, weight) in TypeWeights)
        {
            cumulative += weight;
            if (roll < cumulative) return type;
        }

        return TypeWeights[0].Type;
    }

    private static string PickStatus(Random random)
    {
        var roll = random.NextDouble();
        if (roll < 0.75) return "Available";
        if (roll < 0.90) return "Rented";
        if (roll < 0.97) return "Pending Approval";
        return "Hidden";
    }

    private static string BuildDescription(string adjective, string houseType, string city, Random random)
    {
        var features = new[]
        {
            "close to local markets and restaurants",
            "with easy access to public transport",
            "in a quiet residential neighborhood",
            "near international schools",
            "with 24/7 security and parking",
            "just minutes from the city center",
            "with a private garden and balcony",
            "featuring modern furnishings throughout"
        };

        var feature = features[random.Next(features.Length)];
        return $"{adjective} {houseType.ToLower()} located in {city}, {feature}. " +
               "Perfect for families or professionals looking for a comfortable place to call home.";
    }
}