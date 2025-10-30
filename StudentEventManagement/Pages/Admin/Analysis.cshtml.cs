using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentEventManagement.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StudentEventManagement.Pages.Admin
{
    public class AnalysisModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AnalysisModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<EventParticipationData> EventParticipation { get; set; } = new List<EventParticipationData>();

        public async Task OnGetAsync()
        {
            EventParticipation = await _context.EventParticipants
                .Include(ep => ep.Event)
                .Include(ep => ep.User)
                .Select(ep => new EventParticipationData
                {
                    EventTitle = ep.Event != null ? ep.Event.Title : "N/A",
                    UserName = ep.User != null ? ep.User.Username : "N/A",
                    Status = ep.Status
                })
                .ToListAsync();
        }
    }

    public class EventParticipationData
    {
        public string EventTitle { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
