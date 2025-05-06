namespace Repository.Repositories;

public interface IFavoriteRepository
{
    Task AddFavoriteCarAsync(int userId, int carId);
    Task RemoveFavoriteCarAsync(int userId, int carId);
}
