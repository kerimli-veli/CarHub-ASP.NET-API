namespace Repository.Repositories;

public interface IPaymentRepository
{
    Task<string> CreateCheckoutSessionAsync(int userId);
}