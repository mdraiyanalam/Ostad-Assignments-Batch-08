using ECommerce.Domain.Models;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IStockService
    {
        Task ProcessSalesOrderAsync(SalesOrder order);
        Task ProcessPurchaseOrderAsync(PurchaseOrder order);
    }
}