using AutoMapper;
using Services.Application.Models;
using Services.Application.Models.Enums;
using Services.Application.Repositories.Abstractions;
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
        private readonly IDoctorRepository _doctorRepository;
        private readonly ISpecializationRepository _specializationRepository;
        private readonly IMapper _mapper;

        public SpecializationService(IDoctorRepository doctorRepository, ISpecializationRepository specializationRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _specializationRepository = specializationRepository;
            _mapper = mapper;
        }

        public async Task Create(CreateSpecializationModel createModel, CancellationToken cancellationToken)
        {
            await _specializationRepository.CreateAsync(_mapper.Map<Specialization>(createModel), cancellationToken);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var doctorsWithSpecializationId = await _doctorRepository.GetActiveDoctorsWithSpecializationAsync(id, cancellationToken);

            foreach (var doctor in doctorsWithSpecializationId)
            {
                doctor.Status = _mapper.Map<DoctorStatus>(DoctorStatusModel.Inactive);
                await _doctorRepository.UpdateAsync(doctor, cancellationToken);
            }

            await _specializationRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task<SpecializationModel> Get(Guid id, CancellationToken cancellationToken)
        {
            return _mapper.Map<SpecializationModel>(await _specializationRepository.GetAsync(id, cancellationToken));
        }

        public async Task<IEnumerable<SpecializationModel>> GetAll(CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<SpecializationModel>>(await _specializationRepository.GetAllAsync(cancellationToken));
        }

        public async Task Update(UpdateSpecializationModel updateModel, CancellationToken cancellationToken)
        {
            await _specializationRepository.UpdateAsync(_mapper.Map<Specialization>(updateModel), cancellationToken);
        }
    }
}
