using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEventManagement.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace StudentEventManagement.Pages.Student
{
    public class MyCertificatesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MyCertificatesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<EventParticipant> Participations { get; set; } = new List<EventParticipant>();

        public async Task<IActionResult> OnGetAsync()
        {
            var username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToPage("/Account/Login");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return NotFound();
            }

            Participations = await _context.EventParticipants
                .Include(ep => ep.Event)
                .Where(ep => ep.UserId == user.Id && (ep.Status == "Participation Approved" || ep.Status == "Certificate Issued" || ep.Status == "Certificate Approved" || ep.Status == "Certificate Declined" || ep.Status == "Certificate Requested"))
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostRequestCertificateAsync(int eventId)
        {
            var username = User.Identity.Name;
            var user = await _context.Users.SingleAsync(u => u.Username == username);
            var participation = await _context.EventParticipants.SingleOrDefaultAsync(ep => ep.EventId == eventId && ep.UserId == user.Id);

            if (participation != null && participation.Status == "Participation Approved")
            {
                participation.Status = "Certificate Requested";
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetDownloadCertificateAsync(int eventId)
        {
            var username = User.Identity.Name;
            var user = await _context.Users.SingleAsync(u => u.Username == username);
            var participation = await _context.EventParticipants
                .Include(ep => ep.Event)
                .SingleOrDefaultAsync(ep => ep.EventId == eventId && ep.UserId == user.Id);

            if (participation == null || participation.Status != "Certificate Issued")
            {
                return Forbid();
            }

            // For now, generate a dummy certificate file.
            var certificateContent = $"This is to certify that {user.FirstName} {user.Surname} has successfully participated in the event '{participation.Event.Title}'.";
            var fileName = $"Certificate-{participation.Event.Title.Replace(" ", "_")}.txt";

            return File(Encoding.UTF8.GetBytes(certificateContent), "text/plain", fileName);
        }
    }
}
