using ECommerce.Application.Interfaces;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StockTransactionsController : Controller
    {
        private readonly IGenericRepository<StockTransaction> _repository;

        public StockTransactionsController(IGenericRepository<StockTransaction> repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var transactions = await _repository.GetAllAsync();
            return View(transactions);
        }
    }
}