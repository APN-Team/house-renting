namespace house_renting.Models;

public class Payment
{
    public int PaymentId { get; set; }
    public string TenantId { get; set; } = string.Empty;
    public int HouseId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.Now;
    public string Status { get; set; } = "Pending";
    public string TransactionNumber { get; set; } = string.Empty;

    public ApplicationUser? Tenant { get; set; }
    public House? House { get; set; }
}

