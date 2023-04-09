#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Yoda.Models.Entity;

namespace YodaNotes.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [StringLength(255, ErrorMessage = "The name field should have a maximum of 255 characters.")]
            [Required]
            [Display(Name = "Name")]
            public string FirstName { get; set; }


            [StringLength(255, ErrorMessage = "The surname field should have a maximum of 255 characters.")]
            [Required]
            [Display(Name = "Surname")]
            public string LastName { get; set; }
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {

                var user = CreateUser();
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
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
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);



                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        var callCode = HtmlEncoder.Default.Encode(callbackUrl);
                        await _emailSender.SendEmailAsync(Input.Email, $"{Input.FirstName} {Input.LastName} confirm your email.", $"<div style=\"background-size: cover; background-color:#252525\"><br /><div class=\"container\"><p style=\"text-align:center\"><img src=\"https://scontent.fdnk3-2.fna.fbcdn.net/v/t39.30808-6/338024357_244038684732737_7035756382940797957_n.jpg?_nc_cat=107&ccb=1-7&_nc_sid=730e14&_nc_ohc=IKBrO-igW74AX8xo40W&_nc_ht=scontent.fdnk3-2.fna&oh=00_AfDJFY5MyKJ5EoSfMixIhZU2tcuZchf9tCn1MpW0uqg-2A&oe=643706D4\" style=\"height:89px; width:200px\" /></p></div><p style=\"margin-left:40px\"><span style=\"color:#ffffff\"><span>Hi {Input.FirstName},</span></span></p><p style=\"margin-left:40px\"><span style=\"color:#ffffff\"><span>You have successfully created Yoda account. Please click button below to verify your email address and complete your registration.</span></span></p><p style=\"margin-left:40px\"><a style=\"color:#89DC02\" href=\"{callCode}\" target=\"\\&quot;_blank\\&quot;\">Verify your Email</a></p><p style=\"margin-left:40px\"><span style=\"color:#ffffff\"><strong>Having trouble?</strong><br />If above method doesn&#39;t work try copying and pasting this link into your browser.</span><br /><a style=\"color:#89DC02\" href=\"callCode\" target=\"\\&quot;_blank\\&quot;\">{callCode}</a></p><p style=\"margin-left:40px\"><span style=\"color:#ffffff\">Best Regards,<br />Team Yoda</span></p><hr /><p style=\"text-align:center\"><br /><span style=\"color:#999999\">You are receiving this email as you subscribed at Yoda. Visit our Privacy Policy to learn your rights and how we use your data.</span><br />&nbsp;</p></div>");
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                    $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
    }
}