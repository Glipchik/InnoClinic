using AutoMapper;
using Events.Doctor;
using MassTransit;
using Offices.Data.Entities;
using Offices.Data.Repositories.Abstractions;

namespace Offices.MessageBroking.Consumers.DoctorConsumers
{
    public class CreateDoctorConsumer(IDoctorRepository doctorRepository, IMapper mapper) : IConsumer<DoctorCreated>
    {
        private readonly IDoctorRepository _doctorRepository = doctorRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<DoctorCreated> context)
        {
            await _doctorRepository.CreateAsync(_mapper.Map<Doctor>(context.Message), CancellationToken.None);
        }
    }
}
