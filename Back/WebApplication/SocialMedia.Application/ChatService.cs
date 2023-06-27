using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SocialMedia.Application.Contratos;
using SocialMedia.Application.Dtos;
using SocialMedia.Domain.Models;
using SocialMedia.Persistence.Contratos;

namespace SocialMedia.Application
{
    public class ChatService : IChatService
    {
        private readonly IMessagePersist _messagePersist;
        private readonly IMapper _mapper;

        public ChatService(IMessagePersist messagePersist, IMapper mapper)
        {
            _messagePersist = messagePersist;
            _mapper = mapper;
        }


        public async Task<ConnectionDto> CheckConnectionAsync(int userId, int secondUserId)
        {
            try
            {
                var connection = await _messagePersist.CheckConectionAsync(userId, secondUserId);
                if (connection == null) connection = await CreateConnectionAsync(userId, secondUserId);

                return _mapper.Map<ConnectionDto>(connection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        private async Task<ChatConnection> CreateConnectionAsync(int userId, int secondUserId)
        {
            try
            {
                ChatConnection chatConnection = new ChatConnection { UserId = userId, SecondUserId = secondUserId };
                _messagePersist.Add<ChatConnection>(chatConnection);

                await _messagePersist.SaveChangesAsync();

                return chatConnection;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RegisterMessageAsync(MessageDto model)
        {
            try
            {
                var conn = await _messagePersist.GetConectionAsync(model.ConnectionId);
                if (conn == null) return false;

                var message = _mapper.Map<Message>(model);
                _messagePersist.Add<Message>(message);
                return await _messagePersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<MessageDto>> GetAllMessagesAsync(int connId)
        {
            try
            {
                if (await _messagePersist.GetConectionAsync(connId) == null) return null;

                var messages = await _messagePersist.GetAllMessagesAsync(connId);
                return _mapper.Map<IEnumerable<MessageDto>>(messages);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

    }

}
