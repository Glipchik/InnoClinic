using Appointments.Application.Exceptions;
using Appointments.Application.Models;
using Appointments.Application.Services.Abstractions;
using Appointments.Domain.Entities;
using Appointments.Domain.Repositories.Abstractions;
using AutoMapper;

namespace Appointments.Application.Services
{
    public class AppointmentService(
            IUnitOfWork unitOfWork,
            IMapper mapper) : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<AppointmentModel> Approve(Guid appointmentId, CancellationToken cancellationToken)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetAsync(appointmentId, cancellationToken)
                ?? throw new NotFoundException($"Appointment with id: {appointmentId} is not found. Can't approve appointment.");

            var appointmentDateTime = appointment.Date.ToDateTime(appointment.Time);
            if (!await CheckIsDoctorIsAvailable(appointmentDateTime, appointment.DoctorId, cancellationToken))
            {
                throw new BadRequestException("Doctor is not available at this time. Change time.");
            }

            var approvedAppointment = await _unitOfWork.AppointmentRepository.ApproveAsync(appointmentId, cancellationToken);
            return _mapper.Map<AppointmentModel>(approvedAppointment);
        }

        public async Task<AppointmentModel> Create(CreateAppointmentModel createAppointmentModel, CancellationToken cancellationToken)
        {
            var patient = await _unitOfWork.PatientRepository.GetAsync(createAppointmentModel.PatientId, cancellationToken)
                ?? throw new RelatedObjectNotFoundException($"Patient with id: {createAppointmentModel.PatientId} is not found. Can't create appointment.");

            var doctor = await _unitOfWork.DoctorRepository.GetAsync(createAppointmentModel.DoctorId, cancellationToken);
            if (doctor == null || doctor.Status != Domain.Enums.DoctorStatus.AtWork)
            {
                throw new RelatedObjectNotFoundException($"Doctor with id: {createAppointmentModel.DoctorId} is not found or not at work. Can't create appointment.");
            }

            var service = await _unitOfWork.ServiceRepository.GetAsync(createAppointmentModel.ServiceId, cancellationToken);
            if (service == null || !service.IsActive)
            {
                throw new RelatedObjectNotFoundException($"Service with id: {createAppointmentModel.ServiceId} is not found or not active. Can't create appointment.");
            }

            var appointmentDateTime = createAppointmentModel.Date.ToDateTime(createAppointmentModel.Time);
            ValidateDate(appointmentDateTime, cancellationToken);

            if (!await CheckIsDoctorIsAvailable(appointmentDateTime, createAppointmentModel.DoctorId, cancellationToken))
            {
                throw new BadRequestException("Doctor is not available at this time. Change time.");
            }

            var newAppointment = _mapper.Map<Appointment>(createAppointmentModel);
            newAppointment.IsApproved = false;
            
            var createdAppointment = await _unitOfWork.AppointmentRepository.CreateAsync(newAppointment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<AppointmentModel>(createdAppointment);
        }

        public async Task<IEnumerable<AppointmentModel>> GetAll(CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<AppointmentModel>>(await _unitOfWork.AppointmentRepository.GetAllAsync(cancellationToken));
        }

        public async Task<IEnumerable<AppointmentModel>> GetAllByDoctorId(Guid doctorId, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<AppointmentModel>>(await _unitOfWork.AppointmentRepository.GetAllByDoctorIdAsync(doctorId, cancellationToken));
        }

        public async Task<IEnumerable<AppointmentModel>> GetAllByPatientId(Guid patientId, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<AppointmentModel>>(await _unitOfWork.AppointmentRepository.GetAllByPatientIdAsync(patientId, cancellationToken));
        }

        public async Task<AppointmentModel> Update(UpdateAppointmentModel updateAppointmentModel, CancellationToken cancellationToken)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetAsync(updateAppointmentModel.Id, cancellationToken)
                ?? throw new NotFoundException($"Appointment with id: {updateAppointmentModel.Id} is not found. Can't update appointment.");

            _mapper.Map(appointment, updateAppointmentModel);

            var updatedAppointment = await _unitOfWork.AppointmentRepository.UpdateAsync(appointment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<AppointmentModel>(updatedAppointment);
        }

        private static void ValidateDate(DateTime date, CancellationToken cancellationToken)
        {
            var minimalValidDateTime = DateTime.Now.AddDays(1);
            if (date < minimalValidDateTime)
            {
                throw new BadRequestException("Date and time must be more than 'Now + 1 day'.");
            }
        }

        private async Task<bool> CheckIsDoctorIsAvailable(DateTime dateTime, Guid doctorId, CancellationToken cancellationToken)
        {
            var doctorSchedule = await GetDoctorsSchedule(doctorId, cancellationToken);

            return doctorSchedule.SingleOrDefault(schedule => schedule.Start <= dateTime && schedule.Finish >= dateTime) != null;
        }

        public async Task<IEnumerable<AppointmentsScheduleModel>> GetDoctorsSchedule(Guid doctorId, CancellationToken cancellationToken)
        {
            var doctorAppointments = await _unitOfWork.AppointmentRepository.GetAllApprovedByDoctorIdFromNowAsync(doctorId, cancellationToken);

            return doctorAppointments.Select(appointment => new AppointmentsScheduleModel
            {
                Start = appointment.Date.ToDateTime(appointment.Time),
                Finish = appointment.Date.ToDateTime(appointment.Time).AddMinutes(appointment.Service.ServiceCategory.TimeSlotSize.TotalMinutes)
            });
        }
    }
}
