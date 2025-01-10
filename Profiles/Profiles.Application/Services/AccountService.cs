using AutoMapper;
using Profiles.Application.Models;
using Profiles.Application.Services.Abstractions;
using Profiles.Domain.Entities;
using Profiles.Domain.Repositories.Abstractions;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using System.Net;
using Profiles.Application.Exceptions;

namespace Profiles.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly string _authorizationServerUrl;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, HttpClient httpClient, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpClient = httpClient;
            _authorizationServerUrl = configuration["Authorization:ServerUrl"];
        }

        public async Task<AccountModel> Create(CreateAccountModel createAccountModel, Guid authorId, CancellationToken cancellationToken)
        {
            var createAccountAuthorizationServerModel = _mapper.Map<CreateAccountAuthorizationServerModel>(createAccountModel);
            var response = await SendCreateRequest(createAccountAuthorizationServerModel, cancellationToken);

            var account = _mapper.Map<Account>(response);
            account.CreatedBy = authorId;
            account.UpdatedBy = authorId;
            account.CreatedAt = DateTime.UtcNow;
            account.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.AccountRepository.CreateAsync(account, cancellationToken);

            return _mapper.Map<AccountModel>(account);
        }

        private async Task<AccountModel> SendCreateRequest(CreateAccountAuthorizationServerModel createAccountAuthorizationServerModel, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_authorizationServerUrl}api/Accounts", createAccountAuthorizationServerModel, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new BadRequestException(response.ReasonPhrase);
                }
            }

            var accountModel = await response.Content.ReadFromJsonAsync<AccountModel>(cancellationToken: cancellationToken);
            return accountModel;
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
    }
}