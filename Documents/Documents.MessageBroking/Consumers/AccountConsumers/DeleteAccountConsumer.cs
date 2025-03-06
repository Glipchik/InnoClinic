using AutoMapper;
using Documents.Domain.Entities;
using Documents.Domain.Repositories.Abstractions;
using Events.Account;
using MassTransit;

namespace Documents.MessageBroking.Consumers.AccountConsumers
{
    public class DeleteAccountConsumer(IUnitOfWork unitOfWork) : IConsumer<AccountDeleted>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Consume(ConsumeContext<AccountDeleted> context)
        {
            await _unitOfWork.AccountRepository.DeleteAsync(context.Message.Id, CancellationToken.None);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
