using FinTrack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinTrack.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly SignInManager<FinTrackUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<FinTrackUser> _userManager;

        public IndexModel(ILogger<IndexModel> logger, SignInManager<FinTrackUser> signInManager, RoleManager<IdentityRole> roleManager, UserManager<FinTrackUser> userManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return LocalRedirect("/Dashboard/Index");
            }

            return Page();
        }
    }
}