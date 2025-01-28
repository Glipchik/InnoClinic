using AutoMapper;
using Documents.API.DTOs;
using Documents.Application.Exceptions;
using Documents.Application.Models;
using Documents.Application.Services.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Documents.API.Controllers
{
    /// <summary>
    /// Controller for managing results.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly IResultService _resultService;
        private readonly ILogger<ResultsController> _logger;
        private readonly IMapper _mapper;

        // Validators
        private readonly IValidator<CreateResultDto> _createResultDtoValidator;
        private readonly IValidator<UpdateResultDto> _updateResultDtoValidator;

        public ResultsController(
            IResultService resultService,
            ILogger<ResultsController> logger,
            IMapper mapper,
            IValidator<CreateResultDto> createResultDtoValidator,
            IValidator<UpdateResultDto> updateResultDtoValidator)
        {
            _resultService = resultService;
            _logger = logger;
            _mapper = mapper;
            _createResultDtoValidator = createResultDtoValidator;
            _updateResultDtoValidator = updateResultDtoValidator;
        }

        /// <summary>
        /// Gets list of result for a doctor.
        /// </summary>
        /// <param name="id">The ID of the doctor.</param>
        /// <returns>Returns the list of results.</returns>
        /// <response code="200">If the doctor is found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="404">If the doctor is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("Doctor/{id}")]
        [Authorize(Roles = "Receptionist")]
        public async Task<IEnumerable<ResultModel>> GetDoctorsResultsAsReceptionist(string id, CancellationToken cancellationToken)
        {
            var results = await _resultService.GetAllByDoctorId(Guid.Parse(id), cancellationToken);
            _logger.LogInformation("Requested doctors results with id {id}", id);
            return results;
        }

        /// <summary>
        /// Gets list of results for a patient as patient.
        /// </summary>
        /// <returns>Returns the list of results.</returns>
        /// <response code="200">If the patient is found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="404">If the patient is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("as-patient")]
        [Authorize(Roles = "Patient")]
        public async Task<IEnumerable<ResultModel>> GetPatientsResults(CancellationToken cancellationToken)
        {
            var userId = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                    ?? throw new ForbiddenException("You are not allowed to access this resource");

            var patientModel = await _patientService.GetByAccountId(Guid.Parse(userId), cancellationToken);

            var results = await _resultService.GetAllByPatientId(patientModel.Id, cancellationToken);
            _logger.LogInformation($"Requested patients results with id {patientModel.Id}", patientModel.Id);
            return results;
        }

        /// <summary>
        /// Gets list of results for a doctor as doctor.
        /// </summary>
        /// <returns>Returns the list of results.</returns>
        /// <response code="200">If the doctor is found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="404">If the doctor is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("as-doctor")]
        [Authorize(Roles = "Doctor")]
        public async Task<IEnumerable<ResultModel>> GetDoctorsResults(CancellationToken cancellationToken)
        {
            var userId = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                    ?? throw new ForbiddenException("You are not allowed to access this resource");

            var doctorModel = await _doctorService.GetByAccountId(Guid.Parse(userId), cancellationToken);

            var results = await _resultService.GetAllByDoctorId(doctorModel.Id, cancellationToken);
            _logger.LogInformation("Requested doctors results with id {id}", doctorModel.Id);
            return results;
        }

        /// <summary>
        /// Gets list of results for a patient.
        /// </summary>
        /// <param name="id">The ID of the patient.</param>
        /// <returns>Returns the list of results.</returns>
        /// <response code="200">If the patient is found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="404">If the patient is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("Patient/{id}")]
        [Authorize(Roles = "Receptionist")]
        public async Task<IEnumerable<ResultModel>> GetPatientsResultsAsReceptionist(string id, CancellationToken cancellationToken)
        {
            var results = await _resultService.GetAllByPatientId(Guid.Parse(id), cancellationToken);
            _logger.LogInformation("Requested patient results with id {id}", id);
            return results;
        }

        /// <summary>
        /// Gets list of results.
        /// </summary>
        /// <returns>Returns the list of results.</returns>
        /// <response code="200">Success</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="404">If not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        [Authorize(Roles = "Receptionist")]
        public async Task<IEnumerable<ResultModel>> Get(CancellationToken cancellationToken)
        {
            var results = await _resultService.GetAll(cancellationToken);
            _logger.LogInformation("Requested all results");
            return results;
        }

        /// <summary>
        /// Create an result.
        /// </summary>
        /// <param name="createResultDto">The result object fields containing details.</param>
        /// <response code="200">If the result is created</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task Post([FromBody] CreateResultDto createResultDto, CancellationToken cancellationToken)
        {
            var userId = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                ?? throw new ForbiddenException("You are not allowed to access this resource");

            await _createResultDtoValidator.ValidateAndThrowAsync(createResultDto, cancellationToken);

            var createResultModel = _mapper.Map<CreateResultModel>(createResultDto);
            await _resultService.Create(createResultModel, cancellationToken);
            _logger.LogInformation("New result was successfully created");
        }

        /// <summary>
        /// Update an result.
        /// </summary>
        /// <param name="updateResultDto">The result object fields containing details.</param>
        /// <response code="200">If the result is updated</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut]
        [Authorize(Roles = "Doctor,Receptionist")]
        public async Task Put([FromBody] UpdateResultDto updateResultDto, CancellationToken cancellationToken)
        {
            if (User.IsInRole("Doctor"))
            {
                var result = await _resultService.Get(updateResultDto.Id, cancellationToken);
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? throw new ForbiddenException("You are not allowed to access this resource");

                var doctor = await _doctorService.GetByAccountId(Guid.Parse(userId), cancellationToken);
                if (doctor.Id != result.Appointment.PatientId)
                {
                    throw new ForbiddenException("You are not allowed to access this resource");
                }
            }

            await _updateResultDtoValidator.ValidateAndThrowAsync(updateResultDto, cancellationToken);

            var updateResultModel = _mapper.Map<UpdateResultModel>(updateResultDto);
            await _resultService.Update(updateResultModel, cancellationToken);
            _logger.LogInformation("New result was successfully updated.");
        }
    }
}
