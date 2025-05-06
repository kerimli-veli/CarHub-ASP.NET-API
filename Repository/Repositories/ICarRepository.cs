
using Domain.Entities;
using Domain.Enums;

namespace Repository.Repositories;

public interface ICarRepository
{
    Task AddAsync(Car car);
    void Update(Car car);
    Task Remove(int id);
    IQueryable<Car> GetAll();
    List<(int Id, string Name)> GetAllBodyTypes();
    Task<Car> GetByIdAsync(int id);

    //Dapper Operations
    Task<IEnumerable<Car>> GetFilteredCarsAsync(CarFilterModel filter);


}
