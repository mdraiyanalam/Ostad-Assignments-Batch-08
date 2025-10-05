using Microsoft.AspNetCore.Mvc;
using EventManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EventManagementSystem.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventManagementContext _context;

        public EventsController(EventManagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AllEvents()
        {
            var events = await _context.Events
                .Include(e => e.AssignedUser)
                .ToListAsync();
            return View(events);
        }
    }
}