using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Offices.API.DTOs;
using Offices.API.Extensions;
using Offices.API.Validators;
using Offices.Application.Models;
using Offices.Application.Services.Abstractions;

namespace Offices.API.Controllers
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
        private readonly IValidator<CreateReceptionistDto> _createReceptionistDtoValidator;
        private readonly IValidator<UpdateReceptionistDto> _updateReceptionistDtoValidator;
        private readonly IValidator<ObjectIdDto> _objectIdDtoValidator;

        public ReceptionistsController(IReceptionistService receptionistService, ILogger<ReceptionistsController> logger, IMapper mapper,
            IValidator<CreateReceptionistDto> createReceptionistDtoValidator,
            IValidator<UpdateReceptionistDto> updateReceptionistDtoValidator,
            IValidator<ObjectIdDto> objectIdDtoValidator)
        {
            _receptionistService = receptionistService;
            _logger = logger;
            _mapper = mapper;
            _createReceptionistDtoValidator = createReceptionistDtoValidator;
            _updateReceptionistDtoValidator = updateReceptionistDtoValidator;
            _objectIdDtoValidator = objectIdDtoValidator;
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
        /// <returns>Returns a list of receptionist objects.</returns>
        /// <response code="200">Returns the list of receptionists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        public async Task<IEnumerable<ReceptionistDto>> Get(CancellationToken cancellationToken)
        {
            var receptionists = await _receptionistService.GetAll(cancellationToken);
            _logger.LogInformation("Requested receptionists list");

            var receptionistDtos = _mapper.Map<IEnumerable<ReceptionistDto>>(receptionists);
            return receptionistDtos;
        }

        /// <summary>
        /// Gets an receptionist by ID.
        /// </summary>
        /// <param name="id">The ID of the receptionist to retrieve.</param>
        /// <returns>Returns the receptionist object.</returns>
        /// <response code="200">If the receptionist is found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="404">If the receptionist is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{id}")]
        public async Task<ReceptionistDto> Get(string id, CancellationToken cancellationToken)
        {
            // Validation
            var objectIdDtoValidation = await _objectIdDtoValidator.ValidateAsync(new ObjectIdDto(id), cancellationToken);
            if (!objectIdDtoValidation.IsValid)
            {
                var validationErrors = objectIdDtoValidation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new Domain.Exceptions.ValidationException(validationErrors);
            }

            var receptionist = await _receptionistService.Get(id, cancellationToken);
            _logger.LogInformation("Requested receptionist with id {id}", id);
            return _mapper.Map<ReceptionistDto>(receptionist);
        }

        /// <summary>
        /// Create an receptionist by ID.
        /// </summary>
        /// <param name="createReceptionistDto">The receptionist object fields containing details.</param>
        /// <response code="200">If the receptionist is created</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost]
        public async Task Post([FromBody] CreateReceptionistDto createReceptionistDto, CancellationToken cancellationToken)
        {
            // Validation
            var receptionistValidation = await _createReceptionistDtoValidator.ValidateAsync(createReceptionistDto, cancellationToken);
            if (!receptionistValidation.IsValid)
            {
                var validationErrors = receptionistValidation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new Domain.Exceptions.ValidationException(validationErrors);
            }

            var receptionistCreateModel = _mapper.Map<CreateReceptionistModel>(createReceptionistDto);
            await _receptionistService.Create(receptionistCreateModel, cancellationToken);
            _logger.LogInformation("New receptionist was successfully created");
        }

        /// <summary>
        /// Update an receptionist by ID.
        /// </summary>
        /// <param name="updateReceptionistDto">The receptionist object fields containing details.</param>
        /// <response code="200">If the receptionist is updated</response>
        /// <response code="404">If the receptionist is not found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut]
        public async Task Put([FromBody] UpdateReceptionistDto updateReceptionistDto, CancellationToken cancellationToken)
        {
            // Validation
            var receptionistValidation = await _updateReceptionistDtoValidator.ValidateAsync(updateReceptionistDto, cancellationToken);
            if (!receptionistValidation.IsValid)
            {
                var validationErrors = receptionistValidation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new Domain.Exceptions.ValidationException(validationErrors);
            }

            var updateReceptionistModel = _mapper.Map<UpdateReceptionistModel>(updateReceptionistDto);
            await _receptionistService.Update(updateReceptionistModel, cancellationToken);
            _logger.LogInformation("Receptionist with id {id} was successfully updated", updateReceptionistDto.Id);
        }

        /// <summary>
        /// Delete an receptionist by ID.
        /// </summary>
        /// <param name="id">The receptionist Id of object to delete.</param>
        /// <response code="200">If the receptionist is deleted</response>
        /// <response code="404">If the receptionist is not found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{id}")]
        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            // Validation
            var objectIdDtoValidation = await _objectIdDtoValidator.ValidateAsync(new ObjectIdDto(id), cancellationToken);
            if (!objectIdDtoValidation.IsValid)
            {
                var validationErrors = objectIdDtoValidation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new Domain.Exceptions.ValidationException(validationErrors);
            }

            await _receptionistService.Delete(id, cancellationToken);
            _logger.LogInformation("Receptionist with id {id} was successfully deleted", id);
        }
    }
}