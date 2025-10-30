using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentEventManagement.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StudentEventManagement.Pages.Student
{
    public class MyEventsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MyEventsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<EventParticipationData> MyParticipations { get; set; } = new List<EventParticipationData>();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = GetCurrentUserId();
            if (userId == -1)
            {
                return RedirectToPage("/Account/Login");
            }

            MyParticipations = await _context.EventParticipants
                .Where(ep => ep.UserId == userId)
                .Include(ep => ep.Event)
                .Select(ep => new EventParticipationData
                {
                    EventTitle = ep.Event != null ? ep.Event.Title : "N/A",
                    Status = ep.Status,
                    EventDate = ep.Event != null ? ep.Event.Date : DateTime.MinValue
                })
                .OrderBy(ep => ep.EventDate)
                .ToListAsync();

            return Page();
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

    // Reusing EventParticipationData from Admin Analysis for consistency
    public class EventParticipationData
    {
        public string EventTitle { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
    }
}
