﻿using Appointments.Domain.Entities;
using Appointments.Domain.Repositories.Abstractions;
using AutoMapper;
using Events.Doctor;
using MassTransit;

namespace Appointments.Consumers.Consumers.DoctorConsumers
{
    public class CreateDoctorConsumer(IUnitOfWork unitOfWork, IMapper mapper) : IConsumer<DoctorCreated>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<DoctorCreated> context)
        {
            await _unitOfWork.DoctorRepository.CreateAsync(_mapper.Map<Doctor>(context.Message), CancellationToken.None);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
