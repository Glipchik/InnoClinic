using Appointments.Domain.Entities;
using Appointments.Domain.Repositories.Abstractions;
using AutoMapper;
using Events.Service;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointments.Consumers.Consumers.ServiceConsumers
{
    public class UpdateServiceConsumer(IUnitOfWork unitOfWork, IMapper mapper) : IConsumer<ServiceUpdated>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<ServiceUpdated> context)
        {
            await _unitOfWork.ServiceRepository.UpdateAsync(_mapper.Map<Service>(context.Message), CancellationToken.None);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
