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
using Offices.Data.Repositories.Abstractions;

namespace Offices.Application.Services
{
    public class DoctorService: IDoctorService
    {
        private readonly IGenericRepository<Doctor> _doctorRepository;
        private readonly IGenericRepository<Office> _officeRepository;
        private readonly IMapper _mapper;

        public DoctorService(IGenericRepository<Doctor> doctorRepository, IGenericRepository<Office> officeRepository, IMapper mapper)
        {
            _officeRepository = officeRepository;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task Create(CreateDoctorModel createDoctorModel, CancellationToken cancellationToken)
        {
            // If specified office is not active, can't create entity
            if ((await _officeRepository.GetAsync(createDoctorModel.OfficeId, cancellationToken)) == null)
            {
                throw new RelatedObjectNotFoundException();
            }
            await _doctorRepository.CreateAsync(_mapper.Map<Doctor>(createDoctorModel), cancellationToken: cancellationToken);
        }

        public async Task Update(UpdateDoctorModel updateDoctorModel, CancellationToken cancellationToken)
        {
            // If specified office is not active, can't update entity
            if ((await _officeRepository.GetAsync(updateDoctorModel.OfficeId, cancellationToken)) == null)
            {
                throw new RelatedObjectNotFoundException();
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
