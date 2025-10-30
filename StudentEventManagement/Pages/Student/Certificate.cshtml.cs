using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEventManagement.Data;
using System.Threading.Tasks;

namespace StudentEventManagement.Pages.Student
{
    public class CertificateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CertificateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public User? CertificateUser { get; set; }
        public Event? Event { get; set; }

        public async Task<IActionResult> OnGetAsync(int userId, int eventId)
        {
            CertificateUser = await _context.Users.FindAsync(userId);
            Event = await _context.Events.FindAsync(eventId);

            if (CertificateUser == null || Event == null)
            {
                return NotFound();
            }

            // Security check: ensure the current user is the one the certificate is for, or an admin
            var currentUsername = base.User.Identity?.Name;

            if (!base.User.IsInRole("Admin") && CertificateUser.Username != currentUsername)
            {
                return Forbid();
            }
            
            var participation = await _context.EventParticipants.FindAsync(eventId, userId);
            if (participation == null || participation.Status != "Certificate Issued")
            {
                // Only show certificate if it has been issued.
                // Admins can see it anytime.
                if(!base.User.IsInRole("Admin"))
                {
                    return Forbid();
                }
            }

            return Page();
        }
    }
}