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
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly ILogger<AppointmentsController> _logger;
        private readonly IMapper _mapper;

        // Validators
        private readonly IValidator<CreateAppointmentDto> _createAppointmentDtoValidator;
        private readonly IValidator<UpdateAppointmentDto> _updateAppointmentDtoValidator;
        private readonly IValidator<GetScheduleDto> _getScheduleDtoValidator;

        public AppointmentsController(
            IAppointmentService appointmentService,
            IDoctorService doctorService,
            IPatientService patientService,
            ILogger<AppointmentsController> logger,
            IMapper mapper,
            IValidator<CreateAppointmentDto> createAppointmentDtoValidator,
            IValidator<UpdateAppointmentDto> updateAppointmentDtoValidator,
            IValidator<GetScheduleDto> getScheduleDtoValidator)
        {
            _appointmentService = appointmentService;
            _logger = logger;
            _mapper = mapper;
            _createAppointmentDtoValidator = createAppointmentDtoValidator;
            _updateAppointmentDtoValidator = updateAppointmentDtoValidator;
            _getScheduleDtoValidator = getScheduleDtoValidator;
            _doctorService = doctorService;
            _patientService = patientService;
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
        [Authorize(Roles = "Receptionist")]
        public async Task<IEnumerable<AppointmentModel>> GetDoctorsAppointmentsAsReceptionist(string id, CancellationToken cancellationToken)
        {
            var appointments = await _appointmentService.GetAllByDoctorId(Guid.Parse(id), cancellationToken);
            _logger.LogInformation("Requested doctors appointments with id {id}", id);
            return appointments;
        }

        /// <summary>
        /// Gets list of appointments for a patient as patient.
        /// </summary>
        /// <returns>Returns the list of appointments.</returns>
        /// <response code="200">If the patient is found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="404">If the patient is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("as-patient")]
        [Authorize(Roles = "Patient")]
        public async Task<IEnumerable<AppointmentModel>> GetPatientsAppointments(CancellationToken cancellationToken)
        {
            var userId = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                    ?? throw new ForbiddenException("You are not allowed to access this resource");

            var patientModel = await _patientService.GetByAccountId(Guid.Parse(userId), cancellationToken);

            var appointments = await _appointmentService.GetAllByPatientId(patientModel.Id, cancellationToken);
            _logger.LogInformation($"Requested patients appointments with id {patientModel.Id}", patientModel.Id);
            return appointments;
        }

        /// <summary>
        /// Gets list of appointments for a doctor as doctor.
        /// </summary>
        /// <returns>Returns the list of appointments.</returns>
        /// <response code="200">If the doctor is found</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="404">If the doctor is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("as-doctor")]
        [Authorize(Roles = "Doctor")]
        public async Task<IEnumerable<AppointmentModel>> GetDoctorsAppointments(CancellationToken cancellationToken)
        {
            var userId = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                    ?? throw new ForbiddenException("You are not allowed to access this resource");

            var doctorModel = await _doctorService.GetByAccountId(Guid.Parse(userId), cancellationToken);

            var appointments = await _appointmentService.GetAllByDoctorId(doctorModel.Id, cancellationToken);
            _logger.LogInformation("Requested doctors appointments with id {id}", doctorModel.Id);
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
        [Authorize(Roles = "Receptionist")]
        public async Task<IEnumerable<AppointmentModel>> GetPatientsAppointmentsAsReceptionist(string id, CancellationToken cancellationToken)
        {
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
            var patient = await _patientService.GetByAccountId(Guid.Parse(userId), cancellationToken);
            createAppointmentModel.PatientId = patient.Id;
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
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? throw new ForbiddenException("You are not allowed to access this resource");

                var patient = await _patientService.GetByAccountId(Guid.Parse(userId), cancellationToken);
                if (patient.Id != appointment.PatientId)
                {
                    throw new ForbiddenException("You are not allowed to access this resource");
                }
            }

            await _updateAppointmentDtoValidator.ValidateAndThrowAsync(updateAppointmentDto, cancellationToken);

            var updateAppointmentModel = _mapper.Map<UpdateAppointmentModel>(updateAppointmentDto);
            await _appointmentService.Update(updateAppointmentModel, cancellationToken);
            _logger.LogInformation("New appointment was successfully updated.");
        }

        /// <summary>
        /// Delete an appointment.
        /// </summary>
        /// <param name="updateAppointmentDto">The appointment object fields containing details.</param>
        /// <response code="200">If the appointment is updated</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Patient,Receptionist")]
        public async Task Put(string id, CancellationToken cancellationToken)
        {
            if (User.IsInRole("Patient"))
            {
                var appointment = await _appointmentService.Get(Guid.Parse(id), cancellationToken);
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? throw new ForbiddenException("You are not allowed to access this resource");

                var patient = await _patientService.GetByAccountId(Guid.Parse(userId), cancellationToken);
                if (patient.Id != appointment.PatientId)
                {
                    throw new ForbiddenException("You are not allowed to access this resource");
                }
            }

            await _appointmentService.Delete(Guid.Parse(id), cancellationToken);
            _logger.LogInformation("New appointment was successfully deleted.");
        }

        /// <summary>
        /// Gets a schedule of a doctor on some day.
        /// </summary>
        /// <param name="getScheduleDto">The object which contains fields for getting schedule.</param>
        /// <returns>Returns the schedule.</returns>
        /// <response code="200">Success</response>
        /// <response code="400">If validation errors occured</response>
        /// <response code="404">If not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("Schedule")]
        [Authorize]
        public async Task<IEnumerable<TimeSlotModel>> GetSchedule([FromQuery] GetScheduleDto getScheduleDto, CancellationToken cancellationToken)
        {
            await _getScheduleDtoValidator.ValidateAndThrowAsync(getScheduleDto, cancellationToken);
            var schedule = await _appointmentService.GetDoctorsSchedule(getScheduleDto.DoctorId, getScheduleDto.Date, cancellationToken);
            _logger.LogInformation("Requested schedule");
            return schedule;
        }
    }
}
