using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QandAApp.Entities;

namespace QandAApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Keep cascade only on Question → Answers
            builder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);   // ← keep this one

            // For Comments → no automatic cascade delete
            // Deleting a Question or Answer will NOT auto-delete related Comments
            // You will need to delete comments manually in business logic before deleting question/answer

            builder.Entity<Comment>()
                .HasOne(c => c.Question)
                .WithMany(q => q.Comments)
                .HasForeignKey(c => c.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);   // ← Restrict / NoAction

            builder.Entity<Comment>()
                .HasOne(c => c.Answer)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.AnswerId)
                .OnDelete(DeleteBehavior.Restrict);   // ← Restrict / NoAction
        }
    }
}