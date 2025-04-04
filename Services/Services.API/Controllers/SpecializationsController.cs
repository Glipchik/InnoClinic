﻿using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.API.DTOs;
using Services.Application.Models;
using Services.Application.Services.Abstractions;
using Services.Domain.Models;

namespace Services.API.Controllers
{
    /// <summary>
    /// Controller for managing specializations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationsController : ControllerBase
    {
        private readonly ISpecializationService _specializationService;
        private readonly ILogger<SpecializationsController> _logger;
        private readonly IMapper _mapper;

        // Validators
        private readonly IValidator<CreateSpecializationDto> _createSpecializationDtoValidator;
        private readonly IValidator<UpdateSpecializationDto> _updateSpecializationDtoValidator;

        public SpecializationsController(ISpecializationService specializationService, ILogger<SpecializationsController> logger, IMapper mapper,
            IValidator<CreateSpecializationDto> createSpecializationDtoValidator,
            IValidator<UpdateSpecializationDto> updateSpecializationDtoValidator)
        {
            _specializationService = specializationService;
            _logger = logger;
            _mapper = mapper;
            _createSpecializationDtoValidator = createSpecializationDtoValidator;
            _updateSpecializationDtoValidator = updateSpecializationDtoValidator;
        }

        /// <summary>
        /// Gets the list of all specializations.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Specializations
        ///
        /// </remarks>
        /// <returns>Returns a list of specializations objects.</returns>
        /// <response code="200">Returns the list of specializations</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<SpecializationDto>> Get(CancellationToken cancellationToken)
        {
            var specializations = await _specializationService.GetAll(cancellationToken);
            _logger.LogInformation("Requested specializations list");

            var specializationDtos = _mapper.Map<IEnumerable<SpecializationDto>>(specializations);
            return specializationDtos;
        }

        /// <summary>
        /// Gets the list of all specializations with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Specializations/with-pagination
        ///
        /// </remarks>
        /// <returns>Returns a list of specializations objects.</returns>
        /// <response code="200">Returns the list of specializations</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("with-pagination")]
        [Authorize]
        public async Task<PaginatedList<SpecializationDto>> Get(CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10)
        {
            var specializations = await _specializationService.GetAll(pageIndex, pageSize, cancellationToken);
            _logger.LogInformation("Requested specializations list");

            List<SpecializationDto> specializationDtos = _mapper.Map<List<SpecializationDto>>(specializations.Items);
            return new PaginatedList<SpecializationDto>(specializationDtos, specializations.PageIndex, specializations.TotalPages);
        }

        /// <summary>
        /// Gets a specialization by ID.
        /// </summary>
        /// <param name="id">The ID of the specialization to retrieve.</param>
        /// <returns>Returns the specialization object.</returns>
        /// <response code="200">If the specialization is found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="404">If the specialization is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<SpecializationDto> Get(string id, CancellationToken cancellationToken)
        {
            var specialization = await _specializationService.Get(Guid.Parse(id), cancellationToken);
            _logger.LogInformation("Requested specialization with id {id}", id);
            return _mapper.Map<SpecializationDto>(specialization);
        }

        /// <summary>
        /// Create a specialization by ID.
        /// </summary>
        /// <param name="createSpecializationDto">The specialization object fields containing details.</param>
        /// <response code="200">If the specialization is created</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost]
        [Authorize(Roles="Receptionist")]
        public async Task Post([FromBody] CreateSpecializationDto createSpecializationDto, CancellationToken cancellationToken)
        {
            await _createSpecializationDtoValidator.ValidateAndThrowAsync(createSpecializationDto, cancellationToken);

            var specializationCreateModel = _mapper.Map<CreateSpecializationModel>(createSpecializationDto);
            await _specializationService.Create(specializationCreateModel, cancellationToken);
            _logger.LogInformation("New specialization was successfully created");
        }

        /// <summary>
        /// Update a specialization by ID.
        /// </summary>
        /// <param name="updateSpecializationDto">The specialization object fields containing details.</param>
        /// <response code="200">If the specialization is updated</response>
        /// <response code="404">If the specialization is not found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut]
        [Authorize(Roles="Receptionist")]
        public async Task Put([FromBody] UpdateSpecializationDto updateSpecializationDto, CancellationToken cancellationToken)
        {
            await _updateSpecializationDtoValidator.ValidateAndThrowAsync(updateSpecializationDto, cancellationToken);

            var updateSpecializationModel = _mapper.Map<UpdateSpecializationModel>(updateSpecializationDto);
            await _specializationService.Update(updateSpecializationModel, cancellationToken);
            _logger.LogInformation("Specialization with id {id} was successfully updated", updateSpecializationDto.Id);
        }

        /// <summary>
        /// Delete a specialization by ID.
        /// </summary>
        /// <param name="id">The specialization Id of object to delete.</param>
        /// <response code="200">If the specialization is deleted</response>
        /// <response code="404">If the specialization is not found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{id}")]
        [Authorize(Roles="Receptionist")]
        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            await _specializationService.Delete(Guid.Parse(id), cancellationToken);
            _logger.LogInformation("Specialization with id {id} was successfully deleted", id);
        }
    }
}
