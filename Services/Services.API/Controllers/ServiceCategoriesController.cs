using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.API.DTOs;
using Services.Application.Models;
using Services.Application.Services.Abstractions;

namespace Services.API.Controllers
{
    /// <summary>
    /// Controller for managing service categories.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceCategoriesController : ControllerBase
    {
        private readonly IServiceCategoryManager _serviceCategoryManager;
        private readonly ILogger<ServiceCategoriesController> _logger;
        private readonly IMapper _mapper;

        // Validators
        private readonly IValidator<CreateServiceCategoryDto> _createServiceCategoryDtoValidator;
        private readonly IValidator<UpdateServiceCategoryDto> _updateServiceCategoryDtoValidator;

        public ServiceCategoriesController(IServiceCategoryManager serviceCategoryService, ILogger<ServiceCategoriesController> logger, IMapper mapper,
            IValidator<CreateServiceCategoryDto> createServiceCategoryDtoValidator,
            IValidator<UpdateServiceCategoryDto> updateServiceCategoryDtoValidator)
        {
            _serviceCategoryManager = serviceCategoryService;
            _logger = logger;
            _mapper = mapper;
            _createServiceCategoryDtoValidator = createServiceCategoryDtoValidator;
            _updateServiceCategoryDtoValidator = updateServiceCategoryDtoValidator;
        }

        /// <summary>
        /// Gets the list of all service categories.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/ServiceCategories
        ///
        /// </remarks>
        /// <returns>Returns a list of service categories objects.</returns>
        /// <response code="200">Returns the list of service categories</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<ServiceCategoryDto>> Get(CancellationToken cancellationToken)
        {
            var serviceCategories = await _serviceCategoryManager.GetAll(cancellationToken);
            _logger.LogInformation("Requested service categories list");

            var serviceCategoriesDtos = _mapper.Map<IEnumerable<ServiceCategoryDto>>(serviceCategories);
            return serviceCategoriesDtos;
        }

        /// <summary>
        /// Gets an service category by ID.
        /// </summary>
        /// <param name="id">The ID of the service category to retrieve.</param>
        /// <returns>Returns the service category object.</returns>
        /// <response code="200">If the service category is found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="404">If the service category is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ServiceCategoryDto> Get(string id, CancellationToken cancellationToken)
        {
            var serviceCategory = await _serviceCategoryManager.Get(Guid.Parse(id), cancellationToken);
            _logger.LogInformation("Requested service category with id {id}", id);
            return _mapper.Map<ServiceCategoryDto>(serviceCategory);
        }

        /// <summary>
        /// Create an service category by ID.
        /// </summary>
        /// <param name="createServiceCategoryDto">The service category object fields containing details.</param>
        /// <response code="200">If the service category is created</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost]
        [Authorize(Roles="Receptionist")]
        public async Task Post([FromBody] CreateServiceCategoryDto createServiceCategoryDto, CancellationToken cancellationToken)
        {
            await _createServiceCategoryDtoValidator.ValidateAndThrowAsync(createServiceCategoryDto, cancellationToken);

            var serviceCategoryCreateModel = _mapper.Map<CreateServiceCategoryModel>(createServiceCategoryDto);
            await _serviceCategoryManager.Create(serviceCategoryCreateModel, cancellationToken);
            _logger.LogInformation("New serviceCategory was successfully created");
        }

        /// <summary>
        /// Update an service category by ID.
        /// </summary>
        /// <param name="updateServiceCategoryDto">The service category object fields containing details.</param>
        /// <response code="200">If the service category is updated</response>
        /// <response code="404">If the service category is not found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut]
        [Authorize(Roles="Receptionist")]
        public async Task Put([FromBody] UpdateServiceCategoryDto updateServiceCategoryDto, CancellationToken cancellationToken)
        {
            await _updateServiceCategoryDtoValidator.ValidateAndThrowAsync(updateServiceCategoryDto, cancellationToken);

            var updateServiceCategoryModel = _mapper.Map<UpdateServiceCategoryModel>(updateServiceCategoryDto);
            await _serviceCategoryManager.Update(updateServiceCategoryModel, cancellationToken);
            _logger.LogInformation("ServiceCategory with id {id} was successfully updated", updateServiceCategoryDto.Id);
        }

        /// <summary>
        /// Delete an service category by ID.
        /// </summary>
        /// <param name="id">The service category Id of object to delete.</param>
        /// <response code="200">If the service category is deleted</response>
        /// <response code="404">If the service category is not found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{id}")]
        [Authorize(Roles="Receptionist")]
        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            await _serviceCategoryManager.Delete(Guid.Parse(id), cancellationToken);
            _logger.LogInformation("ServiceCategory with id {id} was successfully deleted", id);
        }
    }
}
