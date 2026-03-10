using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace QandAApp.Entities
{
    public class Answer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Body is required")]
        public string? Body { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }

        public int QuestionId { get; set; }
        public Question? Question { get; set; }

        public bool IsAccepted { get; set; } = false;
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}