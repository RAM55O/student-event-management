using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentEventManagement.Data;

namespace StudentEventManagement.Pages.Admin
{
    public class EditModel : PageModel
    {
        private readonly StudentEventManagement.Data.ApplicationDbContext _context;

        public EditModel(StudentEventManagement.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Event Event { get; set; } = default!;

        public SelectList AllCategories { get; set; }

        [BindProperty]
        public int[] SelectedCategories { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anevent =  await _context.Events.Include(e => e.EventCategories).FirstOrDefaultAsync(m => m.Id == id);
            if (anevent == null)
            {
                return NotFound();
            }
            Event = anevent;
            AllCategories = new SelectList(_context.Categories, "Id", "Name");
            SelectedCategories = Event.EventCategories.Select(ec => ec.CategoryId).ToArray();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var eventToUpdate = await _context.Events.Include(e => e.EventCategories).FirstOrDefaultAsync(e => e.Id == Event.Id);

            if (eventToUpdate == null)
            {
                return NotFound();
            }

            eventToUpdate.Title = Event.Title;
            eventToUpdate.Description = Event.Description;
            eventToUpdate.Date = Event.Date;

            eventToUpdate.EventCategories.Clear();
            if (SelectedCategories != null)
            {
                foreach (var categoryId in SelectedCategories)
                {
                    eventToUpdate.EventCategories.Add(new EventCategory { EventId = eventToUpdate.Id, CategoryId = categoryId });
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(Event.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}