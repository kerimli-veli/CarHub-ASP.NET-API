using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.CQRS.Cars.ResponseDtos;

public class CarAddDto
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public int Price { get; set; }
    public FuelTypes Fuel { get; set; }
    public TransmissionTypes Transmission { get; set; }
    public double Miles { get; set; }
    public List<string> CarImagePaths { get; set; }
    public BodyTypes Body { get; set; }
    public string Color { get; set; }
    public string VIN { get; set; }
    public string Text { get; set; }

}
