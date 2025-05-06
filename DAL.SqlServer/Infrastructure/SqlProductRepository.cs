using DAL.SqlServer.Context;
using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;
using System.Text.Json;

namespace DAL.SqlServer.Infrastructure;

public class SqlProductRepository(string connectionString, AppDbContext context) : BaseSqlRepository(connectionString), IProductRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int productId)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        product.IsDeleted = true;
        product.DeletedDate = DateTime.Now;
        product.DeletedBy = 0;


    }

    public IQueryable<Product> GetAll()
    {
        return _context.Products;
    }

    public IEnumerable<Product> GetByCategoryId(int categoryId)
    {
        var sql = @"SELECT Id, Name, CategoryId, UnitPrice, UnitsInStock, Description, ImagePath FROM Products WHERE CategoryId = @CategoryId";
        using var connection = OpenConnection();

        var rawProducts = connection.Query<dynamic>(sql, new { CategoryId = categoryId });


        var products = rawProducts.Select(p => new Product
        {
            Id = p.Id,
            Name = p.Name,
            CategoryId = p.CategoryId,
            UnitPrice = p.UnitPrice,
            UnitsInStock = p.UnitsInStock,
            Description = p.Description,
            ImagePath = JsonSerializer.Deserialize<List<string>>(p.ImagePath)
        });

        return products;
    }

    public Task<Product> GetByIdAsync(int id)
    {
        var product = _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        return product;

    }

    public Task<List<Product>> GetByNameAsync(string productName)
    {
        var products = _context.Products.Where(p => p.Name.Contains(productName)).ToListAsync();
        return products;
    }

    public async Task<List<Product>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
    {
        return await _context.Products.Where(p => p.UnitPrice >= minPrice && p.UnitPrice <= maxPrice).ToListAsync();
    }

    public void Update(Product product)
    {
        product.UpdatedDate = DateTime.Now;
        _context.Products.Update(product);
        _context.SaveChanges();
    }


}

