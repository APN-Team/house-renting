using house_renting.Data;
using house_renting.Models;
using Microsoft.EntityFrameworkCore;

namespace house_renting.Services;

public class CsvHouseImporter
{
    private readonly ApplicationDbContext _context;

    public CsvHouseImporter(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task ImportAsync(string csvPath)
    {
        // Check if CSV file exists
        if (!File.Exists(csvPath))
        {
            Console.WriteLine("CSV file not found.");
            return;
        }

        // Prevent importing duplicate data
        if (await _context.Houses.AnyAsync())
        {
            Console.WriteLine("Database already contains houses. Import skipped.");
            return;
        }

        var lines = await File.ReadAllLinesAsync(csvPath);

        if (lines.Length <= 1)
        {
            Console.WriteLine("CSV file is empty.");
            return;
        }

        int imported = 0;

        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var data = line.Split(',');

            // Expected columns:
            // Title,Description,Address,City,HouseType,
            // MonthlyPrice,AllowYearlyRental,
            // YearlyDiscountPercent,Bedrooms,
            // Bathrooms,Area,Status,ImageUrl

            if (data.Length < 13)
            {
                Console.WriteLine($"Skipped invalid row: {line}");
                continue;
            }

            try
            {
                var house = new House
                {
                    // Basic Information
                    Title = data[0].Trim(),
                    Description = data[1].Trim(),
                    Address = data[2].Trim(),
                    City = data[3].Trim(),
                    HouseType = data[4].Trim(),

                    // Pricing
                    MonthlyPrice = decimal.Parse(data[5]),
                    AllowYearlyRental = bool.Parse(data[6]),
                    YearlyDiscountPercent = decimal.Parse(data[7]),

                    // Details
                    Bedrooms = int.Parse(data[8]),
                    Bathrooms = int.Parse(data[9]),
                    Area = double.Parse(data[10]),

                    // Status
                    Status = data[11].Trim(),

                    // Thumbnail
                    ImageUrl = data[12].Trim(),

                    // Admin Approval
                    IsApproved = true,
                    ApprovedAt = DateTime.Now,

                    // Date
                    CreatedAt = DateTime.Now

                    // LandlordId intentionally left null.
                    // Demo houses imported from CSV are not owned by anyone.
                };

                _context.Houses.Add(house);
                imported++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to import row:");
                Console.WriteLine(line);
                Console.WriteLine(ex.Message);
            }
        }

        await _context.SaveChangesAsync();

        Console.WriteLine();
        Console.WriteLine("====================================");
        Console.WriteLine($" Successfully imported {imported} houses");
        Console.WriteLine("====================================");
        Console.WriteLine();
    }
}