using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public HomeController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Welcome to E-Commerce";

            var model = new HomeViewModel
            {
                Categories = await _categoryService.GetAllAsync(),
                Products = await _productService.GetAllAsync()
            };

            return View(model);
        }
    }

    // Important: public class inside the same namespace as the controller
    public class HomeViewModel
    {
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
    }
}