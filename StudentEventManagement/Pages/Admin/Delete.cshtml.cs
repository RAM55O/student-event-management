using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEventManagement.Data;

namespace StudentEventManagement.Pages.Admin
{
    public class DeleteModel : PageModel
    {
        private readonly StudentEventManagement.Data.ApplicationDbContext _context;

        public DeleteModel(StudentEventManagement.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Event Event { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anevent = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);

            if (anevent == null)
            {
                return NotFound();
            }
            else
            {
                Event = anevent;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anevent = await _context.Events.FindAsync(id);
            if (anevent != null)
            {
                Event = anevent;
                _context.Events.Remove(Event);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}