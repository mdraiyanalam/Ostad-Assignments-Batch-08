using EventManagementSystem.Models;
using System;
using System.Collections.Generic;

namespace EventManagementSystem.Services
{
    public static class DataService
    {
        public static List<Event> Events { get; } = new List<Event>
        {
            new Event { Id = 1, Name = "Past Event 1", Description = "Old conference", Date = new DateTime(2025, 8, 1), Address = "City A", AssignedUserId = 1 },
            new Event { Id = 2, Name = "Past Event 2", Description = "Old workshop", Date = new DateTime(2025, 9, 1), Address = "City B", AssignedUserId = 2 },
            new Event { Id = 3, Name = "Upcoming Event 1", Description = "Future seminar", Date = new DateTime(2025, 9, 3), Address = "City C", AssignedUserId = 3 },
            new Event { Id = 4, Name = "Upcoming Event 2", Description = "Future party", Date = new DateTime(2025, 10, 1), Address = "City D", AssignedUserId = 4 },
            // Add more as needed, ensuring past (before Sept 2, 2025) and future dates

            // Upcoming events (after Sept 2, 2025, but adjust for current date Dec 9, 2025)
            new Event { Id = 5, Name = "Upcoming Event 1", Description = "Future seminar on innovation", Date = new DateTime(2025, 9, 3), Address = "City E Auditorium", AssignedUserId = 1 }, // Past as of Dec 9
            new Event { Id = 6, Name = "Upcoming Event 2", Description = "Company party", Date = new DateTime(2025, 10, 1), Address = "City F Venue", AssignedUserId = 2 }, // Past as of Dec 9
            new Event { Id = 7, Name = "Upcoming Event 3", Description = "Tech expo", Date = new DateTime(2025, 11, 15), Address = "City G Expo Center", AssignedUserId = 3 }, // Past as of Dec 9
            new Event { Id = 8, Name = "Future Event 4", Description = "Holiday gathering", Date = new DateTime(2025, 12, 20), Address = "City H Resort", AssignedUserId = 4 }, // Upcoming as of Dec 9
            new Event { Id = 9, Name = "Future Event 5", Description = "New Year planning", Date = new DateTime(2026, 1, 5), Address = "City I Office", AssignedUserId = 1 }, // Upcoming
            new Event { Id = 10, Name = "Future Event 6", Description = "Annual review", Date = new DateTime(2026, 2, 10), Address = "City J Hall", AssignedUserId = 2 } // Upcoming
        };

        public static List<User> Users { get; } = new List<User>
        {
            new User { Id = 1, FirstName = "Admin", LastName = "User", ContactNumber = "123-456", Email = "admin@example.com", UserType = 1 },
            new User { Id = 2, FirstName = "Standard", LastName = "User", ContactNumber = "789-012", Email = "standard@example.com", UserType = 2 },
            new User { Id = 3, FirstName = "External", LastName = "User", ContactNumber = "345-678", Email = "external@example.com", UserType = 3 },
            new User { Id = 4, FirstName = "Guest", LastName = "User", ContactNumber = "901-234", Email = "guest@example.com", UserType = 4 },
        };
    }
}