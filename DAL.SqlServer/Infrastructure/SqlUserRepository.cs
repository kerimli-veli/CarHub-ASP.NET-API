using DAL.SqlServer.Context;
using Dapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infrastructure;

public class SqlUserRepository(string connectionString, AppDbContext context) : BaseSqlRepository(connectionString), IUserRepository
{
    private readonly AppDbContext _context = context;

    public IQueryable<User> GetAll()
    {
        return _context.Users;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return (await _context.Users.Include(c => c.Favorites).FirstOrDefaultAsync(u => u.Id == id))!;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return (await _context.Users.FirstOrDefaultAsync(u => u.Email == email))!;
    }
    public void Update(User user)
    {
        var users = _context.Users.ToList();
        user.UpdatedDate = DateTime.Now;
        _context.Update(user);
        _context.SaveChanges();
    }
    public async Task RegisterAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task Remove(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        user.IsDeleted = true;
        user.DeletedDate = DateTime.Now;
        user.DeletedBy = 0;
    }

    public async Task<List<Car>> GetUserFavoritesAsync(int userId)
    {
        var cars = await _context.Cars
        .Where(c => c.FavoritedByUsers.Any(f => f.UserId == userId))
        .Include(c => c.CarImagePaths)
        .ToListAsync();

        return cars;
    }

    public async Task<List<Car>> GetUserCarAsync(int userId)
    {
        var cars = await _context.Cars
            .Where(c => c.CreatedBy == userId)
            .Include(c => c.CarImagePaths)
            .ToListAsync();

        return cars; 
    }
}