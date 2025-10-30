using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentEventManagement.Data;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace StudentEventManagement.Pages.Events
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

                [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel
        {
            public string? Title { get; set; }
            public string? Description { get; set; }
            public DateTime Date { get; set; }
        }

        public void OnGet()
        {
            var role = HttpContext.Session.GetString("role");
            if (role != "Admin")
            {
                RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var role = HttpContext.Session.GetString("role");
            if (role != "Admin")
            {
                return RedirectToPage("/Index");
            }

            if (ModelState.IsValid)
            {
                var username = HttpContext.Session.GetString("username");
                var user = _context.Users.FirstOrDefault(u => u.Username == username);

                if (user != null)
                {
                    var eventItem = new Event
                    {
                        Title = Input.Title,
                        Description = Input.Description,
                        Date = Input.Date,
                        CreatedBy = user.Id
                    };

                    _context.Events.Add(eventItem);
                    await _context.SaveChangesAsync();

                    return RedirectToPage("/Events/Index");
                }
            }

            return Page();
        }
    }
}
