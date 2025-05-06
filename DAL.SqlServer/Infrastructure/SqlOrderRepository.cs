using DAL.SqlServer.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infrastructure;

public class SqlOrderRepository(string connectionString, AppDbContext context) : BaseSqlRepository(connectionString), IOrderRepository
{
    private readonly string _connectionString = connectionString;
    private readonly AppDbContext _context= context;

    public async Task<Order> CreateOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null) return false;

        order.IsDeleted = true;
        order.DeletedDate = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders.Where(o => !o.IsDeleted).Include(o => o.OrderLines).ThenInclude(ol => ol.Product).ToListAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int id)
    {
        return await _context.Orders.Include(o => o.OrderLines).ThenInclude(ol => ol.Product).FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
    }

    public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
    {
        return await _context.Orders.Where(o => o.UserId == userId && !o.IsDeleted).Include(o => o.OrderLines).ThenInclude(ol => ol.Product).ToListAsync();
    }

    public async Task<bool> OrderExistsAsync(int orderId)
    {
        return await _context.Orders.AnyAsync(o => o.Id == orderId && !o.IsDeleted);
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null || order.IsDeleted) return false;

        order.Status = newStatus;
        order.UpdatedDate = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }
}
