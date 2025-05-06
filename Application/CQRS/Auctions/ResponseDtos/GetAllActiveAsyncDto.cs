using Application.CQRS.Cars.ResponseDtos;

namespace Application.CQRS.Auctions.ResponseDtos;

public class GetAllActiveAsyncDto
{
    public int Id { get; set; }
    public CarGetByIdDto Car { get; set; }
    public decimal StartingPrice { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsActive { get; set; }
}
