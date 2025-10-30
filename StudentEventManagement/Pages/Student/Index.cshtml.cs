using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentEventManagement.Data;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace StudentEventManagement.Pages.Student
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Event>? Events { get; set; }
        public List<EventParticipant> UserEventParticipants { get; set; } = new List<EventParticipant>();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = GetCurrentUserId();
            if (userId == -1)
            {
                return RedirectToPage("/Account/Login");
            }

            Events = await _context.Events.ToListAsync();
            UserEventParticipants = await _context.EventParticipants
                .Where(ep => ep.UserId == userId)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostJoinAsync(int eventId)
        {
            var userId = GetCurrentUserId();
            if (userId == -1)
            {
                return RedirectToPage("/Account/Login");
            }

            var existingParticipant = await _context.EventParticipants
                .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.UserId == userId);

            if (existingParticipant == null)
            {
                var participant = new EventParticipant
                {
                    EventId = eventId,
                    UserId = userId,
                    Status = "Joined"
                };
                _context.EventParticipants.Add(participant);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostParticipateAsync(int eventId)
        {
            var userId = GetCurrentUserId();
            if (userId == -1)
            {
                return RedirectToPage("/Account/Login");
            }

            var existingParticipant = await _context.EventParticipants
                .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.UserId == userId);

            if (existingParticipant != null)
            {
                existingParticipant.Status = "Participated";
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        private int GetCurrentUserId()
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return -1;
            }
            var user = _context.Users.FirstOrDefault(u => u.Username == userName);
            return user?.Id ?? -1;
        }
    }
}
