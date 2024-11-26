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
        private readonly IValidator<CreateDoctorDto> _createDoctorDtoValidator;
        private readonly IValidator<UpdateDoctorDto> _updateDoctorDtoValidator;
        private readonly IValidator<ObjectIdDto> _objectIdDtoValidator;

        public DoctorsController(IDoctorService doctorService, ILogger<DoctorsController> logger, IMapper mapper,
            IValidator<CreateDoctorDto> createDoctorDtoValidator,
            IValidator<UpdateDoctorDto> updateDoctorDtoValidator,
            IValidator<ObjectIdDto> objectIdDtoValidator)
        {
            _doctorService = doctorService;
            _logger = logger;
            _mapper = mapper;
            _createDoctorDtoValidator = createDoctorDtoValidator;
            _updateDoctorDtoValidator = updateDoctorDtoValidator;
            _objectIdDtoValidator = objectIdDtoValidator;
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
        public async Task<IEnumerable<DoctorDto>> Get(CancellationToken cancellationToken)
        {
            var doctors = await _doctorService.GetAll(cancellationToken);
            _logger.LogInformation("Requested doctors list");

            var doctorDtos = _mapper.Map<IEnumerable<DoctorDto>>(doctors);
            return doctorDtos;
        }

        /// <summary>
        /// Gets an doctor by ID.
        /// </summary>
        /// <param name="id">The ID of the doctor to retrieve.</param>
        /// <returns>Returns the doctor object.</returns>
        /// <response code="200">If the doctor is found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="404">If the doctor is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{id}")]
        public async Task<DoctorDto> Get(string id, CancellationToken cancellationToken)
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

            var doctor = await _doctorService.Get(id, cancellationToken);
            _logger.LogInformation("Requested doctor with id {id}", id);
            return _mapper.Map<DoctorDto>(doctor);
        }

        /// <summary>
        /// Create an doctor by ID.
        /// </summary>
        /// <param name="createDoctorDto">The doctor object fields containing details.</param>
        /// <response code="200">If the doctor is created</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost]
        public async Task Post([FromBody] CreateDoctorDto createDoctorDto, CancellationToken cancellationToken)
        {
            // Validation
            var doctorValidation = await _createDoctorDtoValidator.ValidateAsync(createDoctorDto, cancellationToken);
            if (!doctorValidation.IsValid)
            {
                var validationErrors = doctorValidation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new Domain.Exceptions.ValidationException(validationErrors);
            }

            var doctorCreateModel = _mapper.Map<CreateDoctorModel>(createDoctorDto);
            await _doctorService.Create(doctorCreateModel, cancellationToken);
            _logger.LogInformation("New doctor was successfully created");
        }

        /// <summary>
        /// Update an doctor by ID.
        /// </summary>
        /// <param name="updateDoctorDto">The doctor object fields containing details.</param>
        /// <response code="200">If the doctor is updated</response>
        /// <response code="404">If the doctor is not found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut]
        public async Task Put([FromBody] UpdateDoctorDto updateDoctorDto, CancellationToken cancellationToken)
        {
            // Validation
            var doctorValidation = await _updateDoctorDtoValidator.ValidateAsync(updateDoctorDto, cancellationToken);
            if (!doctorValidation.IsValid)
            {
                var validationErrors = doctorValidation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new Domain.Exceptions.ValidationException(validationErrors);
            }

            var updateDoctorModel = _mapper.Map<UpdateDoctorModel>(updateDoctorDto);
            await _doctorService.Update(updateDoctorModel, cancellationToken);
            _logger.LogInformation("Doctor with id {id} was successfully updated", updateDoctorDto.Id);
        }

        /// <summary>
        /// Delete an doctor by ID.
        /// </summary>
        /// <param name="id">The doctor Id of object to delete.</param>
        /// <response code="200">If the doctor is deleted</response>
        /// <response code="404">If the doctor is not found</response>
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

            await _doctorService.Delete(id, cancellationToken);
            _logger.LogInformation("Doctor with id {id} was successfully deleted", id);
        }
    }
}
