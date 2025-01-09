using Authorization.Application.Models;
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
        private readonly IMapper _mapper;

        public AccountsController(IAccountService accountService, ILogger<AccountsController> logger, IMapper mapper,
            IValidator<CreateAccountDto> createAccountDtoValidator)
        {
            _accountService = accountService;
            _logger = logger;
            _mapper = mapper;
            _createAccountDtoValidator = createAccountDtoValidator;
        }

        [HttpPost]
        [Authorize(Policy = "CreateAccountScope")]
        public async Task<AccountModel> CreateAccount([FromBody] CreateAccountDto createAccountDto, CancellationToken cancellationToken)
        {
            await _createAccountDtoValidator.ValidateAndThrowAsync(createAccountDto, cancellationToken);

            var accountCreateModel = _mapper.Map<CreateAccountModel>(createAccountDto);
            var createdAccountModel = await _accountService.CreateAccount(accountCreateModel, cancellationToken);
            _logger.LogInformation("New account was successfully created");

            return createdAccountModel;
        }
    }
}
