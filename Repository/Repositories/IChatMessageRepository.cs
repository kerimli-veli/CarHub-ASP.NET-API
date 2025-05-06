using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories;

public interface IChatMessageRepository
{
    Task AddAsync(ChatMessage message);
    Task<ChatMessage> GetByIdAsync(int id);
    Task<IEnumerable<ChatMessage>> GetMessagesByUserIdsAsync(int senderId, int receiverId);
}
