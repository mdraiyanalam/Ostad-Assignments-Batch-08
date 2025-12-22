using System.Collections.Generic;

namespace Singularity.Models
{
    public class ProductManageViewModel
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public Product NewProduct { get; set; } = new Product();
    }
}