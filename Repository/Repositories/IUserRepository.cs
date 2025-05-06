using Domain.Entities;

namespace Repository.Repositories;

public interface IUserRepository
{
    Task RegisterAsync(User user);
    void Update(User user);
    Task Remove(int id);
    IQueryable<User> GetAll();
    Task<User> GetByIdAsync(int id);
    Task<User> GetUserByEmailAsync(string email);
    Task<List<Car>> GetUserFavoritesAsync(int userId);
    Task<List<Car>> GetUserCarAsync(int userId);
  
}
