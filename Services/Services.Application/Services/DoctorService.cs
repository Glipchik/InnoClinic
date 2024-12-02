using AutoMapper;
using Services.Application.Models;
using Services.Domain.Repositories.Abstractions;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DoctorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Create(CreateDoctorModel createModel, CancellationToken cancellationToken)
        {
            // Get related to doctor specialization
            var specializationRelatedToDoctor = await _unitOfWork.GetSpecializationRepository().GetAsync(createModel.SpecializationId, cancellationToken);
            // If specified specialization is not active or not found, can't create entity
            if (specializationRelatedToDoctor == null || !specializationRelatedToDoctor.IsActive)
            {
                throw new RelatedObjectNotFoundException($"Related specialization with id {createModel.SpecializationId} is not found or not active.");
            }
            await _unitOfWork.GetDoctorRepository().CreateAsync(_mapper.Map<Doctor>(createModel), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            await _unitOfWork.GetDoctorRepository().DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<DoctorModel> Get(Guid id, CancellationToken cancellationToken)
        {
            return _mapper.Map<DoctorModel>(await _unitOfWork.GetDoctorRepository().GetAsync(id, cancellationToken));
        }

        public async Task<IEnumerable<DoctorModel>> GetAll(CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<DoctorModel>>(await _unitOfWork.GetDoctorRepository().GetAllAsync(cancellationToken));
        }

        public async Task Update(UpdateDoctorModel updateModel, CancellationToken cancellationToken)
        {
            // Get related to doctor specialization
            var specializationRelatedToDoctor = await _unitOfWork.GetSpecializationRepository().GetAsync(updateModel.SpecializationId, cancellationToken);
            // If specified specialization is not active or not found, can't update entity
            if (specializationRelatedToDoctor == null || !specializationRelatedToDoctor.IsActive)
            {
                throw new RelatedObjectNotFoundException($"Related specialization with id {updateModel.SpecializationId} is not found or not active.");
            }
            await _unitOfWork.GetDoctorRepository().UpdateAsync(_mapper.Map<Doctor>(updateModel), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
