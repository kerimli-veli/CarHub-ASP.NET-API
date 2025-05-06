using Domain.Enums;

namespace Domain.Entities;

public class CarFilterModel
{
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int? MinYear { get; set; }
    public int? MaxYear { get; set; }
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public FuelTypes Fuel { get; set; }
    public TransmissionTypes Transmission { get; set; }
    public double? MinMiles { get; set; }
    public double? MaxMiles { get; set; }
    public BodyTypes Body { get; set; }
    public string? Color { get; set; }
}

