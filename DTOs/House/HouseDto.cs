namespace house_renting.DTOs.House;

public class HouseDto
{
    public int HouseId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public decimal Price { get; set; }

    // Yearly rental info (used by frontend to show discounted yearly price)
    public bool AllowYearlyRental { get; set; }

    public decimal YearlyDiscountPercent { get; set; }

    public decimal YearlyPrice { get; set; }

    public int Bedrooms { get; set; }

    public int Bathrooms { get; set; }

    public double Area { get; set; }

    public string Status { get; set; } = string.Empty;

    public string HouseType { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? LandlordName { get; set; }
}