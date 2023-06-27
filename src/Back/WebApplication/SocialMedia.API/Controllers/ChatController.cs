using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Application.Contratos;
using SocialMedia.Application.Dtos;

namespace SocialMedia.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("messages/{connId}")]
        public async Task<IActionResult> GetMessagesHistory(int connId) 
        {
            try
            {
                var messages = await _chatService.GetAllMessagesAsync(connId);
                if (messages == null) return NoContent();
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Mensagens. Erro: {ex.Message} ");

            }
        }

        [HttpGet("connection/{userId}/{targetId}")]
        public async Task<IActionResult> GetConnection(int userId, int targetId)
        {
            try
            {
                var conn = await _chatService.CheckConnectionAsync(userId, targetId);
                if (conn == null) return NoContent();
                return Ok(conn.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Conexão. Erro: {ex.Message} ");

            }
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage(MessageDto message)
        {
            try
            {
                var result = await _chatService.RegisterMessageAsync(message);
                if (result == null) return NoContent();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar registrar mensagem. Erro: {ex.Message} ");

            }
        }
    }
}
