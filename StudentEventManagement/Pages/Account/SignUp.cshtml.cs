
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentEventManagement.Data;

namespace StudentEventManagement.Pages.Account
{
    public class SignUpModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SignUpModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel
        {
            public string? FirstName { get; set; }
            public string? Surname { get; set; }
            public string? MobileNumber { get; set; }
            public string? Email { get; set; }
            public string? Username { get; set; }
            public string? Password { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid && Input.Password != null)
            {
                var user = new User
                {
                    FirstName = Input.FirstName,
                    Surname = Input.Surname,
                    MobileNumber = Input.MobileNumber,
                    Email = Input.Email,
                    Username = Input.Username,
                    Password = Input.Password,
                    Role = "User"
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToPage("/EventHome");
            }

            return Page();
        }
    }
}
