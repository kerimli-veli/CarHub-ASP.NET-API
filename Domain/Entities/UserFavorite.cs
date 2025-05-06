namespace Domain.Entities;


public class UserFavorite
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

    public int CarId { get; set; }
    public Car Car { get; set; }
}
