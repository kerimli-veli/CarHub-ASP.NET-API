using DAL.SqlServer.Context;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;
using Stripe.Checkout;

public class SqlStripePaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public SqlStripePaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string> CreateCheckoutSessionAsync(int userId)
    {

        var cart = await _context.Carts
            .Include(c => c.CartLines) 
            .ThenInclude(cl => cl.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsDeleted);

        if (cart == null || !cart.CartLines.Any(cl => !cl.IsDeleted))
        {
            Console.WriteLine("Cart not found or empty");
            throw new Exception("The cart is empty or all products have been deleted.");
        }

        Console.WriteLine($"Number of items in the cart: {cart.CartLines.Count}"); 

 
        var lineItems = cart.CartLines
            .Where(cl => !cl.IsDeleted) 
            .Select(cl => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(cl.UnitPrice * 100), 
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = cl.Product?.Name ?? $"Product ID: {cl.ProductId}"
                    }
                },
                Quantity = cl.Quantity
            }).ToList();

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = lineItems,
            Mode = "payment",
            SuccessUrl = "https://seninsiten.com/payment-success",
            CancelUrl = "https://seninsiten.com/payment-cancel"
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return session.Url;
    }
}
