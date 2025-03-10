using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using AutoMapper;
using Moq;
using Services.Application.Exceptions;
using Services.Application.Models;
using Services.Application.Services;
using Services.Domain.Entities;
using Services.Domain.Repositories.Abstractions;
using Services.MessageBroking.Producers.Abstractions;
using Shouldly;

namespace Services.Tests
{
    public class ServiceCategoryManagerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IServiceCategoryProducer> _producerMock;
        private readonly ServiceCategoryManager _serviceCategoryManager;

        public ServiceCategoryManagerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _unitOfWorkMock = _fixture.Freeze<Mock<IUnitOfWork>>();
            _producerMock = _fixture.Freeze<Mock<IServiceCategoryProducer>>();

            _serviceCategoryManager = new ServiceCategoryManager(_unitOfWorkMock.Object, _mapperMock.Object, _producerMock.Object);

            _fixture.Customize<decimal>(c => c.FromFactory(() => (decimal)(new Random().NextDouble() * 100)));
        }

        [Theory]
        [AutoData]
        public async void Delete_ServiceCategoryDoesntExist_ShouldBeException(Guid serviceCategoryId)
        {
            // Arrange
            _unitOfWorkMock.Setup(repo => repo.ServiceCategoryRepository.GetAsync(serviceCategoryId, CancellationToken.None))
                .ReturnsAsync((ServiceCategory?)null);

            // Act and Assert
            await Should.ThrowAsync<NotFoundException>(async () =>
            {
                await _serviceCategoryManager.Delete(serviceCategoryId, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Delete_RelatedServiceDoesExist_ShouldBeException(Guid serviceCategoryId)
        {
            // Arrange
            _unitOfWorkMock.Setup(repo => repo.ServiceCategoryRepository.GetAsync(serviceCategoryId, CancellationToken.None))
                .ReturnsAsync(CreateServiceCategory(Id: serviceCategoryId, new List<Service> { CreateService() }));

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectFoundException>(async () =>
            {
                await _serviceCategoryManager.Delete(serviceCategoryId, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Delete_ServiceCategoryDoesExist_ShouldBeSuccess(Guid serviceCategoryId)
        {
            // Arrange
            _unitOfWorkMock.Setup(repo => repo.ServiceCategoryRepository.GetAsync(serviceCategoryId, CancellationToken.None))
                .ReturnsAsync(CreateServiceCategory(Id: serviceCategoryId));

            // Act and Assert
            await Should.NotThrowAsync(async () =>
            {
                await _serviceCategoryManager.Delete(serviceCategoryId, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Update_ServiceCategoryDoesntExist_ShouldBeException(UpdateServiceCategoryModel updateServiceCategoryModel)
        {
            // Arrange
            _unitOfWorkMock.Setup(repo => repo.ServiceCategoryRepository.GetAsync(updateServiceCategoryModel.Id, CancellationToken.None))
                .ReturnsAsync((ServiceCategory?)null);

            // Act and Assert
            await Should.ThrowAsync<NotFoundException>(async () =>
            {
                await _serviceCategoryManager.Update(updateServiceCategoryModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Update_ShouldBeSuccess(UpdateServiceCategoryModel updateServiceCategoryModel)
        {
            // Arrange
            _unitOfWorkMock.Setup(repo => repo.ServiceCategoryRepository.GetAsync(updateServiceCategoryModel.Id, CancellationToken.None))
               .ReturnsAsync(CreateServiceCategory(Id: updateServiceCategoryModel.Id));

            // Act and Assert
            await Should.NotThrowAsync(async () =>
            {
                await _serviceCategoryManager.Update(updateServiceCategoryModel, CancellationToken.None);
            });
        }

        private ServiceCategory CreateServiceCategory(Guid? Id = null, ICollection<Service>? Services = null)
        {
            return _fixture.Build<ServiceCategory>()
                .With(x => x.Id, Guid.NewGuid())
                .With(x => x.Services, Services ?? [])
                .Create();
        }

        private Service CreateService(Guid? Id = null, Guid? SpecializationId = null, Guid? ServiceCategoryId = null)
        {
            return _fixture.Build<Service>()
                .With(x => x.Id, Id ?? Guid.NewGuid())
                .With(x => x.Price, (decimal)(new Random().NextDouble() * 100))
                .With(x => x.SpecializationId, SpecializationId ?? Guid.NewGuid())
                .With(x => x.ServiceCategoryId, ServiceCategoryId ?? Guid.NewGuid())
                .Without(x => x.Specialization)
                .Without(x => x.ServiceCategory)
                .Create();
        }
    }
}
