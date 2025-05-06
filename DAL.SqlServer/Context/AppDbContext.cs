using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;

namespace DAL.SqlServer.Context;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartLine> CartLines { get; set; }
    public DbSet<UserFavorite> UserFavorites { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }
    public DbSet<Auction> Auctions { get; set; }

}
