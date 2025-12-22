using System.Collections.Generic;

namespace Singularity.Models
{
    public static class ProductData
    {
        public static List<Product> Products { get; } = new List<Product>
        {
            new Product
            {
                productId = 1,
                productName = "Laptop",
                productDescription = "High-performance laptop",
                productPrice = 999.99m,
                StockQuantity = 10
            },
            new Product
            {
                productId = 2,
                productName = "Smartphone",
                productDescription = "Latest model smartphone",
                productPrice = 699.99m,
                StockQuantity = 20
            }
        };

        private static int _nextId = 3;

        public static int GetNextId()
        {
            return _nextId++;
        }
    }
}