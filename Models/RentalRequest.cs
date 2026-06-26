using System.ComponentModel.DataAnnotations;

namespace house_renting.Models;

public class RentalRequest
{
    [Key]
    public int RequestId { get; set; }
    public string TenantId { get; set; } = string.Empty;
    public int HouseId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime RequestDate { get; set; } = DateTime.Now;
    public string Status { get; set; } = "Pending";
    public string Message { get; set; } = string.Empty;

    public ApplicationUser? Tenant { get; set; }
    public House? House { get; set; }
}
