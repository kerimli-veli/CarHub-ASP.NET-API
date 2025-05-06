using Domain.BaseEntities;
using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities;

public class Car : BaseEntity
{
    public string Brand { get; set; }
    public string BrandImagePath { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public int Price { get; set; }
    public FuelTypes Fuel { get; set; }
    public TransmissionTypes Transmission { get; set; }
    public double Miles { get; set; }
    public List<CarImage> CarImagePaths { get; set; } = new List<CarImage>();
    public BodyTypes Body { get; set; }
    public string Color { get; set; }
    public string VIN { get; set; }
    public string Text { get; set; }
    public List<UserFavorite> FavoritedByUsers { get; set; } = new List<UserFavorite>();
    public int UserId { get; set; }
    public User User { get; set; }

    public Car()
    {
        BrandImagePath = "None";
    }
}
