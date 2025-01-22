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
using Profiles.API.Validators;

namespace Profiles.API.Controllers
{
    /// <summary>
    /// Controller for managing patients.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly ILogger<PatientsController> _logger;
        private readonly IMapper _mapper;

        // Validators

        private readonly IValidator<CreateAccountDto> _createAccountDtoValidator;

        private readonly IValidator<CreatePatientDto> _createPatientDtoValidator;
        private readonly IValidator<UpdatePatientDto> _updatePatientDtoValidator;
        private readonly IValidator<UpdatePatientByReceptionistDto> _updatePatientByReceptionistDtoValidator;

        public PatientsController(IPatientService patientService, ILogger<PatientsController> logger, IMapper mapper,
            IValidator<CreateAccountDto> createAccountDtoValidator,
            IValidator<CreatePatientDto> createPatientDtoValidator,
            IValidator<UpdatePatientDto> updatePatientDtoValidator,
            IValidator<UpdatePatientByReceptionistDto> updatePatientByReceptionistDtoValidator)
        {
            _patientService = patientService;
            _logger = logger;
            _mapper = mapper;
            _createAccountDtoValidator = createAccountDtoValidator;
            _createPatientDtoValidator = createPatientDtoValidator;
            _updatePatientDtoValidator = updatePatientDtoValidator;
            _updatePatientByReceptionistDtoValidator = updatePatientByReceptionistDtoValidator;
        }

        /// <summary>
        /// Gets the list of all patients.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Patients
        ///
        /// </remarks>
        /// <returns>Returns a list of patients objects.</returns>
        /// <response code="200">Returns the list of patients</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        [Authorize(Roles = "Receptionist")]
        public async Task<IEnumerable<PatientDto>> Get(CancellationToken cancellationToken)
        {
            var patients = await _patientService.GetAll(cancellationToken);
            _logger.LogInformation("Requested patients list");

            var patientDtos = _mapper.Map<IEnumerable<PatientDto>>(patients);
            return patientDtos;
        }

        /// <summary>
        /// Gets a patient by ID.
        /// </summary>
        /// <param name="id">The ID of the patient to retrieve.</param>
        /// <returns>Returns the patient object.</returns>
        /// <response code="200">If the patient is found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="404">If the patient is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<PatientDto> Get(string id, CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null || (Guid.Parse(userId) != Guid.Parse(id) && !User.IsInRole("Receptionist")))
            {
                _logger.LogWarning("Unauthorized access to doctor with id {id}", Guid.Parse(id));
                throw new ForbiddenException("You are not allowed to access this resource");
            }

