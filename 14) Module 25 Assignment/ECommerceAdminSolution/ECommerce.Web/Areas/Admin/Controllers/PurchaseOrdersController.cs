using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Models;
using System.Threading.Tasks;

namespace ECommerce.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PurchaseOrdersController : Controller
    {
        private readonly IStockService _stockService;
        private readonly IProductService _productService;

        public PurchaseOrdersController(IStockService stockService, IProductService productService)
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
        public async Task<IActionResult> Create(PurchaseOrder order)
        {
            if (ModelState.IsValid)
            {
                await _stockService.ProcessPurchaseOrderAsync(order);
                return RedirectToAction("Index", "Product");
            }
            ViewBag.Products = new SelectList(await _productService.GetAllAsync(), "Id", "Name");
            return View(order);
        }
    }
}