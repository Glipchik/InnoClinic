using AutoMapper;
using Services.Application.Models;
using Services.Application.Repositories.Abstractions;
using Services.Application.Services.Abstractions;
using Services.Domain.Entities;
using Services.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly ISpecializationRepository _specializationRepository;
        private readonly IMapper _mapper;

        public DoctorService(IDoctorRepository doctorRepository, IMapper mapper, ISpecializationRepository specializationRepository)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
            _specializationRepository = specializationRepository;
        }

        public async Task Create(CreateDoctorModel createModel, CancellationToken cancellationToken)
        {
            // Get related to doctor specialization
            var specializationRelatedToDoctor = await _specializationRepository.GetAsync(createModel.SpecializationId, cancellationToken);
            // If specified specialization is not active or not found, can't create entity
            if (specializationRelatedToDoctor == null || !specializationRelatedToDoctor.IsActive)
            {
                throw new RelatedObjectNotFoundException($"Related specialization with id {createModel.SpecializationId} is not found or not active.");
            }
            await _doctorRepository.CreateAsync(_mapper.Map<Doctor>(createModel), cancellationToken);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            await _doctorRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task<DoctorModel> Get(Guid id, CancellationToken cancellationToken)
        {
            return _mapper.Map<DoctorModel>(await _doctorRepository.GetAsync(id, cancellationToken));
        }

        public async Task<IEnumerable<DoctorModel>> GetAll(CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<DoctorModel>>(await _doctorRepository.GetAllAsync(cancellationToken));
        }

        public async Task Update(UpdateDoctorModel updateModel, CancellationToken cancellationToken)
        {
            // Get related to doctor specialization
            var specializationRelatedToDoctor = await _specializationRepository.GetAsync(updateModel.SpecializationId, cancellationToken);
            // If specified specialization is not active or not found, can't update entity
            if (specializationRelatedToDoctor == null || !specializationRelatedToDoctor.IsActive)
            {
                throw new RelatedObjectNotFoundException($"Related specialization with id {updateModel.SpecializationId} is not found or not active.");
            }
            await _doctorRepository.UpdateAsync(_mapper.Map<Doctor>(updateModel), cancellationToken);
        }
    }
}
