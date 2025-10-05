using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookManagementApp.Models;
using System;
using System.Threading.Tasks;

namespace BookManagementApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _context.Books.ToListAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Index: {ex.Message}");
                ViewData["ErrorMessage"] = "An error occurred while retrieving the book list. Please try again later.";
                return View("Error");
            }
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return NotFound();
                }
                return View(book);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Details: {ex.Message}");
                ViewData["ErrorMessage"] = "An error occurred while retrieving book details.";
                return View("Error");
            }
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,Price")] Book book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Books.Add(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbEx)
                {
                    Console.WriteLine($"Database error in Create: {dbEx.Message}");
                    ViewData["ErrorMessage"] = "A database error occurred while creating the book. Please check your input.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in Create: {ex.Message}");
                    ViewData["ErrorMessage"] = "An unexpected error occurred while creating the book.";
                }
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return NotFound();
                }
                return View(book);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Edit (GET): {ex.Message}");
                ViewData["ErrorMessage"] = "An error occurred while loading the book for editing.";
                return View("Error");
            }
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Price")] Book book)
        {
            if (id != book.Id || !ModelState.IsValid)
            {
                return NotFound();
            }

            try
            {
                _context.Update(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException concEx)
            {
                Console.WriteLine($"Concurrency error in Edit: {concEx.Message}");
                ViewData["ErrorMessage"] = "The book was modified by another user. Please reload and try again.";
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database error in Edit: {dbEx.Message}");
                ViewData["ErrorMessage"] = "A database error occurred while updating the book.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Edit: {ex.Message}");
                ViewData["ErrorMessage"] = "An unexpected error occurred while updating the book.";
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return NotFound();
                }
                return View(book);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Delete (GET): {ex.Message}");
                ViewData["ErrorMessage"] = "An error occurred while loading the book for deletion.";
                return View("Error");
            }
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database error in Delete: {dbEx.Message}");
                ViewData["ErrorMessage"] = "A database error occurred while deleting the book.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Delete: {ex.Message}");
                ViewData["ErrorMessage"] = "An unexpected error occurred while deleting the book.";
            }
            return View("Error");
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}