            var patient = await _patientService.Get(Guid.Parse(id), cancellationToken);
            _logger.LogInformation("Requested patient with id {id}", id);
            return _mapper.Map<PatientDto>(patient);
        }

        /// <summary>
        /// Create a patient by ID.
        /// </summary>
        /// <param name="createPatientDto">The patient object fields containing details.</param>
        /// <param name="photo">Photo for profile image</param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">If the patient is created</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost]
        [Authorize(Roles = "Receptionist")]
        public async Task Post([FromForm] CreatePatientDto createPatientDto, IFormFile? photo, CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                _logger.LogWarning("Unauthorized access to create patient");
                throw new ForbiddenException("You are not allowed to access this resource");
            }

            await _createAccountDtoValidator.ValidateAndThrowAsync(createPatientDto.Account, cancellationToken: cancellationToken);
            await _createPatientDtoValidator.ValidateAndThrowAsync(createPatientDto, cancellationToken: cancellationToken);

            var createPatientModel = _mapper.Map<CreatePatientModel>(createPatientDto);

            var createAccountModel = new CreateAccountModel
            {
                Email = createPatientDto.Account.Email,
                PhoneNumber = createPatientDto.Account.PhoneNumber,
                AuthorId = Guid.Parse(userId),
                IsEmailVerified = false,
                PhotoFileName = String.Empty,
                Role = RoleModel.Patient
            };

            if (photo != null)
            {
                using var stream = photo.OpenReadStream();
                var fileModel = new FileModel(Guid.NewGuid().ToString(), stream, photo.ContentType);

                createAccountModel.PhotoFileName = fileModel.FileName;

                await _patientService.Create(
                    createPatientModel,
                    createAccountModel,
                    fileModel,
                    cancellationToken);
            }
            else
            {
                await _patientService.Create(
                    createPatientModel,
                    createAccountModel,
                    null,
                    cancellationToken);
            }

            _logger.LogInformation("New patient was successfully created");
        }

        /// <summary>
        /// Update a patient by ID.
        /// </summary>
        /// <param name="updatePatientDto">The patient object fields containing details.</param>
        /// <param name="photo">Photo for profile image</param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">If the patient is updated</response>
        /// <response code="404">If the patient is not found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut]
        [Authorize]
        public async Task Put([FromForm] UpdatePatientDto updatePatientDto, IFormFile? photo, CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var existingPatientModel = await _patientService.Get(updatePatientDto.Id, cancellationToken);
            if (existingPatientModel == null)
            {
                throw new NotFoundException($"Patient with id: {updatePatientDto.Id} is not found. Can't update.");
            }

            if (userId == null || (Guid.Parse(userId) != existingPatientModel.AccountId && !User.IsInRole("Receptionist")))
            {
                _logger.LogWarning("Unauthorized access to patient with id {id}", updatePatientDto.Id);
                throw new ForbiddenException("You are not allowed to access this resource");
            }

            await _updatePatientDtoValidator.ValidateAndThrowAsync(updatePatientDto, cancellationToken: cancellationToken);


            _mapper.Map(updatePatientDto, existingPatientModel);
            var updatePatientModel = _mapper.Map<UpdatePatientModel>(existingPatientModel);
            updatePatientModel.AuthorId = Guid.Parse(userId);

            if (photo != null)
            {
                using var stream = photo.OpenReadStream();
                var fileModel = new FileModel(Guid.NewGuid().ToString(), stream, photo.ContentType);

                await _patientService.Update(
                    updatePatientModel,
                    fileModel,
                    cancellationToken);
            }
            else
            {
                await _patientService.Update(
                updatePatientModel,
                null,
                cancellationToken);
            }

            _logger.LogInformation("Patient with id {id} was successfully updated", updatePatientDto.Id);
        }

        /// <summary>
        /// Update a patient by ID as Receptionist.
        /// </summary>
        /// <param name="updatePatientByReceptionistDto">The patient object fields containing details.</param>
        /// <response code="200">If the patient is updated</response>
        /// <response code="404">If the patient is not found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("as-receptionist")]
        [Authorize(Roles = "Receptionist")]
        public async Task Put([FromForm] UpdatePatientByReceptionistDto updatePatientByReceptionistDto, IFormFile? photo, CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _updatePatientByReceptionistDtoValidator.ValidateAndThrowAsync(updatePatientByReceptionistDto, cancellationToken: cancellationToken);

            var existingPatientModel = await _patientService.Get(updatePatientByReceptionistDto.Id, cancellationToken);
            if (existingPatientModel == null)
            {
                throw new NotFoundException($"Patient with id: {updatePatientByReceptionistDto.Id} is not found. Can't update.");
            }

            _mapper.Map(updatePatientByReceptionistDto, existingPatientModel);
            var updatePatientModel = _mapper.Map<UpdatePatientModel>(existingPatientModel);
            updatePatientModel.AuthorId = Guid.Parse(userId);

            if (photo != null)
            {
                using var stream = photo.OpenReadStream();
                var fileModel = new FileModel(Guid.NewGuid().ToString(), stream, photo.ContentType);

                await _patientService.Update(
                    updatePatientModel,
                    fileModel,
                    cancellationToken);
            }
            else
            {
                await _patientService.Update(
                    updatePatientModel,
                    null,
                    cancellationToken);
            }

            _logger.LogInformation("Patient with id {id} was successfully updated", updatePatientByReceptionistDto.Id);
        }

        /// <summary>
        /// Delete a patient by ID.
        /// </summary>
        /// <param name="id">The patient Id of object to delete.</param>
        /// <response code="200">If the patient is deleted</response>
        /// <response code="404">If the patient is not found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Receptionist")]
        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            await _patientService.Delete(Guid.Parse(id), cancellationToken);
            _logger.LogInformation("Patient with id {id} was successfully deleted", id);
        }
    }
}
