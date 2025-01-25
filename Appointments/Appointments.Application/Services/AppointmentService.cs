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

        private readonly TimeOnly _startOfTheWorkingDay = new TimeOnly(9, 0, 0);
        private readonly TimeOnly _endOfTheWorkingDay = new TimeOnly(18, 0, 0);
        private readonly int _timeSlotSize = 10;

        public async Task<AppointmentModel> Approve(Guid appointmentId, CancellationToken cancellationToken)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetAsync(appointmentId, cancellationToken)
                ?? throw new NotFoundException($"Appointment with id: {appointmentId} is not found. Can't approve appointment.");

            if (!await CheckIsDoctorIsAvailable(appointment.Time, appointment.Date, appointment.DoctorId, appointment.Service.ServiceCategory, cancellationToken))
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

            var appointmentDate = createAppointmentModel.Date;
            ValidateDate(appointmentDate, cancellationToken);

            var doctorSchedule = await GetDoctorsSchedule(createAppointmentModel.DoctorId, createAppointmentModel.Date, cancellationToken);

            if (!await CheckIsDoctorIsAvailable(
                (doctorSchedule.SingleOrDefault(t => t.Id == createAppointmentModel.TimeSlotId)
                ?? throw new NotFoundException($"Time slot with id: {createAppointmentModel.TimeSlotId} is not found. Can't create appointment.")).Start,
                createAppointmentModel.Date,
                createAppointmentModel.DoctorId,
                service.ServiceCategory,
                cancellationToken))
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

            var updatedAppointment = await _unitOfWork.AppointmentRepository.Update(appointment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<AppointmentModel>(updatedAppointment);
        }

        private static void ValidateDate(DateOnly date, CancellationToken cancellationToken)
        {
            var minimalValidDateTime = DateTime.Now.AddDays(2);
            if (date.ToDateTime(TimeOnly.MinValue) < minimalValidDateTime)
            {
                throw new BadRequestException("Date and time must be more than 'Now + 2 days'.");
            }
        }

        private async Task<bool> CheckIsDoctorIsAvailable(
            TimeOnly time, 
            DateOnly date, 
            Guid doctorId, 
            ServiceCategory category,
            CancellationToken cancellationToken)
        {
            var doctorSchedule = await GetDoctorsSchedule(doctorId, date, cancellationToken);

            var neededTimeSlots = Math.Ceiling(category.TimeSlotSize.TotalMinutes / _timeSlotSize);

            var timeSlot = doctorSchedule.SingleOrDefault(t => t.Start <= time && t.Finish >= time) 
                ?? throw new NotFoundException($"Time slot with time {time} is not found.");

            for (int i = 0; i < neededTimeSlots; i++)
            {
                var currentTime = time.AddMinutes(i * _timeSlotSize);

                if (!(doctorSchedule.SingleOrDefault(t => t.Start <= currentTime && t.Finish >= currentTime)
                    ?? throw new BadRequestException("Not enough available slots for this service")).IsAvailable)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<IEnumerable<TimeSlotModel>> GetDoctorsSchedule(Guid doctorId, DateOnly date, CancellationToken cancellationToken)
        {
            var doctorAppointments = await _unitOfWork.AppointmentRepository.GetAllApprovedByDoctorIdAsync(doctorId, date, cancellationToken);

            var numberOfTimeSlots = (int)(_endOfTheWorkingDay.ToTimeSpan().TotalMinutes - _startOfTheWorkingDay.ToTimeSpan().TotalMinutes)
                / _timeSlotSize;

            var timeSlots = new List<TimeSlotModel>();
            for (int i = 0; i < numberOfTimeSlots; i++)
            {
                timeSlots.Add(new TimeSlotModel()
                {
                    Start = _startOfTheWorkingDay.AddMinutes(_timeSlotSize * i),
                    Finish = _startOfTheWorkingDay.AddMinutes(_timeSlotSize * i + 1),
                    Id = i,
                    IsAvailable = true,
                });
            }

            foreach (var appointment in doctorAppointments)
            {
                int startIndex = (int)((appointment.Time - _startOfTheWorkingDay).TotalMinutes / _timeSlotSize);
                int endIndex = (int)((appointment.Time.AddMinutes(appointment.Service.ServiceCategory.TimeSlotSize.TotalMinutes)
                    - _startOfTheWorkingDay).TotalMinutes / _timeSlotSize);

                for (int i = startIndex; i < endIndex && i < timeSlots.Count; i++)
                {
                    timeSlots[i].IsAvailable = false;
                }
            }

            return timeSlots;
        }
    }
}
