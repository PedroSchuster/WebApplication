using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.Domain.Models;

namespace SocialMedia.Persistence.Contratos
{
    public interface IMessagePersist : IGeralPersist
    {
        Task<IEnumerable<Message>> GetAllMessagesAsync(int connId);

        Task<ChatConnection> GetConectionAsync(int connId);

        Task<ChatConnection> CheckConectionAsync(int userId, int targetUserId);
    }
}
