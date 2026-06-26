namespace house_renting.Models;

public class Review
{
    public int ReviewId { get; set; }
    public string TenantId { get; set; } = string.Empty;
    public int HouseId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ApplicationUser? Tenant { get; set; }
    public House? House { get; set; }
}