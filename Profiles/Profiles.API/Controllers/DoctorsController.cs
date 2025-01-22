using AutoMapper;
using CommunityToolkit.HighPerformance;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profiles.API.DTOs;
using Profiles.API.Validators;
using Profiles.Application.Exceptions;
using Profiles.Application.Models;
using Profiles.Application.Models.Enums;
using Profiles.Application.Services.Abstractions;
using System;
using System.IO;
using System.Security.Claims;

namespace Profiles.API.Controllers;

/// <summary>
/// Controller for managing doctors.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    private readonly ILogger<DoctorsController> _logger;
    private readonly IMapper _mapper;

    // Validators

    private readonly IValidator<CreateAccountDto> _createAccountDtoValidator;

    private readonly IValidator<CreateDoctorDto> _createDoctorDtoValidator;
    private readonly IValidator<UpdateDoctorDto> _updateDoctorDtoValidator;
    private readonly IValidator<UpdateDoctorByReceptionistDto> _updateDoctorByReceptionistDtoValidator;

    public DoctorsController(IDoctorService doctorService, ILogger<DoctorsController> logger, IMapper mapper,
        IValidator<CreateAccountDto> createAccountDtoValidator,
        IValidator<CreateDoctorDto> createDoctorDtoValidator,
        IValidator<UpdateDoctorDto> updateDoctorDtoValidator,
        IValidator<UpdateDoctorByReceptionistDto> updateDoctorByReceptionistDtoValidator)
    {
        _doctorService = doctorService;
        _logger = logger;
        _mapper = mapper;
        _createAccountDtoValidator = createAccountDtoValidator;
        _createDoctorDtoValidator = createDoctorDtoValidator;
        _updateDoctorDtoValidator = updateDoctorDtoValidator;
        _updateDoctorByReceptionistDtoValidator = updateDoctorByReceptionistDtoValidator;
    }

    /// <summary>
    /// Gets the list of all doctors.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/Doctors
    ///
    /// </remarks>
    /// <returns>Returns a list of doctors objects.</returns>
    /// <response code="200">Returns the list of doctors</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet]
    [Authorize(Roles = "Receptionist")]
    public async Task<IEnumerable<DoctorDto>> Get(CancellationToken cancellationToken)
    {
        var doctors = await _doctorService.GetAll(cancellationToken);
        _logger.LogInformation("Requested doctors list");

        var doctorDtos = _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        return doctorDtos;
    }

    /// <summary>
    /// Gets a doctor by ID.
    /// </summary>
    /// <param name="id">The ID of the doctor to retrieve.</param>
    /// <returns>Returns the doctor object.</returns>
    /// <response code="200">If the doctor is found</response>
    /// <response code="400">If validation errors occured</response>
    /// <response code="404">If the doctor is not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("me")]
    [Authorize(Roles = "Doctor")]
    public async Task<DoctorDto> GetMyProfile(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            throw new ForbiddenException("You are not allowed to access this resource");
        }

        var doctor = await _doctorService.GetByAccountId(Guid.Parse(userId), cancellationToken);
        _logger.LogInformation("Requested doctor with id {id}", userId);
        return _mapper.Map<DoctorDto>(doctor);
    }

    /// <summary>
    /// Create a doctor by ID.
    /// </summary>
    /// <param name="createDoctorDto">The doctor object fields containing details.</param>
    /// <param name="photo">Photo for profile image</param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">If the doctor is created</response>
    /// <response code="400">If validation errors occured</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost]
    [Authorize(Roles = "Receptionist")]
    public async Task Post([FromForm] CreateDoctorDto createDoctorDto, IFormFile? photo, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            _logger.LogWarning("Unauthorized access to create doctor");
            throw new ForbiddenException("You are not allowed to access this resource");
        }

        await _createAccountDtoValidator.ValidateAndThrowAsync(createDoctorDto.Account, cancellationToken: cancellationToken);
        await _createDoctorDtoValidator.ValidateAndThrowAsync(createDoctorDto, cancellationToken: cancellationToken);

        var createDoctorModel = _mapper.Map<CreateDoctorModel>(createDoctorDto);

        var createAccountModel = new CreateAccountModel
        {
            Email = createDoctorDto.Account.Email,
            PhoneNumber = createDoctorDto.Account.PhoneNumber,
            AuthorId = Guid.Parse(userId),
            IsEmailVerified = false,
            PhotoFileName = String.Empty,
            Role = RoleModel.Doctor
        };

        if (photo != null)
        {
            using var stream = photo.OpenReadStream();
            var fileModel = new FileModel(Guid.NewGuid().ToString(), stream, photo.ContentType);

            createAccountModel.PhotoFileName = fileModel.FileName;

            await _doctorService.Create(
                createDoctorModel,
                fileModel,
                createAccountModel,
                cancellationToken);
        }
        else
        {
            await _doctorService.Create(
                createDoctorModel,
                null,
                createAccountModel,
                cancellationToken);
        }

        _logger.LogInformation("New doctor was successfully created");
    }

    /// <summary>
    /// Update a doctor by ID.
    /// </summary>
    /// <param name="updateDoctorDto">The doctor object fields containing details.</param>
    /// <param name="photo">Photo for profile image</param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">If the doctor is updated</response>
    /// <response code="404">If the doctor is not found</response>
    /// <response code="400">If validation errors occured</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPut]
    [Authorize]
    public async Task Put([FromForm] UpdateDoctorDto updateDoctorDto, IFormFile? photo, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        await _updateDoctorDtoValidator.ValidateAndThrowAsync(updateDoctorDto, cancellationToken: cancellationToken);

        var existingDoctorModel = await _doctorService.Get(updateDoctorDto.Id, cancellationToken);
        if (existingDoctorModel == null)
        {
            throw new NotFoundException($"Doctor with id: {updateDoctorDto.Id} is not found. Can't update.");
        }

        if (userId == null || (Guid.Parse(userId) != existingDoctorModel.AccountId && !User.IsInRole("Receptionist")))
        {
            _logger.LogWarning("Unauthorized access to doctor with id {id}", updateDoctorDto.Id);
            throw new ForbiddenException("You are not allowed to access this resource");
        }

        _mapper.Map(updateDoctorDto, existingDoctorModel);
        var updateDoctorModel = _mapper.Map<UpdateDoctorModel>(existingDoctorModel);
        updateDoctorModel.AuthorId = Guid.Parse(userId);

        if (photo != null)
        {
            using var stream = photo.OpenReadStream();
            var fileModel = new FileModel(Guid.NewGuid().ToString(), stream, photo.ContentType);

            await _doctorService.Update(
                updateDoctorModel,
                fileModel,
                cancellationToken);
        }
        else
        {
            await _doctorService.Update(
                updateDoctorModel,
                null,
                cancellationToken);
        }

        _logger.LogInformation("Doctor with id {id} was successfully updated", updateDoctorDto.Id);
    }

    /// <summary>
    /// Update a doctor by ID as Receptionist.
    /// </summary>
    /// <param name="updateDoctorByReceptionistDto">The doctor object fields containing details.</param>
    /// <response code="200">If the doctor is updated</response>
    /// <response code="404">If the doctor is not found</response>
    /// <response code="400">If validation errors occured</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPut("as-receptionist")]
    [Authorize(Roles = "Receptionist")]
    public async Task Put([FromForm] UpdateDoctorByReceptionistDto updateDoctorByReceptionistDto, IFormFile? photo, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        await _updateDoctorByReceptionistDtoValidator.ValidateAndThrowAsync(updateDoctorByReceptionistDto, cancellationToken: cancellationToken);

        var existingDoctorModel = await _doctorService.Get(updateDoctorByReceptionistDto.Id, cancellationToken);
        if (existingDoctorModel == null)
        {
            throw new NotFoundException($"Doctor with id: {updateDoctorByReceptionistDto.Id} is not found. Can't update.");
        }

        _mapper.Map(updateDoctorByReceptionistDto, existingDoctorModel);
        var updateDoctorModel = _mapper.Map<UpdateDoctorModel>(existingDoctorModel);
        updateDoctorModel.AuthorId = Guid.Parse(userId);

        if (photo != null)
        {
            using var stream = photo.OpenReadStream();
            var fileModel = new FileModel(Guid.NewGuid().ToString(), stream, photo.ContentType);

            await _doctorService.Update(
                updateDoctorModel,
                fileModel,
                cancellationToken);
        }
        else
        {
            await _doctorService.Update(
            updateDoctorModel,
            null,
            cancellationToken);
        }

        _logger.LogInformation("Doctor with id {id} was successfully updated", updateDoctorByReceptionistDto.Id);
    }

    /// <summary>
    /// Delete a doctor by ID.
    /// </summary>
    /// <param name="id">The doctor Id of object to delete.</param>
    /// <response code="200">If the doctor is deleted</response>
    /// <response code="404">If the doctor is not found</response>
    /// <response code="400">If validation errors occured</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Receptionist")]
    public async Task Delete(string id, CancellationToken cancellationToken)
    {
        await _doctorService.Delete(Guid.Parse(id), cancellationToken);
        _logger.LogInformation("Doctor with id {id} was successfully deleted", id);
    }
}
