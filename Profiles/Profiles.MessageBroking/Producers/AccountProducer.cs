using AutoMapper;
using MassTransit.Transports;
using MassTransit;
using Profiles.Domain.Entities;
using Profiles.MessageBroking.Producers.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Events.ServiceCategory;
using Events.Account;

namespace Profiles.MessageBroking.Producers
{
    public class AccountProducer(IPublishEndpoint publishEndpoint, IMapper mapper) : IAccountProducer
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IMapper _mapper = mapper;

        public async Task PublishAccountCreated(Account account, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(_mapper.Map<AccountCreated>(account), cancellationToken);
        }

        public async Task PublishAccountDeleted(Guid id, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(new AccountDeleted() { Id = id }, cancellationToken);
        }
    }
}
