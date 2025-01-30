using AutoMapper;
using Events.Doctor;
using MassTransit;
using Services.Domain.Entities;
using Services.Domain.Repositories.Abstractions;

namespace Services.Consumers.Consumers.DoctorConsumers
{
    public class UpdateDoctorConsumer(IDoctorRepository doctorRepository, IMapper mapper) : IConsumer<DoctorUpdated>
    {
        private readonly IDoctorRepository _doctorRepository = doctorRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<DoctorUpdated> context)
        {
            await _doctorRepository.UpdateAsync(_mapper.Map<Doctor>(context.Message), CancellationToken.None);
        }
    }
}
