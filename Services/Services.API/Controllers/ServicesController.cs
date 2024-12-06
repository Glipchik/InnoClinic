using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Services.API.DTOs;
using Services.Application.Models;
using Services.Application.Services.Abstractions;
using Services.Domain.Entities;

namespace Services.API.Controllers
{
    /// <summary>
    /// Controller for managing services.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILogger<ServicesController> _logger;
        private readonly IMapper _mapper;

        // Validators
        private readonly IValidator<CreateServiceDto> _createServiceDtoValidator;
        private readonly IValidator<UpdateServiceDto> _updateServiceDtoValidator;

        public ServicesController(IServiceManager serviceManager, ILogger<ServicesController> logger, IMapper mapper,
            IValidator<CreateServiceDto> createServiceDtoValidator,
            IValidator<UpdateServiceDto> updateServiceDtoValidator)
        {
            _serviceManager = serviceManager;
            _logger = logger;
            _mapper = mapper;
            _createServiceDtoValidator = createServiceDtoValidator;
            _updateServiceDtoValidator = updateServiceDtoValidator;
        }

        /// <summary>
        /// Gets the list of all services.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Services
        ///
        /// </remarks>
        /// <returns>Returns a list of services objects.</returns>
        /// <response code="200">Returns the list of services</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        public async Task<IEnumerable<ServiceDto>> Get(CancellationToken cancellationToken)
        {
            var services = await _serviceManager.GetAll(cancellationToken);
            _logger.LogInformation("Requested services list");

            var servicesDtos = _mapper.Map<IEnumerable<ServiceDto>>(services);
            return servicesDtos;
        }

        /// <summary>
        /// Gets a service by ID.
        /// </summary>
        /// <param name="id">The ID of the service to retrieve.</param>
        /// <returns>Returns the service object.</returns>
        /// <response code="200">If the service is found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="404">If the service is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{id}")]
        public async Task<ServiceDto> Get(string id, CancellationToken cancellationToken)
        {
            var service = await _serviceManager.Get(Guid.Parse(id), cancellationToken);
            _logger.LogInformation("Requested service with id {id}", id);
            return _mapper.Map<ServiceDto>(service);
        }

        /// <summary>
        /// Create a service by ID.
        /// </summary>
        /// <param name="createServiceDto">The service object fields containing details.</param>
        /// <response code="200">If the service is created</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost]
        public async Task Post([FromBody] CreateServiceDto createServiceDto, CancellationToken cancellationToken)
        {
            var serviceValidation = await _createServiceDtoValidator.ValidateAsync(createServiceDto, cancellationToken);
            if (!serviceValidation.IsValid)
            {
                var validationErrors = serviceValidation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new Application.Exceptions.ValidationException(validationErrors);
            }

            var serviceCreateModel = _mapper.Map<CreateServiceModel>(createServiceDto);
            await _serviceManager.Create(serviceCreateModel, cancellationToken);
            _logger.LogInformation("New service was successfully created");
        }

        /// <summary>
        /// Update a service by ID.
        /// </summary>
        /// <param name="updateServiceDto">The service object fields containing details.</param>
        /// <response code="200">If the service is updated</response>
        /// <response code="404">If the service is not found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut]
        public async Task Put([FromBody] UpdateServiceDto updateServiceDto, CancellationToken cancellationToken)
        {
            var serviceValidation = await _updateServiceDtoValidator.ValidateAsync(updateServiceDto, cancellationToken);
            if (!serviceValidation.IsValid)
            {
                var validationErrors = serviceValidation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new Application.Exceptions.ValidationException(validationErrors);
            }

            var updateServiceModel = _mapper.Map<UpdateServiceModel>(updateServiceDto);
            await _serviceManager.Update(updateServiceModel, cancellationToken);
            _logger.LogInformation("Service with id {id} was successfully updated", updateServiceDto.Id);
        }

        /// <summary>
        /// Delete a service by ID.
        /// </summary>
        /// <param name="id">The service Id of object to delete.</param>
        /// <response code="200">If the service is deleted</response>
        /// <response code="404">If the service is not found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{id}")]
        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            await _serviceManager.Delete(Guid.Parse(id), cancellationToken);
            _logger.LogInformation("Service with id {id} was successfully deleted", id);
        }
    }
}
