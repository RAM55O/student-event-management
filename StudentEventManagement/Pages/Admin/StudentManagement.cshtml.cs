using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentEventManagement.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentEventManagement.Pages.Admin
{
    public class StudentManagementModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public StudentManagementModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<User> Users { get; set; } = new List<User>();
        public Dictionary<int, List<EventParticipant>> UserEvents { get; set; } = new Dictionary<int, List<EventParticipant>>();
        public IList<Event> AllEvents { get; set; } = new List<Event>();

        public async Task OnGetAsync()
        {
            Users = await _context.Users.Where(u => u.Role == "Student").ToListAsync();
            var eventParticipants = await _context.EventParticipants
                .Include(ep => ep.Event)
                .ToListAsync();

            UserEvents = new Dictionary<int, List<EventParticipant>>();
            foreach (var user in Users)
            {
                UserEvents[user.Id] = eventParticipants.Where(ep => ep.UserId == user.Id).ToList();
            }
            AllEvents = await _context.Events.ToListAsync();
        }

        public async Task<IActionResult> OnPostApproveJoinAsync(int userId, int eventId)
        {
            var participant = await _context.EventParticipants.FindAsync(eventId, userId);
            if (participant != null)
            {
                participant.Status = "Join Approved";
                _context.Update(participant);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostApproveParticipateAsync(int userId, int eventId)
        {
            var participant = await _context.EventParticipants.FindAsync(eventId, userId);
            if (participant != null)
            {
                participant.Status = "Participation Approved";
                _context.Update(participant);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                var participations = _context.EventParticipants.Where(ep => ep.UserId == userId);
                _context.EventParticipants.RemoveRange(participations);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostApproveCertificateAsync(int userId, int eventId)
        {
            var participant = await _context.EventParticipants.FindAsync(eventId, userId);
            if (participant != null)
            {
                participant.Status = "Certificate Approved";
                _context.Update(participant);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeclineCertificateAsync(int userId, int eventId)
        {
            var participant = await _context.EventParticipants.FindAsync(eventId, userId);
            if (participant != null)
            {
                participant.Status = "Certificate Declined";
                _context.Update(participant);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostIssueCertificateAsync(int userId, int eventId)
        {
            var participant = await _context.EventParticipants.FindAsync(eventId, userId);
            if (participant != null)
            {
                participant.Status = "Certificate Issued";
                _context.Update(participant);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAdminJoinEventAsync(int userId, int eventId)
        {
            if (eventId == 0)
            {
                return RedirectToPage();
            }
            var existingParticipation = await _context.EventParticipants.FindAsync(eventId, userId);
            if (existingParticipation == null)
            {
                var participant = new EventParticipant
                {
                    UserId = userId,
                    EventId = eventId,
                    Status = "Joined"
                };
                _context.EventParticipants.Add(participant);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAdminParticipateEventAsync(int userId, int eventId)
        {
            var participant = await _context.EventParticipants.FindAsync(eventId, userId);
            if (participant != null)
            {
                participant.Status = "Participated";
                _context.Update(participant);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
