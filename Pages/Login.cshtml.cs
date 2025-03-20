using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using sample_1.Data;
using System.Security.Claims;

namespace sample_1.Pages
{
    public class LoginModel(ApplicationDbContext db) : PageModel
    {
        private readonly ApplicationDbContext _db = db;

        [BindProperty]
        public string RegisterNo { get; set; } = string.Empty;

        [BindProperty]
        public string Dob { get; set; } = string.Empty;

        public void OnGet()
        {
            
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var student = _db.Student_Details.FirstOrDefault(s => s.RegisterNo == RegisterNo);

            if (student == null)
            {
                ModelState.AddModelError(string.Empty, "Student not found.");
                return Page();
            }

            if (student.Dob == Dob)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, student.Name ?? string.Empty),
            new Claim("RegisterNo", student.RegisterNo ?? string.Empty),
            new Claim("Department", student.Dept ?? string.Empty),
            new Claim("Year", student.Year.ToString()),
            new Claim("Semester", student.Semester.ToString())
        };
                foreach (var claim in claims)
                {
                    Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToPage("/Index");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }

    }
}

