using FinTrack.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;

namespace FinTrack.Pages
{
    public class SignUpModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Full Name")]
            public string FullName { get; set; }

            [Required]
            [Display(Name = "Age")]
            public int Age { get; set; }

            [Required]
            [Display(Name = "Sex")]
            public string Sex { get; set; }

            [Required]
            [Display(Name = "Country")]
            public string Country { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        private readonly SignInManager<FinTrackUser> _signInManager;
        private readonly UserManager<FinTrackUser> _userManager;
        private readonly IUserStore<FinTrackUser> _userStore;
        private readonly IUserEmailStore<FinTrackUser> _emailStore;
        private readonly ILogger<SignUpModel> _logger;
        private readonly IEmailSender _emailSender;

        public SignUpModel(
            UserManager<FinTrackUser> userManager,
            IUserStore<FinTrackUser> userStore,
            SignInManager<FinTrackUser> signInManager,
            ILogger<SignUpModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.FullName = Input.FullName;
                user.Age = Input.Age;
                user.Sex = Input.Sex;
                user.Country = Input.Country;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "", userId = userId, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation");
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect("~/");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        private FinTrackUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<FinTrackUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(FinTrackUser)}'. " +
                    $"Ensure that '{nameof(FinTrackUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<FinTrackUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<FinTrackUser>)_userStore;
        }
    }
}
