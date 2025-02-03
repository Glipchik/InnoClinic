using Authorization.Application.Exceptions;
using Authorization.Application.Models;
using Authorization.Application.Models.Enums;
using Authorization.Application.Services.Abstractions;
using Authorization.Presentation.DTOs;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountsController> _logger;
        private readonly IValidator<CreateAccountDto> _createAccountDtoValidator;
        private readonly IValidator<ResetPasswordDto> _resetPasswordDtoValidator;
        private readonly IMapper _mapper;
        private readonly IEmailTokenStoreService _emailTokenStoreService;
        private readonly IEmailService _emailService;

        public AccountsController(IAccountService accountService, ILogger<AccountsController> logger, IMapper mapper,
            IValidator<CreateAccountDto> createAccountDtoValidator,
            IValidator<ResetPasswordDto> resetPasswordDtoValidator,
            IEmailTokenStoreService emailTokenStoreService,
            IEmailService emailService)
        {
            _accountService = accountService;
            _logger = logger;
            _mapper = mapper;
            _createAccountDtoValidator = createAccountDtoValidator;
            _emailTokenStoreService = emailTokenStoreService;
            _emailService = emailService;
            _resetPasswordDtoValidator = resetPasswordDtoValidator;
        }

        [HttpPost]
        [Authorize(Policy = "RequireCreateAccountScope")]
        public async Task<AccountModel> CreateAccount([FromBody] CreateAccountDto createAccountDto, CancellationToken cancellationToken)
        {
            await _createAccountDtoValidator.ValidateAndThrowAsync(createAccountDto, cancellationToken);

            var accountCreateModel = new CreateAccountModel
            (
                Email: createAccountDto.Email,
                PhoneNumber: createAccountDto.PhoneNumber,
                Role: (RoleModel)createAccountDto.RoleId,
                null
            );
            var createdAccountModel = await _accountService.CreateAccount(accountCreateModel, cancellationToken, null, isCreatingPatientRequired: false);
            _logger.LogInformation("New account was successfully created");

            return createdAccountModel;
        }

        [HttpPost("request-reset")]
        public async Task RequestPasswordReset([FromBody] string email)
        {
            string token = Guid.NewGuid().ToString();
            _emailTokenStoreService.StoreToken(email, token);

            await _emailService.SendEmailAsync(email, "Password reset", $"Your password reset token is: {token}", CancellationToken.None);
        }

        [HttpPost("reset-password")]
        public async Task ResetPassword([FromBody] ResetPasswordDto request)
        {
            await _resetPasswordDtoValidator.ValidateAndThrowAsync(request);

            if (!_emailTokenStoreService.ValidateToken(request.Email, request.Token))
            {
                throw new BadRequestException("Invalid or expired token.");
            }

            _emailTokenStoreService.RemoveToken(request.Email);

            await _accountService.ResetPassword(request.Email, request.NewPassword, CancellationToken.None);
        }
    }
}