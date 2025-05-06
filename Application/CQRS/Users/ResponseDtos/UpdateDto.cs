namespace Application.CQRS.Users.ResponseDtos;

public class UpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string UserImagePath { get; set; }
    //public string PasswordHash { get; set; }
}
