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
using Profiles.Domain.Models;

namespace Profiles.API.Controllers
{
    /// <summary>
    /// Controller for managing receptionists.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReceptionistsController : ControllerBase
    {
        private readonly IReceptionistService _receptionistService;
        private readonly ILogger<ReceptionistsController> _logger;
        private readonly IMapper _mapper;

        // Validators

        private readonly IValidator<CreateAccountDto> _createAccountDtoValidator;

        private readonly IValidator<CreateReceptionistDto> _createReceptionistDtoValidator;
        private readonly IValidator<UpdateReceptionistDto> _updateReceptionistDtoValidator;

        public ReceptionistsController(IReceptionistService receptionistService, ILogger<ReceptionistsController> logger, IMapper mapper,
            IValidator<CreateAccountDto> createAccountDtoValidator,
            IValidator<CreateReceptionistDto> createReceptionistDtoValidator,
            IValidator<UpdateReceptionistDto> updateReceptionistDtoValidator)
        {
            _receptionistService = receptionistService;
            _logger = logger;
            _mapper = mapper;
            _createAccountDtoValidator = createAccountDtoValidator;
            _createReceptionistDtoValidator = createReceptionistDtoValidator;
            _updateReceptionistDtoValidator = updateReceptionistDtoValidator;
        }

        /// <summary>
        /// Gets the list of all receptionists.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Receptionists
        ///
        /// </remarks>
        /// <returns>Returns a list of receptionists objects.</returns>
        /// <response code="200">Returns the list of receptionists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        [Authorize(Roles = "Receptionist")]
        public async Task<IEnumerable<ReceptionistDto>> Get(ReceptionistQueryParametresModel receptionistQueryParametresModel, CancellationToken cancellationToken)
        {
            var receptionists = await _receptionistService.GetAll(cancellationToken, receptionistQueryParametresModel);
            _logger.LogInformation("Requested receptionists list");

            var receptionistDtos = _mapper.Map<IEnumerable<ReceptionistDto>>(receptionists);
            return receptionistDtos;
        }

        /// <summary>
        /// Gets the list of all receptionists with-pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Receptionists/with-pagination
        ///
        /// </remarks>
        /// <returns>Returns a list of receptionists objects.</returns>
        /// <response code="200">Returns the list of receptionists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("with-pagination")]
        [Authorize(Roles = "Receptionist")]
        public async Task<PaginatedList<ReceptionistDto>> Get(ReceptionistQueryParametresModel receptionistQueryParametresModel, CancellationToken cancellationToken,  [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var receptionists = await _receptionistService.GetAll(cancellationToken, receptionistQueryParametresModel, pageIndex, pageSize);
            _logger.LogInformation("Requested receptionists list");

            var receptionistDtos = _mapper.Map<IEnumerable<ReceptionistDto>>(receptionists.Items);
            return new PaginatedList<ReceptionistDto>([.. receptionistDtos], receptionists.PageIndex, receptionists.TotalPages);
        }

        /// <summary>
        /// Gets a receptionist by ID.
        /// </summary>
        /// <param name="id">The ID of the receptionist to retrieve.</param>
        /// <returns>Returns the receptionist object.</returns>
        /// <response code="200">If the receptionist is found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="404">If the receptionist is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("me")]
        [Authorize(Roles = "Receptionist")]
        public async Task<ReceptionistDto> GetMyProfile(CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                throw new ForbiddenException("You are not allowed to access this resource");
            }

            var receptionist = await _receptionistService.GetByAccountId(Guid.Parse(userId), cancellationToken);
            _logger.LogInformation("Requested receptionist with id {id}", userId);
            return _mapper.Map<ReceptionistDto>(receptionist);
        }

        /// <summary>
        /// Create a receptionist by ID.
        /// </summary>
        /// <param name="createReceptionistDto">The receptionist object fields containing details.</param>
        /// <param name="photo">Photo for profile image</param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">If the receptionist is created</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost]
        [Authorize(Roles = "Receptionist")]
        public async Task Post([FromForm] CreateReceptionistDto createReceptionistDto, IFormFile? photo, CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                _logger.LogWarning("Unauthorized access to create receptionist");
                throw new ForbiddenException("You are not allowed to access this resource");
            }

            await _createReceptionistDtoValidator.ValidateAndThrowAsync(createReceptionistDto, cancellationToken: cancellationToken);
            await _createAccountDtoValidator.ValidateAndThrowAsync(createReceptionistDto.Account, cancellationToken: cancellationToken);

            var createReceptionistModel = _mapper.Map<CreateReceptionistModel>(createReceptionistDto);

            var createAccountModel = new CreateAccountModel
            {
                Email = createReceptionistDto.Account.Email,
                PhoneNumber = createReceptionistDto.Account.PhoneNumber,
                AuthorId = Guid.Parse(userId),
                IsEmailVerified = false,
                PhotoFileName = String.Empty,
                Role = RoleModel.Receptionist
            };

            if (photo != null)
            {
                using var stream = photo.OpenReadStream();
                var fileModel = new FileModel(Guid.NewGuid().ToString(), stream, photo.ContentType);

                createAccountModel.PhotoFileName = fileModel.FileName;

                await _receptionistService.Create(
                    createReceptionistModel,
                    createAccountModel,
                    fileModel,
                    cancellationToken);
            }
            else
            {
                await _receptionistService.Create(
                    createReceptionistModel,
                    createAccountModel,
                    null,
                    cancellationToken);
            }

            _logger.LogInformation("New receptionist was successfully created");
        }

        /// <summary>
        /// Update a receptionist by ID as Receptionist.
        /// </summary>
        /// <param name="updateReceptionistDto">The receptionist object fields containing details.</param>
        /// <response code="200">If the receptionist is updated</response>
        /// <response code="404">If the receptionist is not found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut]
        [Authorize(Roles = "Receptionist")]
        public async Task Put([FromForm] UpdateReceptionistDto updateReceptionistDto, IFormFile? photo, CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _updateReceptionistDtoValidator.ValidateAndThrowAsync(updateReceptionistDto, cancellationToken: cancellationToken);

            var existingReceptionistModel = await _receptionistService.Get(updateReceptionistDto.Id, cancellationToken);
            if (existingReceptionistModel == null)
            {
                throw new NotFoundException($"Receptionist with id: {updateReceptionistDto.Id} is not found. Can't update.");
            }

            _mapper.Map(updateReceptionistDto, existingReceptionistModel);
            var updateReceptionistModel = _mapper.Map<UpdateReceptionistModel>(existingReceptionistModel);
            updateReceptionistModel.AuthorId = Guid.Parse(userId);

            if (photo != null)
            {
                using var stream = photo.OpenReadStream();
                var fileModel = new FileModel(Guid.NewGuid().ToString(), stream, photo.ContentType);

                await _receptionistService.Update(
                    updateReceptionistModel,
                    fileModel,
                    cancellationToken);
            }
            else
            {

                await _receptionistService.Update(
                    updateReceptionistModel,
                    null,
                    cancellationToken);
            }

            _logger.LogInformation("Receptionist with id {id} was successfully updated", updateReceptionistDto.Id);
        }

        /// <summary>
        /// Delete a receptionist by ID.
        /// </summary>
        /// <param name="id">The receptionist Id of object to delete.</param>
        /// <response code="200">If the receptionist is deleted</response>
        /// <response code="404">If the receptionist is not found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Receptionist")]
        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            await _receptionistService.Delete(Guid.Parse(id), cancellationToken);
            _logger.LogInformation("Receptionist with id {id} was successfully deleted", id);
        }
    }
}
