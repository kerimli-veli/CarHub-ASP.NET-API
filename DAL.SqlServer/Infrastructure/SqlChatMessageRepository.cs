using DAL.SqlServer.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infrastructure;


public class SqlChatMessageRepository : IChatMessageRepository
{
    private readonly AppDbContext _context;

    public SqlChatMessageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ChatMessage message)
    {
        await _context.ChatMessages.AddAsync(message);
        await _context.SaveChangesAsync();
    }

    public async Task<ChatMessage> GetByIdAsync(int id)
    {
        return await _context.ChatMessages
                             .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<ChatMessage>> GetMessagesByUserIdsAsync(int senderId, int receiverId)
    {
        return await _context.ChatMessages
                             .Where(x => (x.SenderId == senderId && x.ReceiverId == receiverId) ||
                                         (x.SenderId == receiverId && x.ReceiverId == senderId))
                             .OrderBy(x => x.SentAt)
                             .ToListAsync();
    }
}
