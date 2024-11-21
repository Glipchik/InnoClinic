using System.Threading;
using AutoMapper;
using DnsClient.Internal;
using Offices.Application.Models;
using Offices.Application.Services.Abstractions;
using Offices.Data.Entities;
using Offices.Data.Exceptions;
using Offices.Data.Repositories.Abstractions;

namespace Offices.Application.Services
{
    public class OfficeService : IOfficeService
    {
        private readonly IGenericRepository<Office> _officeRepository;
        private readonly IGenericRepository<Doctor> _doctorRepository;
        private readonly IGenericRepository<Receptionist> _receptionistsRepository;
        private readonly IMapper _mapper;

        public OfficeService(IGenericRepository<Office> officeRepository, IGenericRepository<Doctor> doctorRepository, IGenericRepository<Receptionist> receptionistRepository, IMapper mapper)
        {
            _receptionistsRepository = receptionistRepository;
            _doctorRepository = doctorRepository;
            _officeRepository = officeRepository;
            _mapper = mapper;
        }

        public async Task Create(CreateOfficeModel createOfficeModel, CancellationToken cancellationToken)
        {
            await _officeRepository.CreateAsync(_mapper.Map<Office>(createOfficeModel), cancellationToken);
        }

        public async Task Update(UpdateOfficeModel updateOfficeModel, CancellationToken cancellationToken)
        {
            var office = _mapper.Map<Office>(updateOfficeModel);
            await _officeRepository.UpdateAsync(office, cancellationToken);
        }

        public async Task<OfficeModel> Get(string id, CancellationToken cancellationToken)
        {
            var office = await _officeRepository.GetAsync(id, cancellationToken);
            return _mapper.Map<OfficeModel>(office);
        }

        public async Task<IEnumerable<OfficeModel>> GetAll(CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<OfficeModel>>(await _officeRepository.GetAllAsync(cancellationToken));
        }

        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            // If there are doctors or receptionists found in this office, can't make this office inactive
            if (await CheckIfThereAreWorkersInOffice(id, cancellationToken))
            {
                // Throw exception if someone works in this office, need to free if first
                throw new RelatedObjectFoundException();
            }
            else
            {
                await _officeRepository.DeleteAsync(id, cancellationToken);
            }
        }

        private async Task<bool> CheckIfThereAreWorkersInOffice(string officeId, CancellationToken cancellationToken)
        {
            var areAnyDoctorsInOffice = (await _doctorRepository.GetAllAsync(cancellationToken)).Any(d => d.OfficeId == officeId);
            var areAnyReceptionistsInOffice = (await _receptionistsRepository.GetAllAsync(cancellationToken)).Any(r => r.OfficeId == officeId);

            // If someone works in the office, returns true
            return areAnyDoctorsInOffice || areAnyReceptionistsInOffice;
        }
    }
}
