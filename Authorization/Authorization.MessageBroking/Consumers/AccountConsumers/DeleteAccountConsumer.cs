using Authorization.Data.Repositories.Abstractions;
using Events.Account;
using MassTransit;

namespace Authorization.MessageBroking.Consumers.AccountConsumers
{
    public class DeleteAccountConsumer(IAccountRepository accountRepository) : IConsumer<AccountDeleted>
    {
        private readonly IAccountRepository _accountRepository = accountRepository;

        public async Task Consume(ConsumeContext<AccountDeleted> context)
        {
            await _accountRepository.DeleteAsync(context.Message.Id, CancellationToken.None);
        }
    }
}
