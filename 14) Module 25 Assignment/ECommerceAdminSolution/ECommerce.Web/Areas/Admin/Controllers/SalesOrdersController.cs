using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Models;
using System.Threading.Tasks;

namespace ECommerce.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SalesOrdersController : Controller
    {
        private readonly IStockService _stockService;
        private readonly IProductService _productService;

        public SalesOrdersController(IStockService stockService, IProductService productService)
        {
            _stockService = stockService;
            _productService = productService;
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Products = new SelectList(await _productService.GetAllAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesOrder order)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _stockService.ProcessSalesOrderAsync(order);
                    return RedirectToAction("Index", "Product");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message); // e.g., Insufficient stock
                }
            }
            ViewBag.Products = new SelectList(await _productService.GetAllAsync(), "Id", "Name");
            return View(order);
        }
    }
}