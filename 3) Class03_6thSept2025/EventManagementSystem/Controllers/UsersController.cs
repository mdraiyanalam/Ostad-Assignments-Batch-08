using Microsoft.AspNetCore.Mvc;
using EventManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EventManagementSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly EventManagementContext _context;
        public UsersController(EventManagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }
    }
}
