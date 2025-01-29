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

namespace Offices.Consumers.ReceptionistConsumers
{
    public class DeleteReceptionistConsumer(IReceptionistRepository receptionistRepository) : IConsumer<ReceptionistDeleted>
    {
        private readonly IReceptionistRepository _receptionistRepository = receptionistRepository;

        public async Task Consume(ConsumeContext<ReceptionistDeleted> context)
        {
            await _receptionistRepository.DeleteAsync(context.Message.Id, CancellationToken.None);
        }
    }
}
