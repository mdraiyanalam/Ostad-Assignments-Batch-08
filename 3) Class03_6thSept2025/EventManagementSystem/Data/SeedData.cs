using EventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EventManagementSystem.Data
{
    public class SeedData
    {
        public static void Initialize(EventManagementContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return; // DB has been seeded
            }

            var users = new User[]
            {
                new User { FirstName = "John", LastName = "Doe", ContactNumber = "1234567890", Email = "john.doe@example.com", UserType = 1 },
                new User { FirstName = "Jane", LastName = "Smith", ContactNumber = "0987654321", Email = "jane.smith@example.com", UserType = 2 },
                new User { FirstName = "Bob", LastName = "Johnson", ContactNumber = "5551234567", Email = "bob.johnson@example.com", UserType = 3 },
                new User { FirstName = "Alice", LastName = "Brown", ContactNumber = "4449876543", Email = "alice.brown@example.com", UserType = 4 }
            };

            context.Users.AddRange(users);
            context.SaveChanges();

            var events = new Event[]
            {
                new Event
                {
                    Name = "Tech Conference",
                    Description = "Annual tech conference",
                    Date = new DateTime(2025, 8, 15),
                    Address = "123 Tech St",
                    Category = "Technology",
                    AssignedUserId = users[0].Id
                },
                new Event
                {
                    Name = "Music Festival",
                    Description = "Summer music festival",
                    Date = new DateTime(2025, 9, 10),
                    Address = "456 Music Ave",
                    Category = "Music",
                    AssignedUserId = users[1].Id
                },
                new Event
                {
                    Name = "Art Exhibition",
                    Description = "Local art showcase",
                    Date = new DateTime(2025, 9, 5),
                    Address = "789 Art Rd",
                    Category = "Art",
                    AssignedUserId = users[2].Id
                },
                new Event
                {
                    Name = "Charity Run",
                    Description = "5K charity run",
                    Date = new DateTime(2025, 10, 1),
                    Address = "101 Run Blvd",
                    Category = "Sports",
                    AssignedUserId = users[3].Id
                }
            };

            context.Events.AddRange(events);
            context.SaveChanges();
        }
    }
}