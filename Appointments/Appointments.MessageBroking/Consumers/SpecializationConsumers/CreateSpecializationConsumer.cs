using Appointments.Domain.Entities;
using Appointments.Domain.Repositories.Abstractions;
using AutoMapper;
using Events.Specialization;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointments.Consumers.Consumers.SpecializationConsumers
{
    public class CreateSpecializationConsumer(IUnitOfWork unitOfWork, IMapper mapper) : IConsumer<SpecializationCreated>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<SpecializationCreated> context)
        {
            await _unitOfWork.SpecializationRepository.CreateAsync(_mapper.Map<Specialization>(context.Message), CancellationToken.None);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
