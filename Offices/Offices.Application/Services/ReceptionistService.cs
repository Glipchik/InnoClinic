using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Offices.Application.Models;
using Offices.Application.Services.Abstractions;
using Offices.Data.Entities;
using Offices.Data.Repositories;
using Offices.Data.Repositories.Abstractions;
using Offices.Domain.Exceptions;

namespace Offices.Application.Services
{
    public class ReceptionistService: IReceptionistService
    {
        private readonly IGenericRepository<Receptionist> _receptionistRepository;
        private readonly IOfficeRepository _officeRepository;
        private readonly IMapper _mapper;

        public ReceptionistService(IGenericRepository<Receptionist> receptionistRepository, IOfficeRepository officeRepository, IMapper mapper)
        {
            _officeRepository = officeRepository;
            _receptionistRepository = receptionistRepository;
            _mapper = mapper;
        }
        public async Task Create(CreateReceptionistModel createReceptionistModel, CancellationToken cancellationToken)
        {
            // Get related to receptionist office
            var officeRelatedToReceptionist = await _officeRepository.GetAsync(createReceptionistModel.OfficeId, cancellationToken);
            // If specified office is not active, can't create entity
            if (officeRelatedToReceptionist == null)
            {
                // Throw exception if there is no active office with this id for this worker
                throw new RelatedObjectNotFoundException($"Can't create receptionist because office with id {createReceptionistModel.OfficeId} is not active or doesn't exist!");
            }
            await _receptionistRepository.CreateAsync(_mapper.Map<Receptionist>(createReceptionistModel), cancellationToken);
        }

        public async Task Update(UpdateReceptionistModel updateReceptionistModel, CancellationToken cancellationToken)
        {
            // Get related to receptionist office
            var officeRelatedToReceptionist = await _officeRepository.GetAsync(updateReceptionistModel.OfficeId, cancellationToken);
            // If specified office is not active, can't update entity
            if (officeRelatedToReceptionist == null)
            {
                // Throw exception if there is no active office with this id for this worker
                throw new RelatedObjectNotFoundException($"Can't update receptionist with id {updateReceptionistModel.Id} because office with id {updateReceptionistModel.OfficeId} is not active or doesn't exist!");
            }
            var receptionist = _mapper.Map<Receptionist>(updateReceptionistModel);
            await _receptionistRepository.UpdateAsync(receptionist, cancellationToken);
        }

        public async Task<ReceptionistModel> Get(string id, CancellationToken cancellationToken)
        {
            var receptionist = await _receptionistRepository.GetAsync(id, cancellationToken);
            return _mapper.Map<ReceptionistModel>(receptionist);
        }

        public async Task<IEnumerable<ReceptionistModel>> GetAll(CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<ReceptionistModel>>(await _receptionistRepository.GetAllAsync(cancellationToken));
        }

        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            await _receptionistRepository.DeleteAsync(id, cancellationToken);
        }
    }
}
