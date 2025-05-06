using Domain.Entities;

namespace Repository.Repositories;

public interface ICartRepository
{
    
    Task AddAsync(Cart cart);  
    Task DeleteAsync(int cartId);  
    Task<Cart> GetCartWithLinesByUserId(int userId); 
    Task<Cart> GetCartWithLinesAsync(int cartId);
    Task AddProductToCartAsync(int cartId, int productId, int quantity, decimal unitPrice); 
    Task RemoveProductFromCartAsync(int cartId, int productId); 
    Task ClearCartLineAsync(int cartId); 
    Task<decimal> GetTotalPriceAsync(int cartId); 
    Task UpdateProductQuantityInCartAsync(int cartId, int productId, bool increase); 
}