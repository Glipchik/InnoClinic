using Documents.Domain.Entities;
using Documents.Domain.Repositories.Abstractions;
using AutoMapper;
using Events.Patient;
using Events.ServiceCategory;
using MassTransit;

namespace Documents.MessageBroking.Consumers.ServiceCategoryConsumers
{
    public class DeleteServiceCategoryConsumer(IUnitOfWork unitOfWork) : IConsumer<ServiceCategoryDeleted>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Consume(ConsumeContext<ServiceCategoryDeleted> context)
        {
            await _unitOfWork.ServiceCategoryRepository.DeleteAsync(context.Message.Id, CancellationToken.None);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
