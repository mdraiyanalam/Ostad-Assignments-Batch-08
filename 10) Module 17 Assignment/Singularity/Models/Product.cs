using System.ComponentModel.DataAnnotations;

namespace Singularity.Models
{
    public class Product
    {
        public int productId { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        public string? productName { get; set; }

        [Required(ErrorMessage = "Product Description is required")]
        public string? productDescription { get; set; }

        [Required(ErrorMessage = "Product Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal productPrice { get; set; }

        [Required(ErrorMessage = "Stock Quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock Quantity must be non-negative")]
        public int StockQuantity { get; set; }
    }
}