using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Domain.Models;
using SocialMedia.Persistence.Contextos;
using SocialMedia.Persistence.Contratos;

namespace SocialMedia.Persistence
{
    public class MessagePersist : GeralPersist, IMessagePersist
    {
        private readonly SocialMediaContext _context;

        public MessagePersist(SocialMediaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetAllMessagesAsync(int connId)
        {
            return await _context.Messages
                .Where(x=>x.ConnectionId == connId).ToListAsync();
        }

        public async Task<ChatConnection> CheckConectionAsync(int userId, int targetUserId)
        {
            return await _context.ChatConnections
                .Where(x => (x.UserId == userId && x.SecondUserId == targetUserId) ||
                      (x.UserId == targetUserId && x.SecondUserId == userId)
            ).FirstOrDefaultAsync();
        }

        public async Task<ChatConnection> GetConectionAsync(int connId)
        {
            return await _context.ChatConnections
                .Where(x => x.Id == connId)
                .FirstOrDefaultAsync();
        }


    }
}
