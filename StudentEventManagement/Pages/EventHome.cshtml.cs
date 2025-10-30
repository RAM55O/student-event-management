using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentEventManagement.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace StudentEventManagement.Pages
{
    public class EventHomeModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EventHomeModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? MobileNumber { get; set; }
        public string? Role { get; set; }
        public List<Event> Events { get; set; } = new List<Event>();

        public SelectList AllCategories { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? SelectedCategoryId { get; set; }

        public IActionResult OnGet()
        {
            var username = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToPage("/Account/Login");
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                FullName = $"{user.FirstName} {user.Surname}";
                Email = user.Email;
                MobileNumber = user.MobileNumber;
                Role = user.Role;
            }

            AllCategories = new SelectList(_context.Categories, "Id", "Name");

            IQueryable<Event> eventsQuery = _context.Events
                .Where(e => e.Date >= DateTime.Now);

            if (SelectedCategoryId.HasValue)
            {
                eventsQuery = eventsQuery
                    .Where(e => e.EventCategories.Any(ec => ec.CategoryId == SelectedCategoryId.Value));
            }

            Events = eventsQuery
                .OrderBy(e => e.Date)
                .Take(6)
                .ToList();

            return Page();
        }
    }
}