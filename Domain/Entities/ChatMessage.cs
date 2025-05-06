using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;
public class ChatMessage
{
    public int Id { get; set; }
    public int SenderId { get; set; } = default!;
    public int ReceiverId { get; set; } = default!;
    public string Text { get; set; } = default!;
    public DateTime SentAt { get; set; }
}


