using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

// CORRECT: Use the actual project namespace
using APIs;  // This refers to your Book class in Book.cs

namespace APIs.Controllers;  // Must match project name

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private static List<Book> _books = new List<Book>
    {
        new Book { Id = 1, Title = "1984", Author = "George Orwell", Year = 1949 },
        new Book { Id = 2, Title = "To Kill a Mockingbird", Author = "Harper Lee", Year = 1960 }
    };

    private static int _nextId = 3;

    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetBooks()
    {
        return Ok(_books);
    }

    [HttpGet("{id}")]
    public ActionResult<Book> GetBook(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        if (book == null)
            return NotFound();
        return Ok(book);
    }

    [HttpPost]
    public ActionResult<Book> AddBook([FromBody] Book newBook)
    {
        if (newBook == null)
            return BadRequest();

        newBook.Id = _nextId++;
        _books.Add(newBook);
        return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateBook(int id, [FromBody] Book updatedBook)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        if (book == null)
            return NotFound();

        book.Title = updatedBook.Title;
        book.Author = updatedBook.Author;
        book.Year = updatedBook.Year;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBook(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        if (book == null)
            return NotFound();

        _books.Remove(book);
        return NoContent();
    }
}