using AutoMapper;
using Profiles.Application.Exceptions;
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
    public class ReceptionistService : IReceptionistService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public ReceptionistService(IUnitOfWork unitOfWork, IMapper mapper, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task Create(CreateReceptionistModel createReceptionistModel, CreateAccountModel createAccountModel, Guid authorId, CancellationToken cancellationToken)
        {
            _unitOfWork.BeginTransaction(cancellationToken: cancellationToken);
            await _accountService.Create(createAccountModel, authorId, cancellationToken);

            //TODO: Send new account to message broker

            var receptionist = _mapper.Map<Receptionist>(createReceptionistModel);

            var relatedAccount = await _accountService.FindByEmail(createAccountModel.Email, cancellationToken);

            if (relatedAccount == null)
            {
                throw new NotFoundException($"Related account with email: {createAccountModel.Email} is not found. Can't create.");
            }

            receptionist.AccountId = relatedAccount.Id;

            await _unitOfWork.ReceptionistRepository.CreateAsync(receptionist, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var receptionistToDelete = await _unitOfWork.ReceptionistRepository.GetAsync(id, cancellationToken);
            if (receptionistToDelete == null)
            {
                throw new NotFoundException($"Receptionist with id: {id} is not found. Can't delete.");
            }

            await _unitOfWork.ReceptionistRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<ReceptionistModel> Get(Guid id, CancellationToken cancellationToken)
        {
            return _mapper.Map<ReceptionistModel>(await _unitOfWork.ReceptionistRepository.GetAsync(id, cancellationToken));
        }

        public async Task Update(UpdateReceptionistModel updateReceptionistModel, CancellationToken cancellationToken)
        {
            var receptionistToUpdate = await _unitOfWork.ReceptionistRepository.GetAsync(updateReceptionistModel.Id, cancellationToken);
            if (receptionistToUpdate == null)
            {
                throw new NotFoundException($"Receptionist with id: {updateReceptionistModel.Id} is not found. Can't update.");
            }

            _mapper.Map(updateReceptionistModel, receptionistToUpdate);

            await _unitOfWork.ReceptionistRepository.UpdateAsync(receptionistToUpdate, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
