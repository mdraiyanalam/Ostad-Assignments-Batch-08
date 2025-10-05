using Microsoft.EntityFrameworkCore;
using EventManagementSystem.Models;

namespace EventManagementSystem.Data
{
    public class EventManagementContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }

        public EventManagementContext(DbContextOptions<EventManagementContext> options)
            : base(options)
        {
        }
    }
}