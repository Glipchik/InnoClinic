using Authorization.Application.Exceptions;
using Authorization.Application.Models;
using Authorization.Application.Services.Abstractions;
using Authorization.Data.Repositories.Abstractions;
using Authorization.Domain.Entities;
using AutoMapper;
using IdentityModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHashService _hashService;

        public AccountService(IAccountRepository repository, IMapper mapper, IHashService hashService) 
        {
            _repository = repository;
            _mapper = mapper;
            _hashService = hashService;
        }

        public async Task<AccountModel> CreateAccount(CreateAccountModel createAccountModel, CancellationToken cancellationToken)
        {
            if (await _repository.GetByEmailAsync(createAccountModel.Email, cancellationToken) != null)
            {
                throw new Exception("That username already exists.");
            }

            var id = Guid.NewGuid();

            var user = new AccountModel
            (
                Id: id,
                Email: createAccountModel.Email,
                PhoneNumber: createAccountModel.PhoneNumber,
                Role: Models.Enums.RoleModel.Patient,
                IsEmailVerified: false,
                PasswordHash: _hashService.HashString(createAccountModel.Password),
                CreatedAt: DateTime.UtcNow,
                UpdatedAt: DateTime.UtcNow,
                CreatedBy: id,
                UpdatedBy: id
            );

            await _repository.CreateAsync(_mapper.Map<Account>(user), cancellationToken);

            return user;
        }

        public async Task<AccountModel> FindByEmail(string email, CancellationToken cancellationToken)
        {
            var account = await _repository.GetByEmailAsync(email, cancellationToken);
            return _mapper.Map<AccountModel>(account);
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
                if (string.IsNullOrWhiteSpace(account.PasswordHash) && string.IsNullOrWhiteSpace(credentialsModel.Password))
                {
                    return true;
                }

                return account.PasswordHash.Equals(_hashService.HashString(credentialsModel.Password));
            }

            return false;
        }
    }
}
