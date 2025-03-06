using AutoMapper;
using Documents.Domain.Entities;
using Documents.Domain.Repositories.Abstractions;
using Events.Account;
using MassTransit;

namespace Documents.MessageBroking.Consumers.AccountConsumers
{
    public class CreateAccountConsumer(IUnitOfWork unitOfWork, IMapper mapper) : IConsumer<AccountCreated>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<AccountCreated> context)
        {
            await _unitOfWork.AccountRepository.CreateAsync(_mapper.Map<Account>(context.Message), CancellationToken.None);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
