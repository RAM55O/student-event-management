using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentEventManagement.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StudentEventManagement.Pages.Admin
{
    public class FeedbackAnalysisModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public FeedbackAnalysisModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Dictionary<Event, List<Feedback>> FeedbackByEvent { get; set; } = new Dictionary<Event, List<Feedback>>();

        public async Task OnGetAsync()
        {
            var feedback = await _context.Feedback
                .Include(f => f.User)
                .Include(f => f.Event)
                .OrderByDescending(f => f.Timestamp)
                .ToListAsync();

            FeedbackByEvent = feedback.GroupBy(f => f.Event)
                                      .ToDictionary(g => g.Key, g => g.ToList());
        }
    }
}
