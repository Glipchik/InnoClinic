using Authorization.Application.Services.Abstractions;
using Authorization.Presentation.Pages.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authorization.Presentation.Pages.Account.SendCode
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly IEmailTokenStoreService _emailTokenStoreService;
        private readonly IEmailService _emailService;

        [BindProperty]
        public InputModel Input { get; set; } = default!;

        public IndexModel(
            IEmailTokenStoreService emailTokenStoreService,
            IEmailService emailService)
        {
            _emailTokenStoreService = emailTokenStoreService;
            _emailService = emailService;
        }

        public async Task<IActionResult> OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                if (Input.Email == null)
                {
                    ModelState.AddModelError(string.Empty, "Email is empty");
                    return Page();
                }

                string token = Guid.NewGuid().ToString();

                try
                {
                    _emailTokenStoreService.StoreToken(Input.Email, token);

                    await _emailService.SendEmailAsync(Input.Email, "Password reset", $"Your password reset token is: {token}", CancellationToken.None);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Email sending", ex.Message);
                }

                return RedirectToPage("/Account/ResetPassword/Index", new { email = Input.Email });
            }

            return Page();
        }
    }
}
