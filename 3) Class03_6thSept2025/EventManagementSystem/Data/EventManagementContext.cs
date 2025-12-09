using Microsoft.EntityFrameworkCore;
using EventManagementSystem.Models;

namespace EventManagementSystem.Data
{
    public class EventManagementContext : DbContext
    {
        public EventManagementContext(DbContextOptions<EventManagementContext> options)
            : base(options) { }

        public DbSet<Event> Events => Set<Event>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // === Seed Users ===
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FirstName = "Alice", LastName = "Smith", ContactNumber = "111-1111", Email = "alice@company.com", UserType = 1 }, // Admin
                new User { Id = 2, FirstName = "Bob", LastName = "Johnson", ContactNumber = "222-2222", Email = "bob@company.com", UserType = 2 }, // Standard
                new User { Id = 3, FirstName = "Charlie", LastName = "Brown", ContactNumber = "333-3333", Email = "charlie@external.com", UserType = 3 }, // External
                new User { Id = 4, FirstName = "Diana", LastName = "Prince", ContactNumber = "444-4444", Email = "diana@guest.com", UserType = 4 }  // Guest
            );

            // === Seed Events (mix of past & future based on today = Sept 7, 2025) ===
            modelBuilder.Entity<Event>().HasData(
                // Past events
                new Event { Id = 1, Name = "Annual Conference 2025", Description = "Yearly tech conference", Date = new DateTime(2025, 8, 15), Address = "Grand Hotel, City A", AssignedUserId = 1 },
                new Event { Id = 2, Name = "Team Building Workshop", Description = "Outdoor activities", Date = new DateTime(2025, 9, 20), Address = "Mountain Resort", AssignedUserId = 2 },
                new Event { Id = 3, Name = "Product Launch", Description = "New product reveal", Date = new DateTime(2025, 11, 10), Address = "Downtown Hall", AssignedUserId = 3 },

                // Upcoming events (after Sept 7, 2025)
                new Event { Id = 4, Name = "Christmas Party 2025", Description = "Company holiday celebration", Date = new DateTime(2025, 10, 20), Address = "Rooftop Venue", AssignedUserId = 1 },
                new Event { Id = 5, Name = "New Year Kickoff", Description = "2026 planning session", Date = new DateTime(2025, 11, 8), Address = "Head Office", AssignedUserId = 2 },
                new Event { Id = 6, Name = "Spring Tech Expo", Description = "Latest tech showcase", Date = new DateTime(2026, 1, 8), Address = "Convention Center", AssignedUserId = 3 },
                new Event { Id = 7, Name = "Q2 Strategy Meeting", Description = "Planning for next quarter", Date = new DateTime(2025, 12, 20), Address = "Board Room", AssignedUserId = 4 }
            );
        }
    }
}