namespace house_renting.DTOs.Dashboard;

public class DashboardDto
{
    public int TotalHouses { get; set; }

    public int AvailableHouses { get; set; }

    public int RentedHouses { get; set; }

    public int TotalRentalRequests { get; set; }

    public int PendingRequests { get; set; }

    public int ApprovedRequests { get; set; }

    public int RejectedRequests { get; set; }

    public int TotalReviews { get; set; }

    public decimal AverageHousePrice { get; set; }
}