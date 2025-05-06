using Repository.Repositories;

namespace Repository.Common;

public interface IUnitOfWork
{
    public INotificationRepository NotificationRepository { get; }
    public IAuctionRepository AuctionRepository { get; }
    public IChatMessageRepository ChatMessageRepository { get; }
    public IFavoriteRepository FavoriteRepository {  get; } 
    public ICategoryRepository CategoryRepository { get; }
    public IProductRepository ProductRepository { get; }
    public ICarRepository CarRepository { get; }
    public IUserRepository UserRepository { get; }
    public ICartRepository CartRepository { get; }
    public IRefreshTokenRepository RefreshTokenRepository { get; }

    public IPaymentRepository PaymentRepository { get; }

    public IOrderRepository OrderRepository { get; }
    Task CompleteAsync();
    Task<int> SaveChangeAsync();
  
}
