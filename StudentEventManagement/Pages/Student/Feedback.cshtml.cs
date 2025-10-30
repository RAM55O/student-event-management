using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentEventManagement.Data;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace StudentEventManagement.Pages.Student
{
    public class FeedbackModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public FeedbackModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Feedback Feedback { get; set; } = new Feedback();

        public SelectList EventList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = GetCurrentUserId();
            if (userId == -1)
            {
                return RedirectToPage("/Account/Login");
            }

            var allowedStatus = new List<string> { "Participated", "Participation Approved", "Certificate Requested", "Certificate Approved", "Certificate Issued" };
            var participatedEvents = await _context.EventParticipants
                .Where(ep => ep.UserId == userId && allowedStatus.Contains(ep.Status))
                .Select(ep => ep.Event)
                .ToListAsync();

            EventList = new SelectList(participatedEvents, "Id", "Title");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = GetCurrentUserId();
            if (userId == -1)
            {
                return RedirectToPage("/Account/Login");
            }

            Feedback.UserId = userId;
            Feedback.Timestamp = DateTime.UtcNow;

            if (!ModelState.IsValid)
            {
                var allowedStatus = new List<string> { "Participated", "Participation Approved", "Certificate Requested", "Certificate Approved", "Certificate Issued" };
                var participatedEvents = await _context.EventParticipants
                    .Where(ep => ep.UserId == userId && allowedStatus.Contains(ep.Status))
                    .Select(ep => ep.Event)
                    .ToListAsync();
                EventList = new SelectList(participatedEvents, "Id", "Title");
                return Page();
            }

            _context.Feedback.Add(Feedback);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Your feedback has been submitted successfully!";
            return RedirectToPage("./Index");
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
