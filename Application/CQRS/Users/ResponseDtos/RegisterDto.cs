using Microsoft.AspNetCore.Http;

namespace Application.CQRS.Users.ResponseDtos;

public class RegisterDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public IFormFile UserImage { get; set; }

}
