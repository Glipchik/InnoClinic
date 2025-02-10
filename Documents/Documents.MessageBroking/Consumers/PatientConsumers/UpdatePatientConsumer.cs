using Documents.Domain.Entities;
using Documents.Domain.Repositories.Abstractions;
using AutoMapper;
using Events.Patient;
using MassTransit;

namespace Documents.MessageBroking.Consumers.PatientConsumers
{
    public class UpdatePatientConsumer(IUnitOfWork unitOfWork, IMapper mapper) : IConsumer<PatientUpdated>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<PatientUpdated> context)
        {
            _unitOfWork.PatientRepository.Update(_mapper.Map<Patient>(context.Message), CancellationToken.None);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
