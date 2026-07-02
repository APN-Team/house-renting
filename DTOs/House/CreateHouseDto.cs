using System.ComponentModel.DataAnnotations;

namespace house_renting.DTOs.House;

public class CreateHouseDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Range(1, 100000)]
    public decimal Price { get; set; }

    [Range(1, 20)]
    public int Bedrooms { get; set; }

    [Range(1, 20)]
    public int Bathrooms { get; set; }

    public string HouseType { get; set; } = "House";

    public string? ImageUrl { get; set; }
}