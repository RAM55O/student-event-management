using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentEventManagement.Data;

namespace StudentEventManagement.Pages.Admin
{
    public class CreateModel : PageModel
    {
        private readonly StudentEventManagement.Data.ApplicationDbContext _context;

        public CreateModel(StudentEventManagement.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            AllCategories = new SelectList(_context.Categories, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Event Event { get; set; } = default!;

        public SelectList AllCategories { get; set; }

        [BindProperty]
        public int[] SelectedCategories { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Get the current user's ID
            var currentUser = _context.Users.FirstOrDefault(u => u.Username == User.Identity.Name);
            if (currentUser == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return Page();
            }
            Event.CreatedBy = currentUser.Id;

            _context.Events.Add(Event);
            await _context.SaveChangesAsync();

            if (SelectedCategories != null)
            {
                foreach (var categoryId in SelectedCategories)
                {
                    _context.EventCategories.Add(new EventCategory { EventId = Event.Id, CategoryId = categoryId });
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}