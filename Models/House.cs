namespace house_renting.Models;

public class House
{
    public int HouseId { get; set; }

    // Owner
    public string? LandlordId { get; set; }
    public ApplicationUser? Landlord { get; set; }

    // Basic Information
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;

    // House / Villa / Condo / Apartment / Studio
    public string HouseType { get; set; } = "House";

    // Rental Pricing
    public decimal MonthlyPrice { get; set; }

    // Can tenants rent yearly?
    public bool AllowYearlyRental { get; set; } = true;

    // 5 - 30 (%)
    public decimal YearlyDiscountPercent { get; set; } = 10;

    // House Details
    public int Bedrooms { get; set; }

    public int Bathrooms { get; set; }

    // Square meters
    public double Area { get; set; }

    // Available / Pending Approval / Rented / Hidden
    public string Status { get; set; } = "Pending Approval";

    // Main thumbnail
    public string? ImageUrl { get; set; }

    // Dates
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Admin Approval
    public bool IsApproved { get; set; } = false;

    public DateTime? ApprovedAt { get; set; }

    public string? ApprovedBy { get; set; }

    // Navigation Properties
    public ICollection<RentalRequest> RentalRequests { get; set; } = new List<RentalRequest>();

    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    public ICollection<HouseImage> HouseImages { get; set; } = new List<HouseImage>();


    // =============================
    // Computed Property
    // =============================

    public decimal YearlyPrice
    {
        get
        {
            var total = MonthlyPrice * 12;
            var discount = total * (YearlyDiscountPercent / 100);
            return total - discount;
        }
    }
}