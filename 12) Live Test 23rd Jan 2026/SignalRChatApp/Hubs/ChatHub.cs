using Microsoft.AspNetCore.SignalR;

namespace SignalRChatApp.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnConnectedAsync()
        {
            // Fixed: Context.User.Identity.Name (with null-conditional operators)
            string welcomeMsg = $"{Context.User?.Identity?.Name ?? "Someone"} joined the chat";
            await Clients.All.SendAsync("ReceiveMessage", "SYSTEM", welcomeMsg);
            await base.OnConnectedAsync();
        }
    }
}