using Appointments.Domain.Entities;
using Appointments.Domain.Repositories.Abstractions;
using AutoMapper;
using Events.Patient;
using MassTransit;

namespace Appointments.Consumers.Consumers.PatientConsumers
{
    public class CreatePatientConsumer(IUnitOfWork unitOfWork, IMapper mapper) : IConsumer<PatientCreated>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<PatientCreated> context)
        {
            await _unitOfWork.PatientRepository.CreateAsync(_mapper.Map<Patient>(context.Message), CancellationToken.None);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
