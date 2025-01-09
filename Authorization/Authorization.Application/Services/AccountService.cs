﻿using Authorization.Application.Exceptions;
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
        private readonly IPasswordService _passwordService;

        public AccountService(IAccountRepository repository, IMapper mapper, IPasswordService passwordService) 
        {
            _repository = repository;
            _mapper = mapper;
            _passwordService = passwordService;
        }

        public async Task<AccountModel> CreateAccount(CreateAccountModel createAccountModel, CancellationToken cancellationToken)
        {
            if (await _repository.GetByEmailAsync(createAccountModel.Email, cancellationToken) != null)
            {
                throw new Exception("That username already exists.");
            }

            var id = Guid.NewGuid();

            var password = createAccountModel.Password;

            if (password == null)
            {
                password = _passwordService.GeneratePassword();
            }
            var salt = _passwordService.GenerateSalt();

            var user = new AccountModel
            (
                Id: id,
                Email: createAccountModel.Email,
                PhoneNumber: createAccountModel.PhoneNumber,
                Role: Models.Enums.RoleModel.Patient,
                IsEmailVerified: false,
                CreatedAt: DateTime.UtcNow,
                UpdatedAt: DateTime.UtcNow,
                CreatedBy: id,
                UpdatedBy: id
            );

            var account = _mapper.Map<Account>(user);
            account.PasswordHash = _passwordService.HashPassword(password, salt);
            account.PasswordSalt = salt;

            await _repository.CreateAsync(account, cancellationToken);

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

        public async Task<bool> AreCredentialsValid(CredentialsModel credentialsModel, CancellationToken cancellationToken)
        {
            var account = await _repository.GetByEmailAsync(credentialsModel.Email, cancellationToken);
            if (account != null)
            {
                if (string.IsNullOrWhiteSpace(account.PasswordHash) && string.IsNullOrWhiteSpace(credentialsModel.Password))
                {
                    return true;
                }

                return account.PasswordHash.Equals(_passwordService.HashPassword(credentialsModel.Password, account.PasswordSalt));
            }

            return false;
        }
    }
}
