using System.ComponentModel.DataAnnotations;

namespace BookManagementApp.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Author is required")]
        [StringLength(50, ErrorMessage = "Author cannot exceed 50 characters")]
        public string Author { get; set; } = null!;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000, ErrorMessage = "Price must be between 0.01 and 1000")]
        public decimal Price { get; set; }
    }
}