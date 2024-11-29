using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Offices.Application.Models;
using Offices.Application.Services.Abstractions;
using Offices.Data.Entities;
using Offices.Data.Repositories.Abstractions;
using Offices.Domain.Exceptions;

namespace Offices.Application.Services
{
    public class DoctorService: IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IOfficeRepository _officeRepository;
        private readonly IMapper _mapper;

        public DoctorService(IDoctorRepository doctorRepository, IOfficeRepository officeRepository, IMapper mapper)
        {
            _officeRepository = officeRepository;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task Create(CreateDoctorModel createDoctorModel, CancellationToken cancellationToken)
        {
            // Get related to doctor office
            var officeRelatedToDoctor = await _officeRepository.GetAsync(createDoctorModel.OfficeId, cancellationToken);
            // If specified office is not active or not found, can't create entity
            if (officeRelatedToDoctor == null || !officeRelatedToDoctor.IsActive)
            {
                // Throw exception if there is no active office with this id for this worker
                throw new RelatedObjectNotFoundException($"Can't create doctor because office with id {createDoctorModel.OfficeId} is not active or doesn't exist!");
            }
            await _doctorRepository.CreateAsync(_mapper.Map<Doctor>(createDoctorModel), cancellationToken: cancellationToken);
        }

        public async Task Update(UpdateDoctorModel updateDoctorModel, CancellationToken cancellationToken)
        {
            // Get related to doctor office
            var officeRelatedToDoctor = await _officeRepository.GetAsync(updateDoctorModel.OfficeId, cancellationToken);
            // If specified office is not active or not found, can't update entity
            if (officeRelatedToDoctor == null || !officeRelatedToDoctor.IsActive)
            {
                // Throw exception if there is no active office with this id for this worker
                throw new RelatedObjectNotFoundException($"Can't update doctor with id {updateDoctorModel.Id} because office with id {updateDoctorModel.OfficeId} is not active or doesn't exist!");
            }
            var doctor = _mapper.Map<Doctor>(updateDoctorModel);
            await _doctorRepository.UpdateAsync(doctor, cancellationToken);
        }

        public async Task<DoctorModel> Get(string id, CancellationToken cancellationToken)
        {
            var doctor = await _doctorRepository.GetAsync(id, cancellationToken);
            return _mapper.Map<DoctorModel>(doctor);
        }

        public async Task<IEnumerable<DoctorModel>> GetAll(CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<DoctorModel>>(await _doctorRepository.GetAllAsync(cancellationToken));
        }

        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            await _doctorRepository.DeleteAsync(id, cancellationToken);
        }
    }
}
