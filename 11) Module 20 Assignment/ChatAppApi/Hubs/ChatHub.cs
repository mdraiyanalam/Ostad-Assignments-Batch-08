using ChatAppApi.Models;                    // ← correct namespace
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;        // ← fixes ToListAsync(), OrderBy(), Where()

namespace ChatAppApi.Hubs                 // ← optional: better organization (create Hubs folder if not exists)
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _db;

        public ChatHub(AppDbContext db)
        {
            _db = db;
        }

        public async Task SendMessage(int senderId, int receiverId, string content)
        {
            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                Timestamp = DateTime.UtcNow   // ← good practice to set here
            };

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            // Send to specific user (SignalR user ID = string of receiverId)
            await Clients.User(receiverId.ToString())
                .SendAsync("ReceiveMessage", senderId, content, message.Timestamp);
        }

        public async Task GetPreviousMessages(int userId, int otherUserId)
        {
            var messages = await _db.Messages
                .Where(m =>
                    (m.SenderId == userId && m.ReceiverId == otherUserId) ||
                    (m.SenderId == otherUserId && m.ReceiverId == userId))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            await Clients.Caller.SendAsync("PreviousMessages", messages);
        }
    }
}