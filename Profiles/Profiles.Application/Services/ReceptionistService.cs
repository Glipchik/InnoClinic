using AutoMapper;
using Profiles.Application.Exceptions;
using Profiles.Application.Models;
using Profiles.Application.Services.Abstractions;
using Profiles.Domain.Entities;
using Profiles.Domain.Repositories.Abstractions;
using Profiles.MessageBroking.Producers.Abstractions;
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
        private readonly IFileService _fileService;
        private readonly IReceptioinistProducer _receptionistProducer;
        private readonly IAccountProducer _accountProducer;

        public ReceptionistService(IUnitOfWork unitOfWork, IMapper mapper, IAccountService accountService, IFileService fileService,
            IReceptioinistProducer receptionistProducer,
            IAccountProducer accountProducer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountService = accountService;
            _fileService = fileService;
            _receptionistProducer = receptionistProducer;
            _accountProducer = accountProducer;
        }

        public async Task Create(
            CreateReceptionistModel createReceptionistModel, 
            CreateAccountModel createAccountModel, 
            FileModel? fileModel,
            CancellationToken cancellationToken)
        {
            try
            {
                using (var transaction = _unitOfWork.BeginTransaction(cancellationToken: cancellationToken))
                {

                    var officeRelatedToDoctor = await _unitOfWork.OfficeRepository.GetAsync(createReceptionistModel.OfficeId, cancellationToken: cancellationToken);
                    if (officeRelatedToDoctor == null || !officeRelatedToDoctor.IsActive)
                    {
                        throw new RelatedObjectNotFoundException($"Related office with id {createReceptionistModel.OfficeId} is not found or not active.");
                    }

                    var createdAccount = await _accountService.Create(createAccountModel, cancellationToken);

                    var receptionist = _mapper.Map<Receptionist>(createReceptionistModel);

                    receptionist.AccountId = createdAccount.Id;

                    await _unitOfWork.ReceptionistRepository.CreateAsync(receptionist, cancellationToken);

                    if (fileModel != null)
                    {
                        await _fileService.Upload(fileModel.FileName, fileModel.FileStream, fileModel.ContentType);

                        receptionist.Account.PhotoFileName = fileModel.FileName;
                    }

                    await transaction.CommitAsync(cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    await _receptionistProducer.PublishReceptionistCreated(receptionist, cancellationToken);
                    await _accountProducer.PublishAccountCreated(_mapper.Map<Account>(createdAccount), cancellationToken);
                }
            }
            catch
            {
                if (fileModel != null && await _fileService.DoesFileExist(fileModel.FileName))
                {
                    await _fileService.Remove(fileModel.FileName);
                }

                throw;
            }
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var receptionistToDelete = await _unitOfWork.ReceptionistRepository.GetAsync(id, cancellationToken: cancellationToken)
                ?? throw new NotFoundException($"Receptionist with id: {id} is not found. Can't delete.");

            try
            {
                using (var transaction = _unitOfWork.BeginTransaction(cancellationToken: cancellationToken))
                {
                    await _unitOfWork.ReceptionistRepository.DeleteAsync(id, cancellationToken);

                    await _unitOfWork.AccountRepository.DeleteAsync(receptionistToDelete.AccountId, receptionistToDelete.Id, cancellationToken);

                    await _fileService.Remove(receptionistToDelete.Account.PhotoFileName);

                    await transaction.CommitAsync(cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    await _receptionistProducer.PublishReceptionistDeleted(id, cancellationToken);
                    await _accountProducer.PublishAccountDeleted(receptionistToDelete.AccountId, cancellationToken);
                }
            }
            catch
            {
                if (await _fileService.DoesFileExist(receptionistToDelete.Account.PhotoFileName))
                {
                    await _fileService.Remove(receptionistToDelete.Account.PhotoFileName);
                }

                throw;
            }
        }

        public async Task<ReceptionistModel> Get(Guid id, CancellationToken cancellationToken)
        {
            return _mapper.Map<ReceptionistModel>(await _unitOfWork.ReceptionistRepository.GetAsync(id, cancellationToken: cancellationToken));
        }

        public async Task<IEnumerable<ReceptionistModel>> GetAll(CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<ReceptionistModel>>(await _unitOfWork.ReceptionistRepository.GetAllAsync(cancellationToken));
        }

        public async Task<ReceptionistModel> GetByAccountId(Guid id, CancellationToken cancellationToken)
        {
            return _mapper.Map<ReceptionistModel>(await _unitOfWork.ReceptionistRepository.GetByAccountIdAsync(id, cancellationToken: cancellationToken));
        }

        public async Task Update(
            UpdateReceptionistModel updateReceptionistModel, 
            FileModel? fileModel,
            CancellationToken cancellationToken)
        {
            var officeRelatedToDoctor = await _unitOfWork.OfficeRepository.GetAsync(updateReceptionistModel.OfficeId, cancellationToken: cancellationToken);
            if (officeRelatedToDoctor == null || !officeRelatedToDoctor.IsActive)
            {
                throw new RelatedObjectNotFoundException($"Related office with id {updateReceptionistModel.OfficeId} is not found or not active.");
            }


            var receptionistToUpdate = await _unitOfWork.ReceptionistRepository.GetAsync(updateReceptionistModel.Id, isIncluded: false, cancellationToken: cancellationToken)
                ?? throw new NotFoundException($"Receptionist with id: {updateReceptionistModel.Id} is not found. Can't update.");

            if (fileModel != null)
            {
                var accountToUpdate = await _unitOfWork.AccountRepository.GetAsync(receptionistToUpdate.AccountId, cancellationToken: cancellationToken);

                if (accountToUpdate == null)
                {
                    throw new RelatedObjectNotFoundException($"Related account with id {receptionistToUpdate.AccountId} is not found.");
                }

                await _fileService.Remove(receptionistToUpdate.Account.PhotoFileName);
                await _fileService.Upload(fileModel.FileName, fileModel.FileStream, fileModel.ContentType);

                accountToUpdate.PhotoFileName = fileModel.FileName;

                await _unitOfWork.AccountRepository.UpdateAsync(accountToUpdate, updateReceptionistModel.AuthorId, cancellationToken);
            }

            _mapper.Map(updateReceptionistModel, receptionistToUpdate);

            await _unitOfWork.ReceptionistRepository.UpdateAsync(receptionistToUpdate, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _receptionistProducer.PublishReceptionistUpdated(receptionistToUpdate, cancellationToken);
        }
    }
}
