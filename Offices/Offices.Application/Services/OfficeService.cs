using System.Threading;
using AutoMapper;
using DnsClient.Internal;
using Offices.Application.Models;
using Offices.Application.Services.Abstractions;
using Offices.Data.Entities;
using Offices.Data.Repositories.Abstractions;
using Offices.Domain.Exceptions;

namespace Offices.Application.Services
{
    public class OfficeService : IOfficeService
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IReceptionistRepository _receptionistsRepository;
        private readonly IMapper _mapper;

        public OfficeService(IOfficeRepository officeRepository, IDoctorRepository doctorRepository, IReceptionistRepository receptionistRepository, IMapper mapper)
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
            var officeToUpdate = await _officeRepository.GetAsync(updateOfficeModel.Id, cancellationToken)
                ?? throw new NotFoundException($"Office not found: {updateOfficeModel.Id}");

            var office = _mapper.Map<Office>(updateOfficeModel);
            await _officeRepository.UpdateAsync(office, cancellationToken);
        }

        public async Task<OfficeModel> Get(Guid id, CancellationToken cancellationToken)
        {
            var office = await _officeRepository.GetAsync(id, cancellationToken)
                ?? throw new NotFoundException($"Office not found: {id}");

            return _mapper.Map<OfficeModel>(office);
        }

        public async Task<IEnumerable<OfficeModel>> GetAll(CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<OfficeModel>>(await _officeRepository.GetAllAsync(cancellationToken));
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var officeToDelete = await _officeRepository.GetAsync(id, cancellationToken)
                ?? throw new NotFoundException($"Office not found: {id}");

            // If there are doctors or receptionists found in this office, can't make this office inactive
            if (await CheckIfThereAreActiveDoctorsOrReceptionistsInOffice(id, cancellationToken))
            {
                // Throw exception if someone works in this office, need to free if first
                throw new RelatedObjectFoundException($"Can't delete office with id {id}, because someone works there, move them!");
            }
            else
            {
                await _officeRepository.DeleteAsync(id, cancellationToken);
            }
        }

        private async Task<bool> CheckIfThereAreActiveDoctorsOrReceptionistsInOffice(Guid officeId, CancellationToken cancellationToken)
        {
            var doctorsInOfficeCount = (await _doctorRepository.GetActiveDoctorsFromOffice(officeId, cancellationToken)).Count();
            var receptionistsInOfficeCount = (await _receptionistsRepository.GetActiveReceptionistsFromOffice(officeId, cancellationToken)).Count();

            // If someone works in the office, returns true
            return (doctorsInOfficeCount > 0) || (receptionistsInOfficeCount > 0);
        }
    }
}
