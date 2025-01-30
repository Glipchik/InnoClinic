using AutoMapper;
using Events.Doctor;
using MassTransit;
using Services.Domain.Entities;
using Services.Domain.Repositories.Abstractions;

namespace Services.Consumers.Consumers.DoctorConsumers
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
