using Authorization.Application.Exceptions;
using Authorization.Application.Services;
using Authorization.Application.Services.Abstractions;
using Authorization.Presentation.Pages.Login;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authorization.Presentation.Pages.Account.ResetPassword
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly IEmailTokenStoreService _emailTokenStoreService;
        private readonly IEmailService _emailService;
        private readonly IAccountService _accountService;

        [BindProperty(SupportsGet = true)]
        public string? Email { get; set; }

        [BindProperty]
        public InputModel Input { get; set; } = default!;

        public IndexModel(
            IEmailTokenStoreService emailTokenStoreService,
            IEmailService emailService,
            IAccountService accountService)
        {
            _emailTokenStoreService = emailTokenStoreService;
            _emailService = emailService;
            _accountService = accountService;
        }

        public async Task<IActionResult> OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                if (Input.EmailToken == null || Input.NewPassword == null || Email == null)
                {
                    ModelState.AddModelError(string.Empty, "Some fields are empty");
                    return Page();
                }

                if (!_emailTokenStoreService.ValidateToken(Email, Input.EmailToken))
                {
                    ModelState.AddModelError("Email Token validation", "Can not validate token");
                    return Page();
                }

                try
                {
                    _emailTokenStoreService.RemoveToken(Email);
                    await _accountService.ResetPassword(Email, Input.NewPassword, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Email sending", ex.Message);
                    return Page();
                }

                return RedirectToPage("/Account/Login/Index");
            }

            return Page();
        }
    }
}
