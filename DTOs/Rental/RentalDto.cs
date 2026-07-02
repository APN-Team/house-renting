namespace house_renting.DTOs.Rental;

public class RentalDto
{
    public int RequestId { get; set; }

    public int HouseId { get; set; }

    public string TenantId { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime RequestDate { get; set; }
}