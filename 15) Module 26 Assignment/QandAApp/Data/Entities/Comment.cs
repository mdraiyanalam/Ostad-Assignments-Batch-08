using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace QandAApp.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Body is required")]
        public string? Body { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UserId { get; set; }

        public IdentityUser? User { get; set; }

        public int? QuestionId { get; set; }
        public Question? Question { get; set; }

        public int? AnswerId { get; set; }
        public Answer? Answer { get; set; }
    }
}