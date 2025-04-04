﻿using Appointments.Application.Exceptions;
using Appointments.Application.Models;
using Appointments.Application.Services.Abstractions;
using Appointments.Domain.Entities;
using Appointments.Domain.Repositories.Abstractions;
using Appointments.MessageBroking.Producers.Abstractions.AppointmentProducers;
using AutoMapper;

namespace Appointments.Application.Services
{
    public class AppointmentService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IAppointmentProducer appointmentProducer) : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IAppointmentProducer _appointmentProducer = appointmentProducer;

        private readonly TimeOnly _startOfTheWorkingDay = new TimeOnly(10, 0, 0);
        private readonly TimeOnly _endOfTheWorkingDay = new TimeOnly(16, 0, 0);
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
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _appointmentProducer.PublishAppointmentUpdated(approvedAppointment, cancellationToken);

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
            newAppointment.Time = doctorSchedule.ElementAt(createAppointmentModel.TimeSlotId).Start;

            var createdAppointment = await _unitOfWork.AppointmentRepository.CreateAsync(newAppointment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _appointmentProducer.PublishAppointmentCreated(createdAppointment, cancellationToken);

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

            var doctorSchedule = await GetDoctorsSchedule(appointment.DoctorId, appointment.Date, cancellationToken);

            _mapper.Map(updateAppointmentModel, appointment);
            appointment.Time = doctorSchedule.ElementAt(updateAppointmentModel.TimeSlotId).Start;
            appointment.IsApproved = false;

            var updatedAppointment = await _unitOfWork.AppointmentRepository.UpdateAsync(appointment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _appointmentProducer.PublishAppointmentUpdated(updatedAppointment, cancellationToken);

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

            var timeSlot = doctorSchedule.SingleOrDefault(t => t.Start <= time && t.Finish > time) 
                ?? throw new NotFoundException($"Time slot with time {time} is not found.");

            for (int i = 0; i < neededTimeSlots; i++)
            {
                var currentTime = time.AddMinutes(i * _timeSlotSize);

                var currentIndex = doctorSchedule.FirstOrDefault(t => t.Start <= currentTime && t.Finish >= currentTime)
                    ?? throw new BadRequestException("Not enough available slots for this service");

                if (!currentIndex.IsAvailable)
                { 
                    return false; 
                }
            }

            return true;
        }

        public async Task<IEnumerable<TimeSlotModel>> GetDoctorsSchedule(Guid doctorId, DateOnly date, CancellationToken cancellationToken)
        {
            var doctorAppointments = await _unitOfWork.AppointmentRepository.GetAllApprovedByDoctorIdAsync(doctorId, date, cancellationToken);

            var timeSlots = GenerateTimeSlots();

            MarkUnavailableTimeSlots(timeSlots, doctorAppointments);

            return timeSlots;
        }

        private List<TimeSlotModel> GenerateTimeSlots()
        {
            var numberOfTimeSlots = (int)(_endOfTheWorkingDay.ToTimeSpan().TotalMinutes - _startOfTheWorkingDay.ToTimeSpan().TotalMinutes) / _timeSlotSize;

            return Enumerable.Range(0, numberOfTimeSlots).Select(i => new TimeSlotModel
            {
                Start = _startOfTheWorkingDay.AddMinutes(_timeSlotSize * i),
                Finish = _startOfTheWorkingDay.AddMinutes(_timeSlotSize * (i + 1)),
                Id = i,
                IsAvailable = true
            }).ToList();
        }

        private void MarkUnavailableTimeSlots(List<TimeSlotModel> timeSlots, IEnumerable<Appointment> doctorAppointments)
        {
            foreach (var appointment in doctorAppointments)
            {
                int startIndex = (int)((appointment.Time - _startOfTheWorkingDay).TotalMinutes / _timeSlotSize);
                int endIndex = (int)((appointment.Time.AddMinutes(appointment.Service.ServiceCategory.TimeSlotSize.TotalMinutes) - _startOfTheWorkingDay).TotalMinutes / _timeSlotSize);

                for (int i = startIndex; i < endIndex && i < timeSlots.Count; i++)
                {
                    timeSlots[i].IsAvailable = false;
                }
            }
        }

        public async Task<AppointmentModel> Get(Guid Id, CancellationToken cancellationToken)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetAsync(Id, cancellationToken)
                ?? throw new NotFoundException($"Appointment with id: {Id} is not found.");

            return _mapper.Map<AppointmentModel>(appointment);
        }

        public async Task<AppointmentModel> Delete(Guid id, CancellationToken cancellationToken)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetAsync(id, cancellationToken)
                ?? throw new NotFoundException($"Appointment with id: {id} is not found.");
            
            await _unitOfWork.AppointmentRepository.DeleteAsync(id, cancellationToken);

            await _appointmentProducer.PublishAppointmentDeleted(id, cancellationToken);
            
            return _mapper.Map<AppointmentModel>(appointment);
        }
    }
}
