using FinTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinTrack.Pages.Dashboard
{
    [Authorize]
    public class SignOutModel : PageModel
    {
        private readonly SignInManager<FinTrackUser> _signInManager;
        private readonly ILogger<SignOutModel> _logger;

        public SignOutModel(SignInManager<FinTrackUser> signInManager, ILogger<SignOutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnPost(string? returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
