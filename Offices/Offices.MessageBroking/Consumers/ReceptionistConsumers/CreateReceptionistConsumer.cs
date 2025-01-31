using AutoMapper;
using Events.Doctor;
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
    public class CreateReceptionistConsumer(IReceptionistRepository receptionistRepository, IMapper mapper) : IConsumer<ReceptionistCreated>
    {
        private readonly IReceptionistRepository _receptionistRepository = receptionistRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<ReceptionistCreated> context)
        {
            await _receptionistRepository.CreateAsync(_mapper.Map<Receptionist>(context.Message), CancellationToken.None);
        }
    }
}
