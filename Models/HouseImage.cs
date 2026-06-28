namespace house_renting.Models;

public class HouseImage
{
    public int HouseImageId { get; set; }
    public int HouseId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;

    public House? House { get; set; }
}
