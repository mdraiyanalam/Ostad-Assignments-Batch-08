using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Models
{
    public class User
    {

        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int UserType { get; set; }

        public string GetUserTypeText()
        {
            return UserType switch
            {
                1 => "Admin User",
                2 => "Standard User",
                3 => "External User",
                4 => "Guest User",
                _ => "Unknown"
            };
        }

    }
}
