using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Offices.Application.Models;
using Offices.Application.Services.Abstractions;
using Offices.Data.Entities;
using Offices.Data.Exceptions;
using Offices.Data.Repositories;
using Offices.Data.Repositories.Abstractions;

namespace Offices.Application.Services
{
    public class ReceptionistService: IReceptionistService
    {
        private readonly IGenericRepository<Receptionist> _receptionistRepository;
        private readonly IGenericRepository<Office> _officeRepository;
        private readonly IMapper _mapper;

        public ReceptionistService(IGenericRepository<Receptionist> receptionistRepository, IGenericRepository<Office> officeRepository, IMapper mapper)
        {
            _officeRepository = officeRepository;
            _receptionistRepository = receptionistRepository;
            _mapper = mapper;
        }
        public async Task Create(CreateReceptionistModel createReceptionistModel)
        {
            // If specified office is not active, can't create entity
            if ((await _officeRepository.GetAsync(createReceptionistModel.OfficeId)) == null)
            {
                throw new RelatedObjectNotFoundException();
            }
            await _receptionistRepository.CreateAsync(_mapper.Map<Receptionist>(createReceptionistModel));
        }

        public async Task Update(UpdateReceptionistModel updateReceptionistModel)
        {
            // If specified office is not active, can't update entity
            if ((await _officeRepository.GetAsync(updateReceptionistModel.OfficeId)) == null)
            {
                throw new RelatedObjectNotFoundException();
            }
            var receptionist = _mapper.Map<Receptionist>(updateReceptionistModel);
            await _receptionistRepository.UpdateAsync(receptionist);
        }

        public async Task<ReceptionistModel> Get(string id)
        {
            var receptionist = await _receptionistRepository.GetAsync(id);
            return _mapper.Map<ReceptionistModel>(receptionist);
        }

        public async Task<IEnumerable<ReceptionistModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<ReceptionistModel>>(await _receptionistRepository.GetAllAsync());
        }

        public async Task Delete(string id)
        {
            await _receptionistRepository.DeleteAsync(id);
        }
    }
}
