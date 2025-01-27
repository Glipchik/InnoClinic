using Appointments.API.DTOs;
using Appointments.Application.DependencyInjection;
using Appointments.Application.Exceptions;
using Appointments.Application.Models;
using Appointments.Application.Services.Abstractions;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Appointments.API.Controllers
{
    /// <summary>
    /// Controller for managing appointments.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ILogger<AppointmentsController> _logger;
        private readonly IMapper _mapper;

        // Validators
        private readonly IValidator<CreateAppointmentDto> _createAppointmentDtoValidator;
        private readonly IValidator<UpdateAppointmentDto> _updateAppointmentDtoValidator;

        public AppointmentsController(
            IAppointmentService appointmentService,
            ILogger<AppointmentsController> logger,
            IMapper mapper,
            IValidator<CreateAppointmentDto> createAppointmentDtoValidator,
            IValidator<UpdateAppointmentDto> updateAppointmentDtoValidator)
        {
            _appointmentService = appointmentService;
            _logger = logger;
            _mapper = mapper;
            _createAppointmentDtoValidator = createAppointmentDtoValidator;
            _updateAppointmentDtoValidator = updateAppointmentDtoValidator;
        }

        /// <summary>
        /// Gets list of appointments for a doctor.
        /// </summary>
        /// <param name="id">The ID of the doctor.</param>
        /// <returns>Returns the list of appointments.</returns>
        /// <response code="200">If the doctor is found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="404">If the doctor is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("Doctor/{id}")]
        [Authorize(Roles = "Doctor,Receptionist")]
        public async Task<IEnumerable<AppointmentModel>> GetDoctorsAppointments(string id, CancellationToken cancellationToken)
        {
            if (User.IsInRole("Doctor"))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null || userId != id)
                {
                    throw new ForbiddenException("You are not allowed to access this resource");
                }
            }
            var appointments = await _appointmentService.GetAllByDoctorId(Guid.Parse(id), cancellationToken);
            _logger.LogInformation("Requested doctors appointments with id {id}", id);
            return appointments;
        }

        /// <summary>
        /// Gets list of appointments for a patient.
        /// </summary>
        /// <param name="id">The ID of the patient.</param>
        /// <returns>Returns the list of appointments.</returns>
        /// <response code="200">If the patient is found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="404">If the patient is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("Patient/{id}")]
        [Authorize(Roles = "Patient,Receptionist")]
        public async Task<IEnumerable<AppointmentModel>> GetPatientsAppointments(string id, CancellationToken cancellationToken)
        {
            if (User.IsInRole("Patient"))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null || userId != id)
                {
                    throw new ForbiddenException("You are not allowed to access this resource");
                }
            }
            var appointments = await _appointmentService.GetAllByPatientId(Guid.Parse(id), cancellationToken);
            _logger.LogInformation("Requested patient appointments with id {id}", id);
            return appointments;
        }

        /// <summary>
        /// Gets list of appointments.
        /// </summary>
        /// <returns>Returns the list of appointments.</returns>
        /// <response code="200">Success</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="404">If not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        [Authorize(Roles = "Receptionist")]
        public async Task<IEnumerable<AppointmentModel>> Get(CancellationToken cancellationToken)
        {
            var appointments = await _appointmentService.GetAll(cancellationToken);
            _logger.LogInformation("Requested all appointments");
            return appointments;
        }

        /// <summary>
        /// Create an appointment.
        /// </summary>
        /// <param name="createAppointmentDto">The appointment object fields containing details.</param>
        /// <response code="200">If the appointment is created</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task Post([FromBody] CreateAppointmentDto createAppointmentDto, CancellationToken cancellationToken)
        {
            var userId = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value) 
                ?? throw new ForbiddenException("You are not allowed to access this resource");

            await _createAppointmentDtoValidator.ValidateAndThrowAsync(createAppointmentDto, cancellationToken);

            var createAppointmentModel = _mapper.Map<CreateAppointmentModel>(createAppointmentDto);
            createAppointmentModel.PatientId = Guid.Parse(userId);
            await _appointmentService.Create(createAppointmentModel, cancellationToken);
            _logger.LogInformation("New appointment was successfully created");
        }

        /// <summary>
        /// Update an appointment.
        /// </summary>
        /// <param name="updateAppointmentDto">The appointment object fields containing details.</param>
        /// <response code="200">If the appointment is updated</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut]
        [Authorize(Roles = "Patient,Receptionist")]
        public async Task Put([FromBody] UpdateAppointmentDto updateAppointmentDto, CancellationToken cancellationToken)
        {
            if (User.IsInRole("Patient"))
            {
                var appointment = await _appointmentService.Get(updateAppointmentDto.Id, cancellationToken);
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null || Guid.Parse(userId) != appointment.PatientId)
                {
                    throw new ForbiddenException("You are not allowed to access this resource.");
                }
            }

            await _updateAppointmentDtoValidator.ValidateAndThrowAsync(updateAppointmentDto, cancellationToken);

            var updateAppointmentModel = _mapper.Map<UpdateAppointmentModel>(updateAppointmentDto);
            await _appointmentService.Update(updateAppointmentModel, cancellationToken);
            _logger.LogInformation("New appointment was successfully updated.");
        }
    }
}
