namespace house_renting.Models;

public class House
{
    public int HouseId { get; set; }
    public string LandlordId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public string Status { get; set; } = "Available";
    public string HouseType { get; set; } = "House";
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ApplicationUser? Landlord { get; set; }
    public ICollection<RentalRequest> RentalRequests { get; set; } = new List<RentalRequest>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}