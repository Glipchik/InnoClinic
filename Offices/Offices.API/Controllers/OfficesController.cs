using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Offices.API.DTOs;
using Offices.API.Extensions;
using Offices.API.Validators;
using Offices.Application.Models;
using Offices.Application.Services.Abstractions;
using Offices.Data.Entities;

namespace Offices.API.Controllers
{
    /// <summary>
    /// Controller for managing offices.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OfficesController : ControllerBase
    {
        private readonly IOfficeService _officeService;
        private readonly ILogger<OfficesController> _logger;
        private readonly IMapper _mapper;

        // Validators
        private readonly IValidator<CreateOfficeDto> _createOfficeDtoValidator;
        private readonly IValidator<UpdateOfficeDto> _updateOfficeDtoValidator;
        private readonly IValidator<ObjectIdDto> _objectIdDtoValidator;

        public OfficesController(IOfficeService officeService, ILogger<OfficesController> logger, IMapper mapper,
            IValidator<CreateOfficeDto> createOfficeDtoValidator,
            IValidator<UpdateOfficeDto> updateOfficeDtoValidator,
            IValidator<ObjectIdDto> objectIdDtoValidator)
        {
            _officeService = officeService;
            _logger = logger;
            _mapper = mapper;
            _createOfficeDtoValidator = createOfficeDtoValidator;
            _updateOfficeDtoValidator = updateOfficeDtoValidator;
            _objectIdDtoValidator = objectIdDtoValidator;
        }

        /// <summary>
        /// Gets the list of all offices.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Offices
        ///
        /// </remarks>
        /// <returns>Returns a list of office objects.</returns>
        /// <response code="200">Returns the list of offices</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var offices = await _officeService.GetAll(cancellationToken);
            _logger.LogInformation("Requested offices list");

            var officeDtos = _mapper.Map<IEnumerable<OfficeDto>>(offices);
            return Ok(officeDtos);
        }

        /// <summary>
        /// Gets an office by ID.
        /// </summary>
        /// <param name="id">The ID of the office to retrieve.</param>
        /// <returns>Returns the office object.</returns>
        /// <response code="200">If the office is found</response>
        /// <response code="204">If the office is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, CancellationToken cancellationToken)
        {
            // Validation
            var objectIdDtoValidation = await _objectIdDtoValidator.ValidateAsync(new ObjectIdDto(id), cancellationToken);
            if (!objectIdDtoValidation.IsValid)
            {
                objectIdDtoValidation.AddToModelState(ModelState, _logger);
                return BadRequest(ModelState);
            }

            var office = await _officeService.Get(id, cancellationToken);
            _logger.LogInformation("Requested office with id {id}", id);
            return Ok(_mapper.Map<OfficeDto>(office));
        }

        /// <summary>
        /// Create an office by ID.
        /// </summary>
        /// <param name="createOfficeDto">The office object fields containing details.</param>
        /// <response code="200">If the office is created</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateOfficeDto createOfficeDto, CancellationToken cancellationToken)
        {
            // Validation
            var officeValidation = await _createOfficeDtoValidator.ValidateAsync(createOfficeDto, cancellationToken);
            if (!officeValidation.IsValid)
            {
                officeValidation.AddToModelState(ModelState, _logger);
                return BadRequest(ModelState);
            }

            var createOfficeModel = _mapper.Map<CreateOfficeModel>(createOfficeDto);
            await _officeService.Create(createOfficeModel, cancellationToken);
            _logger.LogInformation("New office was successfully created");
            return Ok();
        }

        /// <summary>
        /// Update an office by ID.
        /// </summary>
        /// <param name="updateOfficeDto">The office object fields containing details such as ID and address.</param>
        /// <response code="200">If the office is updated</response>
        /// <response code="400">If the office is not found or validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateOfficeDto updateOfficeDto, CancellationToken cancellationToken)
        {
            // Validation
            var officeValidation = await _updateOfficeDtoValidator.ValidateAsync(updateOfficeDto, cancellationToken);
            if (!officeValidation.IsValid)
            {
                officeValidation.AddToModelState(ModelState, _logger);
                return BadRequest(ModelState);
            }

            var updateOfficeModel = _mapper.Map<UpdateOfficeModel>(updateOfficeDto);
            await _officeService.Update(updateOfficeModel, cancellationToken);
            _logger.LogInformation("Office with id {id} was successfully updated", updateOfficeDto.Id);
            return Ok();
        }

        /// <summary>
        /// Delete an office by ID.
        /// </summary>
        /// <param name="id">The office Id of object to delete.</param>
        /// <response code="200">If the office is deleted</response>
        /// <response code="400">If the office is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            // Validation
            var objectIdDtoValidation = await _objectIdDtoValidator.ValidateAsync(new ObjectIdDto(id), cancellationToken);
            if (!objectIdDtoValidation.IsValid)
            {
                objectIdDtoValidation.AddToModelState(ModelState, _logger);
                return BadRequest(ModelState);
            }

            await _officeService.Delete(id, cancellationToken);
            _logger.LogInformation("Office with id {id} was successfully deleted", id);
            return Ok();
        }
    }
}
