using AutoMapper;
using Events.Receptionist;
using MassTransit;
using Offices.Data.Entities;
using Offices.Data.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offices.MessageBroking.Consumers.ReceptionistConsumers
{
    public class UpdateReceptionistConsumer(IReceptionistRepository receptionistRepository, IMapper mapper) : IConsumer<ReceptionistUpdated>
    {
        private readonly IReceptionistRepository _receptionistRepository = receptionistRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<ReceptionistUpdated> context)
        {
            await _receptionistRepository.UpdateAsync(_mapper.Map<Receptionist>(context.Message), CancellationToken.None);
        }
    }
}
