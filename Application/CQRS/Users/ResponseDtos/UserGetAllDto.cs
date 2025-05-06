using Domain.Entities;
using Domain.Enums;

namespace Application.CQRS.Users.ResponseDtos;

public class UserGetAllDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string UserRole { get; set; }
    public List<Car> UserCars { get; set; }
    public string UserImagePath { get; set; }
}
