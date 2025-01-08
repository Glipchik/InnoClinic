using AutoMapper;
using Profiles.Application.Models;
using Profiles.Application.Services.Abstractions;
using Profiles.Domain.Entities;
using Profiles.Domain.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHashService _hashService;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, IHashService hashService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hashService = hashService;
        }

        public async Task Create(CreateAccountModel createAccountModel, Guid authorId, CancellationToken cancellationToken)
        {
            var account = _mapper.Map<Account>(createAccountModel);
            account.CreatedBy = authorId;
            account.UpdatedBy = authorId;
            account.PasswordHash = _hashService.HashString(createAccountModel.Password);
            account.CreatedAt = DateTime.UtcNow;
            account.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.AccountRepository.CreateAsync(account, cancellationToken);
        }

        public Task<AccountModel> FindByEmail(string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
