using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace sample_1.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            // Check if the user is authenticated, otherwise redirect to Login
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToPage("/Login");
            }
        }
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

    }
}
