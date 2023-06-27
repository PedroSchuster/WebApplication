using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.Application.Dtos;

namespace SocialMedia.Application.Contratos
{
    public interface IChatService
    {
        Task<ConnectionDto> CheckConnectionAsync(int userId, int secondUserId);

        Task<bool> RegisterMessageAsync(MessageDto model);

        Task<IEnumerable<MessageDto>> GetAllMessagesAsync(int userId);

    }
}
