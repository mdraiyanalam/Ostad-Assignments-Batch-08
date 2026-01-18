using Microsoft.EntityFrameworkCore;
using ChatAppApi.Models;  // For User and Message (if in same namespace, this might be redundant but safe)
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatAppApi.Models
{
    public class AppDbContext : DbContext
    {
        // DbSets for your entities
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        // Constructor for dependency injection (used by Program.cs AddDbContext)
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Fallback configuration for design-time tools (e.g., migrations, CLI)
        // Only used if no options are provided (e.g., during dotnet ef commands)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=chatapp.db");
            }
        }

        // Configure entity relationships and constraints
        // This ensures proper foreign keys and prevents accidental data loss in chats
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Message → Sender (User has many sent messages)
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Message → Receiver (User has many received messages)
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}