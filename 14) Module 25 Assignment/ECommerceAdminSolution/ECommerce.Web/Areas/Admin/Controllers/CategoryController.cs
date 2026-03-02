using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Models;
using System.Threading.Tasks;

namespace ECommerce.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // LIST - GET: /admin/category
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Categories";
            var list = await _service.GetAllAsync();
            return View(list);
        }

        // CREATE - GET: /admin/category/create
        public IActionResult Create()
        {
            ViewData["Title"] = "Create Category";
            return View();
        }

        // CREATE - POST: /admin/category/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(category);
                return RedirectToAction(nameof(Index));
            }
            ViewData["Title"] = "Create Category";
            return View(category);
        }

        // EDIT - GET: /admin/category/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category == null) return NotFound();
            ViewData["Title"] = "Edit Category";
            return View(category);
        }

        // EDIT - POST: /admin/category/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(category);
                return RedirectToAction(nameof(Index));
            }
            ViewData["Title"] = "Edit Category";
            return View(category);
        }

        // DETAILS - GET: /admin/category/details/5
        public async Task<IActionResult> Details(int id)
        {
            var cat = await _service.GetByIdAsync(id);
            if (cat == null) return NotFound();
            ViewData["Title"] = "Category Details";
            return View(cat);
        }

        // DELETE - GET (confirmation): /admin/category/delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _service.GetByIdAsync(id);
            if (cat == null) return NotFound();
            ViewData["Title"] = "Delete Category";
            return View(cat);
        }

        // DELETE - POST: /admin/category/delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}