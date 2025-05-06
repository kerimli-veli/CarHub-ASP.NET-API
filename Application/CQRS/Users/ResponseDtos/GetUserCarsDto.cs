using Domain.Entities;
using Domain.Enums;

namespace Application.CQRS.Users.ResponseDtos;

public class GetUserCarsDto
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public int Price { get; set; }
    public string Fuel { get; set; }
    public string Transmission { get; set; }
    public double Miles { get; set; }
    public List<CarImage> CarImagePaths { get; set; }
    public string Body { get; set; }
    public string Color { get; set; }
    public string VIN { get; set; }
    public string Text { get; set; }
}
