using AutoMapper;
using Profiles.Application.Models;
using Profiles.Application.Services.Abstractions;
using Profiles.Domain.Entities;
using Profiles.Domain.Repositories.Abstractions;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using System.Net;
using Profiles.Application.Exceptions;
using System.Net.Http.Headers;
using IdentityModel.Client;
using System.Security.Principal;

namespace Profiles.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthorizationServerManager _authorizationServerManager;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, IAuthorizationServerManager authorizationServerManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authorizationServerManager = authorizationServerManager;
        }

        public async Task<AuthorizationAccountModel> Create(CreateAccountModel createAccountModel, CancellationToken cancellationToken)
        {
            var createAccountAuthorizationServerModel = new CreateAccountAuthorizationServerModel
            {
                RoleId = (int)createAccountModel.Role,
                Email = createAccountModel.Email,
                PhoneNumber = createAccountModel.PhoneNumber
            };

            var response = await _authorizationServerManager.CreateAccount(createAccountAuthorizationServerModel, cancellationToken);

            var account = _mapper.Map<Account>(response);

            await _unitOfWork.AccountRepository.CreateAsync(account, createAccountModel.AuthorId, cancellationToken);

            var authorizationAccountModel = _mapper.Map<AuthorizationAccountModel>(account);
            authorizationAccountModel.PhotoFileName = String.Empty;
            authorizationAccountModel.Role = createAccountModel.Role;

            return authorizationAccountModel;
        }

        public async Task<AccountModel> FindByEmail(string email, CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.AccountRepository.FindByEmailAsync(email, cancellationToken);
            return _mapper.Map<AccountModel>(account);
        }

        public async Task<AccountModel> Get(Guid id, CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.AccountRepository.GetAsync(id, cancellationToken);
            return _mapper.Map<AccountModel>(account);
        }

        public async Task<AccountModel> CreateFromAuthServer(CreateAccountFromAuthServerModel createAccountFromAuthServerModel, CancellationToken cancellationToken)
        {
            var account = _mapper.Map<Account>(createAccountFromAuthServerModel);

            await _unitOfWork.AccountRepository.CreateAsync(account, createAccountFromAuthServerModel.AuthorId, cancellationToken);

            var authorizationAccountModel = _mapper.Map<AccountModel>(account);
            authorizationAccountModel.PhotoFileName = String.Empty;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return authorizationAccountModel;
        }
    }
}