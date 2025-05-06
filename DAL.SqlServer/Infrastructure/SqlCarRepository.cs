using DAL.SqlServer.Context;
using Dapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infrastructure;

public class SqlCarRepository(string connectionString, AppDbContext context) : BaseSqlRepository(connectionString), ICarRepository
{
    private readonly AppDbContext _context = context;
    public async Task AddAsync(Car car)
    {
        await _context.Cars.AddAsync(car);
        await _context.SaveChangesAsync();
    }

    public async Task Remove(int id)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(u => u.Id == id);
        car.IsDeleted = true;
        car.DeletedDate = DateTime.Now;
        car.DeletedBy = 0;
    }

    public void Update(Car car)
    {
        var cars = _context.Cars.ToList();
        car.UpdatedDate = DateTime.Now;
        _context.Update(car);
        _context.SaveChanges();
    }

    public IQueryable<Car> GetAll()
    {
        return _context.Cars
            .Where(c => !c.IsDeleted)
            .Include(c => c.CarImagePaths);
    }

    public async Task<Car> GetByIdAsync(int id)
    {
        return (await _context.Cars.Where(c => !c.IsDeleted).
            Include(x => x.CarImagePaths).FirstOrDefaultAsync(u => u.Id == id));
    }

    public async Task<IEnumerable<Car>> GetFilteredCarsAsync(CarFilterModel filter)
    {
        await using var connection = OpenConnection();

        var sql = @"
    SELECT c.*, ci.* 
    FROM Cars c
    LEFT JOIN CarImage ci ON ci.CarId = c.Id
    WHERE 1=1";

        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(filter.Brand))
        {
            sql += " AND c.Brand = @Brand";
            parameters.Add("Brand", filter.Brand);
        }

        if (!string.IsNullOrWhiteSpace(filter.Model))
        {
            sql += " AND LOWER(c.Model) LIKE LOWER(@Model)";
            parameters.Add("Model", filter.Model.ToLower() + "%");
        }

        if (filter.MinYear.HasValue)
        {
            sql += " AND c.Year >= @MinYear";
            parameters.Add("MinYear", filter.MinYear.Value);
        }

        if (filter.MaxYear.HasValue)
        {
            sql += " AND c.Year <= @MaxYear";
            parameters.Add("MaxYear", filter.MaxYear.Value);
        }

        if (filter.MinPrice.HasValue)
        {
            sql += " AND c.Price >= @MinPrice";
            parameters.Add("MinPrice", filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            sql += " AND c.Price <= @MaxPrice";
            parameters.Add("MaxPrice", filter.MaxPrice.Value);
        }


        if (filter.Fuel != 0)
        {
            sql += " AND c.Fuel = @Fuel";
            parameters.Add("Fuel", (int)filter.Fuel);
        }

        if (filter.Transmission != 0)
        {
            sql += " AND c.Transmission = @Transmission";
            parameters.Add("Transmission", (int)filter.Transmission);
        }

        if (filter.MinMiles.HasValue)
        {
            sql += " AND c.Miles >= @MinMiles";
            parameters.Add("MinMiles", filter.MinMiles.Value);
        }

        if (filter.MaxMiles.HasValue)
        {
            sql += " AND c.Miles <= @MaxMiles";
            parameters.Add("MaxMiles", filter.MaxMiles.Value);
        }


        if (filter.Body != 0)
        {
            sql += " AND c.Body = @Body";
            parameters.Add("Body", (int)filter.Body);
        }

        if (!string.IsNullOrWhiteSpace(filter.Color))
        {
            sql += " AND c.Color = @Color";
            parameters.Add("Color", filter.Color);
        }

        var carDictionary = new Dictionary<int, Car>();

        var result = await connection.QueryAsync<Car, CarImage, Car>(
            sql,
            (car, carImage) =>
            {
                if (!carDictionary.TryGetValue(car.Id, out var currentCar))
                {
                    currentCar = car;
                    currentCar.CarImagePaths = new List<CarImage>();
                    carDictionary.Add(currentCar.Id, currentCar);
                }

                if (carImage != null)
                {
                    currentCar.CarImagePaths.Add(carImage);
                }

                return currentCar;
            },
            param: parameters,
            splitOn: "Id"
        );

        return carDictionary.Values;
    }

    public List<(int Id, string Name)> GetAllBodyTypes()
    {
        return Enum.GetValues(typeof(BodyTypes))
        .Cast<BodyTypes>()
        .Select(bt => ((int)bt, bt.ToString()))
        .ToList();
    }
}