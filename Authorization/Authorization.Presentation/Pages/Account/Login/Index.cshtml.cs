// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Authorization.Application.Models;
using Authorization.Application.Services.Abstractions;
using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Duende.IdentityServer.Test;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Authorization.Presentation.Pages.Login
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class Index : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IEventService _events;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IIdentityProviderStore _identityProviderStore;

        public ViewModel View { get; set; } = default!;

        [BindProperty]
        public InputModel Input { get; set; } = default!;

        public Index(
            IIdentityServerInteractionService interaction,
            IAuthenticationSchemeProvider schemeProvider,
            IIdentityProviderStore identityProviderStore,
            IEventService events,
            IAccountService accountService)
        {
            _accountService = accountService;

            _interaction = interaction;
            _schemeProvider = schemeProvider;
            _identityProviderStore = identityProviderStore;
            _events = events;
        }

        public async Task<IActionResult> OnGet(string? returnUrl)
        {
            await BuildModelAsync(returnUrl);

            if (View.IsExternalLoginOnly)
            {
                return RedirectToPage("/ExternalLogin/Challenge", new { scheme = View.ExternalLoginScheme, returnUrl });
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
        {
            var context = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);

            if (Input.Button != "login")
            {
                if (context != null)
                {
                    ArgumentNullException.ThrowIfNull(Input.ReturnUrl, nameof(Input.ReturnUrl));

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

            if (ModelState.IsValid)
            {
                var validateCredentialModel = new CredentialsModel(Input.Email, Input.Password);
                if (await _accountService.AreCredentialsValid(validateCredentialModel, cancellationToken))
                {
                    var account = await _accountService.FindByEmail(Input.Email, cancellationToken);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(account.Email, account.Id.ToString(), account.Email, clientId: context?.Client.ClientId));
                    Telemetry.Metrics.UserLogin(context?.Client.ClientId, IdentityServerConstants.LocalIdentityProvider);

                    var props = new AuthenticationProperties();
                    if (LoginOptions.AllowRememberLogin && Input.RememberLogin)
                    {
                        props.IsPersistent = true;
                        props.ExpiresUtc = DateTimeOffset.UtcNow.Add(LoginOptions.RememberMeLoginDuration);
                    };

                    var isuser = new IdentityServerUser(account.Id.ToString())
                    {
                        DisplayName = account.Email,
                        AdditionalClaims = new List<Claim>
                        {
                            new Claim(JwtClaimTypes.Role, account.Role.ToString())
                        }
                    };

                    await HttpContext.SignInAsync(isuser, props);

                    if (context != null)
                    {
                        ArgumentNullException.ThrowIfNull(Input.ReturnUrl, nameof(Input.ReturnUrl));

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

                const string error = "invalid credentials";
                await _events.RaiseAsync(new UserLoginFailureEvent(Input.Email, error, clientId: context?.Client.ClientId));
                Telemetry.Metrics.UserLoginFailure(context?.Client.ClientId, IdentityServerConstants.LocalIdentityProvider, error);
                ModelState.AddModelError(string.Empty, LoginOptions.InvalidCredentialsErrorMessage);
            }

            await BuildModelAsync(Input.ReturnUrl);
            return Page();
        }

        private async Task BuildModelAsync(string? returnUrl)
        {
            Input = new InputModel
            {
                ReturnUrl = returnUrl
            };

            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider;

                // this is meant to short circuit the UI and only trigger the one external IdP
                View = new ViewModel
                {
                    EnableLocalLogin = local,
                };

                Input.Email = context.LoginHint;

                if (!local)
                {
                    View.ExternalProviders = new[] { new ViewModel.ExternalProvider(authenticationScheme: context.IdP) };
                }

                return;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var dynamicSchemes = (await _identityProviderStore.GetAllSchemeNamesAsync())
                .Where(x => x.Enabled)
                .Select(x => new ViewModel.ExternalProvider
                (
                    authenticationScheme: x.Scheme,
                    displayName: x.DisplayName ?? x.Scheme
                ));


            var allowLocal = true;
            var client = context?.Client;
            if (client != null)
            {
                allowLocal = client.EnableLocalLogin;
            }

            View = new ViewModel
            {
                AllowRememberLogin = LoginOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && LoginOptions.AllowLocalLogin,
            };
        }
    }
}
