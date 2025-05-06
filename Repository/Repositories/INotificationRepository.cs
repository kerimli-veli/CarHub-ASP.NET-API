using Domain.Entities;

namespace Repository.Repositories;

public interface INotificationRepository
{
     Task<List<Notification>> GetAllNotification(int userId);
}
