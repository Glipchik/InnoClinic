namespace Services.MessageBroking.Producers.Abstractions
{
    public interface IDoctorProducer
    {
        Task PublishDoctorDeactivated(Guid doctorId, CancellationToken cancellationToken);
    }
}
