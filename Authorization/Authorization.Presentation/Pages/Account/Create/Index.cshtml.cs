// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Authorization.Application.Models;
using Authorization.Application.Services.Abstractions;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace Authorization.Presentation.Pages.Create
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class Index : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IIdentityServerInteractionService _interaction;

        [BindProperty]
        public InputModel Input { get; set; } = default!;

        public Index(
            IIdentityServerInteractionService interaction,
            IAccountService accountService)
        {
            _accountService = accountService;
            _interaction = interaction;
        }

        public IActionResult OnGet(string? returnUrl)
        {
            Input = new InputModel { ReturnUrl = returnUrl };
            return Page();
        }

        public async Task<IActionResult> OnPost(CancellationToken cancellation)
        {
            var context = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);

            if (Input.Button != "create")
            {
                if (context != null)
                {
                    await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                    if (context.IsNativeClient())
                    {
                        return this.LoadingPage(Input.ReturnUrl);
                    }

                    return Redirect(Input.ReturnUrl ?? "~/");
                }
                else
                {
                    return Redirect("~/");
                }
            }

            if ((await _accountService.FindByEmail(Input.Email, cancellation)) != null)
            {
                ModelState.AddModelError("Input.Email", "Invalid email");
            }

            if (ModelState.IsValid)
            {
                var createAccountModel = new CreateAccountModel(Input.Email, Input.PhoneNumber, Input.Password);
                var user = await _accountService.CreateAccount(createAccountModel, cancellation);

                var isuser = new IdentityServerUser(user.Id.ToString())
                {
                    DisplayName = user.Email
                };

                await HttpContext.SignInAsync(isuser);

                if (context != null)
                {
                    if (context.IsNativeClient())
                    {
                        return this.LoadingPage(Input.ReturnUrl);
                    }

                    return Redirect(Input.ReturnUrl ?? "~/");
                }

                if (Url.IsLocalUrl(Input.ReturnUrl))
                {
                    return Redirect(Input.ReturnUrl);
                }
                else if (string.IsNullOrEmpty(Input.ReturnUrl))
                {
                    return Redirect("~/");
                }
                else
                {
                    throw new ArgumentException("invalid return URL");
                }
            }

            return Page();
        }
    }
}
