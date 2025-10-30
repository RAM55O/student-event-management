using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentEventManagement.Data;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace StudentEventManagement.Pages.Events
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Event>? Events { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }

        public void OnGet()
        {
            Username = HttpContext.Session.GetString("username");
            Role = HttpContext.Session.GetString("role");

            if (Username == null)
            {
                RedirectToPage("/Account/Login");
            }

            Events = _context.Events.ToList();
        }
    }
}
