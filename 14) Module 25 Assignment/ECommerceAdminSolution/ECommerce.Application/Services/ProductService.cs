using ECommerce.Application.Interfaces;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _repository;
        private readonly IGenericRepository<StockTransaction> _transactionRepository; // For transaction logging

        public ProductService(
            IGenericRepository<Product> repository,
            IGenericRepository<StockTransaction> transactionRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _repository.GetAll()
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _repository.GetAll()
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateAsync(Product product)
        {
            await _repository.AddAsync(product);
            await _repository.SaveAsync();

            // Log initial stock if > 0 (treated as initial purchase)
            if (product.StockQuantity > 0)
            {
                var transaction = new StockTransaction
                {
                    ProductId = product.Id,
                    Quantity = product.StockQuantity,
                    TransactionType = "Purchase",
                    Date = DateTime.UtcNow
                };
                await _transactionRepository.AddAsync(transaction);
                await _transactionRepository.SaveAsync();
            }
        }

        public async Task UpdateAsync(Product product)
        {
            // Step 1: Load the current database value WITHOUT tracking (to avoid conflict)
            var oldProduct = await _repository.GetAll()
                .AsNoTracking()                     // ← This is the key fix: prevents double tracking
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (oldProduct == null)
            {
                throw new ArgumentException("Product not found");
            }

            // Step 2: Calculate stock change delta (only if stock changed)
            var delta = product.StockQuantity - oldProduct.StockQuantity;
            if (delta != 0)
            {
                var type = delta > 0 ? "Purchase" : "Sales";

                var transaction = new StockTransaction
                {
                    ProductId = product.Id,
                    Quantity = delta,
                    TransactionType = type,
                    Date = DateTime.UtcNow
                };

                await _transactionRepository.AddAsync(transaction);
                await _transactionRepository.SaveAsync(); // Save transaction (can be combined later if desired)
            }

            // Step 3: Now safe to update the incoming (form-bound) product instance
            _repository.Update(product);
            await _repository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                _repository.Delete(entity);
                await _repository.SaveAsync();
            }
        }

        public async Task<int> GetCurrentStockAsync(int id)
        {
            var product = await GetByIdAsync(id);
            return product?.StockQuantity ?? 0;
        }
    }
}