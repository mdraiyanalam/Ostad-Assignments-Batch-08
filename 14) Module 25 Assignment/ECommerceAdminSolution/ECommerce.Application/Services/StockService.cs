using ECommerce.Application.Interfaces;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Models;
using System;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class StockService : IStockService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<StockTransaction> _transactionRepository;
        private readonly IGenericRepository<SalesOrder> _salesRepository;
        private readonly IGenericRepository<PurchaseOrder> _purchaseRepository;

        public StockService(
            IGenericRepository<Product> productRepository,
            IGenericRepository<StockTransaction> transactionRepository,
            IGenericRepository<SalesOrder> salesRepository,
            IGenericRepository<PurchaseOrder> purchaseRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _salesRepository = salesRepository ?? throw new ArgumentNullException(nameof(salesRepository));
            _purchaseRepository = purchaseRepository ?? throw new ArgumentNullException(nameof(purchaseRepository));
        }

        public async Task ProcessSalesOrderAsync(SalesOrder order)
        {
            var product = await _productRepository.GetByIdAsync(order.ProductId);
            if (product == null) throw new Exception("Product not found");
            if (product.StockQuantity < order.Quantity) throw new Exception("Insufficient stock");

            product.StockQuantity -= order.Quantity;
            _productRepository.Update(product);
            await _productRepository.SaveAsync();

            var transaction = new StockTransaction { ProductId = order.ProductId, Quantity = -order.Quantity, TransactionType = "Sales" };
            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveAsync();

            await _salesRepository.AddAsync(order);
            await _salesRepository.SaveAsync();
        }

        public async Task ProcessPurchaseOrderAsync(PurchaseOrder order)
        {
            var product = await _productRepository.GetByIdAsync(order.ProductId);
            if (product == null) throw new Exception("Product not found");

            product.StockQuantity += order.Quantity;
            _productRepository.Update(product);
            await _productRepository.SaveAsync();

            var transaction = new StockTransaction { ProductId = order.ProductId, Quantity = order.Quantity, TransactionType = "Purchase" };
            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveAsync();

            await _purchaseRepository.AddAsync(order);
            await _purchaseRepository.SaveAsync();
        }
    }
}