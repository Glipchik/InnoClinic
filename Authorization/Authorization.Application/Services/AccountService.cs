using Authorization.Application.Exceptions;
using Authorization.Application.Models;
using Authorization.Application.Services.Abstractions;
using Authorization.Data.Repositories.Abstractions;
using Authorization.Domain.Entities;
using AutoMapper;
using IdentityModel;
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

        public AccountService(IAccountRepository repository, IMapper mapper) 
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AccountModel> AutoProvisionUser(AutoProvisionModel autoProvisionModel, CancellationToken cancellationToken)
        {
            // create a list of claims that we want to transfer into our store
            var filtered = new List<Claim>();

            foreach (var claim in autoProvisionModel.Claims)
            {
                if (claim.Type == ClaimTypes.Email)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, claim.Value));
                    filtered.Add(new Claim(JwtClaimTypes.Email, claim.Value));
                }
                else if (JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.ContainsKey(claim.Type))
                {
                    filtered.Add(new Claim(JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap[claim.Type], claim.Value));
                }
                else
                {
                    filtered.Add(claim);
                }
            }

            // check if a email is available, otherwise exception
            var email = filtered.FirstOrDefault(c => c.Type == JwtClaimTypes.Email)?.Value ?? throw new BadRequestException("No Email found in claims!");
            var id = Guid.NewGuid();

            // create new user
            var user = new AccountModel
            (
                Id: id,
                Email: email,
                PhoneNumber: null,
                Role: Models.Enums.RoleModel.Patient,
                IsEmailVerified: false,
                Password: null,
                Claims: filtered,
                ProviderName: autoProvisionModel.Provider,
                ProviderSubjectId: autoProvisionModel.AccountId,
                IsActive: null,
                CreatedAt: DateTime.UtcNow,
                UpdatedAt: DateTime.UtcNow,
                CreatedBy: id,
                UpdatedBy: id
            );

            await _repository.CreateAsync(_mapper.Map<Account>(user), cancellationToken);

            return user;
        }

        public async Task<AccountModel> CreateAccount(CreateAccountModel createAccountModel, CancellationToken cancellationToken)
        {
            if (await _repository.GetByEmailAsync(createAccountModel.Email, cancellationToken) != null)
            {
                throw new Exception("That username already exists.");
            }

            var claims = new List<Claim>();
            if (!String.IsNullOrEmpty(createAccountModel.Email))
            {
                claims.Add(new Claim(ClaimTypes.Email, createAccountModel.Email));
            }

            var id = Guid.NewGuid();

            // create new user
            var user = new AccountModel
            (
                Id: id,
                Email: createAccountModel.Email,
                PhoneNumber: createAccountModel.PhoneNumber,
                Role: Models.Enums.RoleModel.Patient,
                IsEmailVerified: false,
                Password: createAccountModel.Password,
                Claims: claims,
                ProviderSubjectId: null,
                ProviderName: null,
                IsActive: null,
                CreatedAt: DateTime.UtcNow,
                UpdatedAt: DateTime.UtcNow,
                CreatedBy: id,
                UpdatedBy: id
            );

            await _repository.CreateAsync(_mapper.Map<Account>(user), cancellationToken);

            // success
            return user;
        }

        public async Task<AccountModel> FindByEmail(string email, CancellationToken cancellationToken)
        {
            var account = await _repository.GetByEmailAsync(email, cancellationToken);
            return _mapper.Map<AccountModel>(account);
        }

        public async Task<AccountModel> FindByExternalProvider(ExternalProviderFindModel externalProviderFindModel, CancellationToken cancellationToken)
        {
            var account = await _repository.GetByExternalProviderAsync(externalProviderFindModel.Provider, externalProviderFindModel.AccountId, cancellationToken);
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
