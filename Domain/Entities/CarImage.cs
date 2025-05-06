namespace Domain.Entities;

public class CarImage
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public string MainImage { get; set; }
    public string FirstSideImage { get; set; }
    public string SecondSideImage { get; set; }
    public string EngineImage { get; set; }
    public string SalonImage { get; set; }
}

