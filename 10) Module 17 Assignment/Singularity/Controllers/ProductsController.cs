using Microsoft.AspNetCore.Mvc;
using Singularity.Models;

namespace Singularity.Controllers
{
    public class ProductsController : Controller
    {
        // Optional: Index action if you have an Index view
        public IActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: Display list and form
        public IActionResult Manage()
        {
            var viewModel = new ProductManageViewModel
            {
                Products = ProductData.Products
            };
            ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;
            return View(viewModel);
        }

        // POST: Add new product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Product newProduct)
        {
            if (ModelState.IsValid)
            {
                newProduct.productId = ProductData.GetNextId();
                ProductData.Products.Add(newProduct);
                TempData["SuccessMessage"] = "Product added successfully!";
                return RedirectToAction(nameof(Manage));
            }

            // If invalid, reload the page with errors and preserve entered data
            var viewModel = new ProductManageViewModel
            {
                Products = ProductData.Products,
                NewProduct = newProduct
            };
            ViewBag.SuccessMessage = null;
            return View("Manage", viewModel);
        }
    }
}