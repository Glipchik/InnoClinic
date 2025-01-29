using AutoMapper;
using Events.Doctor;
using MassTransit;
using Offices.Data.Entities;
using Offices.Data.Repositories.Abstractions;

namespace Offices.Consumers.DoctorConsumers
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
