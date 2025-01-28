using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profiles.API.DTOs;
using Profiles.API.Validators;
using Profiles.Application.Models.Enums;
using Profiles.Application.Models;
using AutoMapper;
using FluentValidation;
using Profiles.Application.Services.Abstractions;
using static Duende.IdentityServer.Models.IdentityResources;

namespace Profiles.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly ILogger<PatientsController> _logger;
        private readonly IMapper _mapper;

        // Validators

        private readonly IValidator<CreatePatientFromAuthServerDto> _createPatientFromAuthServerDtoValidator;
        private readonly IValidator<CreateAccountFromAuthDto> _createAccountFromAuthServerDtoValidator;

        public AuthorizationController(
            IPatientService patientService,
            ILogger<PatientsController> logger,
            IMapper mapper,
            IValidator<CreatePatientFromAuthServerDto> createPatientFromAuthServerDtoValidator,
            IValidator<CreateAccountFromAuthDto> createAccountFromAuthServerDtoValidator)
        {
            _patientService = patientService;
            _logger = logger;
            _mapper = mapper;
            _createPatientFromAuthServerDtoValidator = createPatientFromAuthServerDtoValidator;
            _createAccountFromAuthServerDtoValidator = createAccountFromAuthServerDtoValidator;
        }

        [HttpPost]
        [Authorize(Policy = "RequireCreatePatientProfileScope")]
        public async Task CreatePatient([FromForm] CreatePatientFromAuthServerDto createPatientFromAuthServerDto, IFormFile? photo, CancellationToken cancellationToken)
        {
            await _createPatientFromAuthServerDtoValidator.ValidateAndThrowAsync(createPatientFromAuthServerDto, cancellationToken: cancellationToken);
            await _createAccountFromAuthServerDtoValidator.ValidateAndThrowAsync(createPatientFromAuthServerDto.CreateAccountFromAuthDto, cancellationToken: cancellationToken);

            var createPatientModel = _mapper.Map<CreatePatientModel>(createPatientFromAuthServerDto);

            var createAccountFromAuthServerModel = new CreateAccountFromAuthServerModel
            {
                Id = createPatientFromAuthServerDto.CreateAccountFromAuthDto.Id,
                Email = createPatientFromAuthServerDto.CreateAccountFromAuthDto.Email,
                PhoneNumber = createPatientFromAuthServerDto.CreateAccountFromAuthDto.PhoneNumber,
                AuthorId = createPatientFromAuthServerDto.CreateAccountFromAuthDto.Id,
                IsEmailVerified = false,
                PhotoFileName = String.Empty,
            };

            if (photo != null)
            {
                using var stream = photo.OpenReadStream();
                var fileModel = new FileModel(Guid.NewGuid().ToString(), stream, photo.ContentType);

                createAccountFromAuthServerModel.PhotoFileName = fileModel.FileName;

                await _patientService.CreateFromAuthServer(
                    createPatientModel,
                    createAccountFromAuthServerModel,
                    fileModel,
                    cancellationToken);
            }
            else
            {
                await _patientService.CreateFromAuthServer(
                    createPatientModel,
                    createAccountFromAuthServerModel,
                    null,
                    cancellationToken);
            }

            _logger.LogInformation("New patient was successfully created");
        }
    }
}
