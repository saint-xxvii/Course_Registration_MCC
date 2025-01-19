using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace sample_1.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            // Sign out the user from the authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the login page
            return RedirectToPage("/Login");
        }
    }
}
