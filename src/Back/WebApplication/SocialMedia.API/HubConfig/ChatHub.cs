using Microsoft.AspNetCore.SignalR;
using SocialMedia.Application.Contratos;
using SocialMedia.Application.Dtos;
using SocialMedia.Domain.Identity;

namespace SocialMedia.API.HubConfig
{
    public class ChatHub : Hub
    {
   

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "ConnectedUsers");
        }

        public async Task GetUsuariosConectados()
        {
            Clients.Group("ConnectedUsers").SendAsync("GetConnectedUsers");
            
        }

        public async Task GetUserConnectionAsync()
        {
            await Clients.Caller.SendAsync("getUserConnectionResponse", Context.ConnectionId);
        }

       

        public async Task SendMsg(MessageDto msg, string? connId)
        {
            if ( connId != null)
            {
                await Clients.Client(connId).SendAsync("sendMsgResponse", Context.ConnectionId, msg);
            }
        }

    }
}
