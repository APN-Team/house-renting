namespace house_renting.DTOs.Dashboard;

/// <summary>
/// Landlord statistics shown on the Admin Dashboard
/// (how many properties each landlord owns on the platform).
/// </summary>
public class LandlordStatsDto
{
    public string Id { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? ProfileImage { get; set; }

    public DateTime CreatedAt { get; set; }

    public int HouseCount { get; set; }

    public int AvailableCount { get; set; }

    public int PendingCount { get; set; }
}
