using AutoMapper;
using Services.Application.Models;
using Services.Application.Models.Enums;
using Services.Domain.Repositories.Abstractions;
using Services.Application.Services.Abstractions;
using Services.Domain.Entities;
using Services.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Services
{
    public class SpecializationService : ISpecializationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SpecializationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Create(CreateSpecializationModel createModel, CancellationToken cancellationToken)
        {
            await _unitOfWork.GetSpecializationRepository().CreateAsync(_mapper.Map<Specialization>(createModel), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            var doctorsWithSpecializationId = await _unitOfWork.GetDoctorRepository().GetActiveDoctorsWithSpecializationAsync(id, cancellationToken);

            foreach (var doctor in doctorsWithSpecializationId)
            {
                doctor.Status = _mapper.Map<DoctorStatus>(DoctorStatusModel.Inactive);
                await _unitOfWork.GetDoctorRepository().UpdateAsync(doctor, cancellationToken);
            }

            await _unitOfWork.GetSpecializationRepository().DeleteAsync(id, cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
        }

        public async Task<SpecializationModel> Get(Guid id, CancellationToken cancellationToken)
        {
            return _mapper.Map<SpecializationModel>(await _unitOfWork.GetSpecializationRepository().GetAsync(id, cancellationToken));
        }

        public async Task<IEnumerable<SpecializationModel>> GetAll(CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<SpecializationModel>>(await _unitOfWork.GetSpecializationRepository().GetAllAsync(cancellationToken));
        }

        public async Task Update(UpdateSpecializationModel updateModel, CancellationToken cancellationToken)
        {
            await _unitOfWork.GetSpecializationRepository().UpdateAsync(_mapper.Map<Specialization>(updateModel), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
