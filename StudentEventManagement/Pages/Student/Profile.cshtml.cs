using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentEventManagement.Data;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace StudentEventManagement.Pages.Student
{
    public class ProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ProfileModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User? StudentUser { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToPage("/Account/Login");
            }

            StudentUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userName && u.Role == "Student");

            if (StudentUser == null)
            {
                return RedirectToPage("/Account/Login"); // Or an access denied page
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userToUpdate = await _context.Users.FindAsync(StudentUser.Id);

            if (userToUpdate == null)
            {
                return NotFound();
            }

            userToUpdate.MobileNumber = StudentUser.MobileNumber;
            userToUpdate.Email = StudentUser.Email;

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Account/Login");
        }
    }
}