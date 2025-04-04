﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Offices.API.DTOs;
using Offices.API.Extensions;
using Offices.API.Validators;
using Offices.Application.Models;
using Offices.Application.Services.Abstractions;
using Offices.Data.Entities;
using Offices.Domain.Models;

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

        public OfficesController(IOfficeService officeService, ILogger<OfficesController> logger, IMapper mapper,
            IValidator<CreateOfficeDto> createOfficeDtoValidator,
            IValidator<UpdateOfficeDto> updateOfficeDtoValidator)
        {
            _officeService = officeService;
            _logger = logger;
            _mapper = mapper;
            _createOfficeDtoValidator = createOfficeDtoValidator;
            _updateOfficeDtoValidator = updateOfficeDtoValidator;
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
        [Authorize]
        public async Task<PaginatedList<OfficeDto>> Get(CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10)
        {
            var offices = await _officeService.GetAll(pageIndex, pageSize, cancellationToken);
            _logger.LogInformation("Requested offices list");

            List<OfficeDto> officeDtos = _mapper.Map<List<OfficeDto>>(offices.Items);
            return new PaginatedList<OfficeDto>(officeDtos, offices.PageIndex, offices.TotalPages);
        }

        /// <summary>
        /// Gets an office by ID.
        /// </summary>
        /// <param name="id">The ID of the office to retrieve.</param>
        /// <returns>Returns the office object.</returns>
        /// <response code="200">If the office is found</response>
        /// <response code="404">If the office is not found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<OfficeDto> Get(string id, CancellationToken cancellationToken)
        {
            var office = await _officeService.Get(Guid.Parse(id), cancellationToken);
            _logger.LogInformation("Requested office with id {id}", id);
            return _mapper.Map<OfficeDto>(office);
        }

        /// <summary>
        /// Create an office by ID.
        /// </summary>
        /// <param name="createOfficeDto">The office object fields containing details.</param>
        /// <response code="200">If the office is created</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost]
        [Authorize(Roles = "Receptionist")]
        public async Task Post([FromBody] CreateOfficeDto createOfficeDto, CancellationToken cancellationToken)
        {
            // Validation
            var officeValidation = await _createOfficeDtoValidator.ValidateAsync(createOfficeDto, cancellationToken);
            if (!officeValidation.IsValid)
            {
                var validationErrors = officeValidation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new Domain.Exceptions.ValidationException(validationErrors);
            }

            var createOfficeModel = _mapper.Map<CreateOfficeModel>(createOfficeDto);
            await _officeService.Create(createOfficeModel, cancellationToken);
            _logger.LogInformation("New office was successfully created");
        }

        /// <summary>
        /// Update an office by ID.
        /// </summary>
        /// <param name="updateOfficeDto">The office object fields containing details such as ID and address.</param>
        /// <response code="200">If the office is updated</response>
        /// <response code="404">If the office is not found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut]
        [Authorize(Roles = "Receptionist")]
        public async Task Put([FromBody] UpdateOfficeDto updateOfficeDto, CancellationToken cancellationToken)
        {
            // Validation
            var officeValidation = await _updateOfficeDtoValidator.ValidateAsync(updateOfficeDto, cancellationToken);
            if (!officeValidation.IsValid)
            {
                var validationErrors = officeValidation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new Domain.Exceptions.ValidationException(validationErrors);
            }

            var updateOfficeModel = _mapper.Map<UpdateOfficeModel>(updateOfficeDto);
            await _officeService.Update(updateOfficeModel, cancellationToken);
            _logger.LogInformation("Office with id {id} was successfully updated", updateOfficeDto.Id);
        }

        /// <summary>
        /// Delete an office by ID.
        /// </summary>
        /// <param name="id">The office Id of object to delete.</param>
        /// <response code="200">If the office is deleted</response>
        /// <response code="404">If the office is not found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Receptionist")]
        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            await _officeService.Delete(Guid.Parse(id), cancellationToken);
            _logger.LogInformation("Office with id {id} was successfully deleted", id);
        }
    }
}
