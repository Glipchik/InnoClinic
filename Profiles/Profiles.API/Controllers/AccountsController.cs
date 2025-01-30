using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profiles.API.DTOs;
using Profiles.Application.Exceptions;
using Profiles.Application.Models.Enums;
using Profiles.Application.Models;
using Profiles.Application.Services.Abstractions;
using System.Security.Claims;

namespace Profiles.API.Controllers
{
    /// <summary>
    /// Controller for managing accounts.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountsController> _logger;
        private readonly IMapper _mapper;

        public AccountsController(IAccountService accountService, ILogger<AccountsController> logger, IMapper mapper)
        {
            _accountService = accountService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a account by ID.
        /// </summary>
        /// <param name="id">The ID of the account to retrieve.</param>
        /// <returns>Returns the account object.</returns>
        /// <response code="200">If the account is found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="404">If the account is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<AccountDto> Get(string id, CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null || (Guid.Parse(userId) != Guid.Parse(id) && !User.IsInRole("Receptionist")))
            {
                _logger.LogWarning("Unauthorized access to doctor with id {id}", Guid.Parse(id));
                throw new ForbiddenException("You are not allowed to access this resource");
            }

            var account = await _accountService.Get(Guid.Parse(id), cancellationToken);
            _logger.LogInformation("Requested account with id {id}", id);
            return _mapper.Map<AccountDto>(account);
        }
    }

}
