using Domain.Entities;

namespace Repository.Repositories;

public interface IOrderRepository
{
    Task<Order> CreateOrderAsync(Order order);
    Task<Order> GetOrderByIdAsync(int id);
    Task<List<Order>> GetOrdersByUserIdAsync(int userId);
    Task<List<Order>> GetAllOrdersAsync();
    Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus);
    Task<bool> DeleteOrderAsync(int orderId);
    Task<bool> OrderExistsAsync(int orderId);
}

