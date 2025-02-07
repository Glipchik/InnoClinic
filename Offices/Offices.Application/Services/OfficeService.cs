using AutoMapper;
using Offices.Application.Models;
using Offices.Application.Services.Abstractions;
using Offices.Data.Entities;
using Offices.Data.Repositories.Abstractions;
using Offices.Domain.Exceptions;
using Offices.MessageBroking.Producers.Abstractions;

namespace Offices.Application.Services
{
    public class OfficeService : IOfficeService
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IReceptionistRepository _receptionistsRepository;
        private readonly IMapper _mapper;
        private readonly IOfficeProducer _officeProducer;

        public OfficeService(
            IOfficeRepository officeRepository,
            IDoctorRepository doctorRepository,
            IReceptionistRepository receptionistRepository,
            IMapper mapper,
            IOfficeProducer officeProducer)
        {
            _receptionistsRepository = receptionistRepository;
            _doctorRepository = doctorRepository;
            _officeRepository = officeRepository;
            _mapper = mapper;
            _officeProducer = officeProducer;
        }

        public async Task Create(CreateOfficeModel createOfficeModel, CancellationToken cancellationToken)
        {
            var officeToCreate = _mapper.Map<Office>(createOfficeModel);
            await _officeRepository.CreateAsync(officeToCreate, cancellationToken);
            await _officeProducer.PublishOfficeCreated(officeToCreate, cancellationToken);
        }

        public async Task Update(UpdateOfficeModel updateOfficeModel, CancellationToken cancellationToken)
        {
            var officeToUpdate = await _officeRepository.GetAsync(updateOfficeModel.Id, cancellationToken)
                ?? throw new NotFoundException($"Office not found: {updateOfficeModel.Id}");

            var office = _mapper.Map<Office>(updateOfficeModel);
            await _officeRepository.UpdateAsync(office, cancellationToken);
            await _officeProducer.PublishOfficeUpdated(office, cancellationToken);
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
                officeToDelete.IsActive = false;
                await _officeRepository.UpdateAsync(officeToDelete, cancellationToken);
                await _officeProducer.PublishOfficeUpdated(officeToDelete, cancellationToken);
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
