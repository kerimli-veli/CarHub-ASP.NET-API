using DAL.SqlServer.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infrastructure;

public class SqlCategoryRepository(string connectionString, AppDbContext context) : BaseSqlRepository(connectionString), ICategoryRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        category.IsDeleted = true;
        category.DeletedDate = DateTime.Now;
        category.DeletedBy = 0;
    }

    public IQueryable<Category> GetAll()
    {
        return _context.Categories;
    }

    public async Task<Category> GetByIdAsync(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        return category;

    }

    public async Task<Category> GetByNameAsync(string name)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
        return category;
    }

    public void Update(Category category)
    {
        category.UpdatedDate = DateTime.Now;
        _context.Update(category);
        _context.SaveChanges();
    }

    public async Task<IEnumerable<Category>> GetCategoriesWithProducts()
    {
        var categories = await _context.Categories.Include(c => c.Products).ToListAsync();
        return categories;

    }


}



