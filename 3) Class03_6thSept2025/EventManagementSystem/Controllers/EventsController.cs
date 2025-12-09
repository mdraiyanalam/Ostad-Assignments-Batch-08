using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventManagementSystem.Data;
using EventManagementSystem.Models;

namespace EventManagementSystem.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventManagementContext _context;

        public EventsController(EventManagementContext context)
        {
            _context = context;
        }

        // All Events page
        public async Task<IActionResult> AllEvents()
        {
            var events = await _context.Events
                .Include(e => e.AssignedUser)
                .ToListAsync();
            return View(events);
        }

        public async Task<IActionResult> UpcomingEvents()
        {
            var today = DateTime.Today;
            var upcomingEvents = await _context.Events
                .Where(e => e.Date > today)
                .Include(e => e.AssignedUser)
                .ToListAsync();

            return View(upcomingEvents);
        }
    }
}