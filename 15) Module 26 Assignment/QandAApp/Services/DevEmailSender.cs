using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace QandAApp.Services
{
    public class DevEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Just log to console/output window instead of sending real email
            Console.WriteLine("───────────────────────────────────────────────");
            Console.WriteLine($"To: {email}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Body:\n{htmlMessage}");
            Console.WriteLine("───────────────────────────────────────────────");

            return Task.CompletedTask;
        }
    }
}