using Authorization.Application.Models;
using Authorization.Application.Services.Abstractions;
using Authorization.Data.Repositories.Abstractions;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository repository, IMapper mapper) 
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AccountModel> AutoProvisionUser(AutoProvisionModel autoProvisionModel, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<AccountModel> CreateAccount(CreateAccountModel createAccountModel, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<AccountModel> FindByEmail(string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<AccountModel> FindByExternalProvider(ExternalProviderFindModel externalProviderFindModel, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<AccountModel> FindById(Guid guid, CancellationToken cancellationToken)
        {
            var account = await _repository.GetAsync(guid, cancellationToken);
            return _mapper.Map<AccountModel>(account); 
        }

        public async Task<bool> ValidateCredentials(CredentialsModel credentialsModel, CancellationToken cancellationToken)
        {
            var account = await FindByEmail(credentialsModel.Email, cancellationToken);

            if (account != null)
            {
                if (string.IsNullOrWhiteSpace(account.Password) && string.IsNullOrWhiteSpace(credentialsModel.Password))
                {
                    return true;
                }

                return account.Password.Equals(credentialsModel.Password);
            }

            return false;
        }
    }
}
