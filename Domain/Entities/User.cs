using Domain.BaseEntities;
using Domain.Enums;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string PasswordHash { get; set; }
    public UserRoles UserRole { get; set; }
    public List<UserFavorite> Favorites { get; set; } = new List<UserFavorite>();  
    public List<Car> UserCars { get; set; } = new List<Car>();
    public string UserImagePath { get; set; }
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

}
