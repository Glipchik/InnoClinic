using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using AutoMapper;
using EmptyFiles;
using Moq;
using Services.Application.Exceptions;
using Services.Application.Models;
using Services.Application.Services;
using Services.Domain.Entities;
using Services.Domain.Repositories.Abstractions;
using Shouldly;

namespace Services.Tests
{
    public class ServiceManagerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ServiceManager _serviceManager;

        public ServiceManagerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _unitOfWorkMock = _fixture.Freeze<Mock<IUnitOfWork>>();

            _serviceManager = new ServiceManager(_unitOfWorkMock.Object, _mapperMock.Object);

            _fixture.Customize<decimal>(c => c.FromFactory(() => (decimal)(new Random().NextDouble() * 100)));
        }

        [Theory]
        [AutoData]
        public async void Create_RelatedSpecializationAndCategoryExist_ShouldBeSuccess(CreateServiceModel createServiceModel)
        {
            // Arrange
            var specialization = CreateSpecialization(createServiceModel.SpecializationId, IsActive: true);
            var category = CreateServiceCategory(createServiceModel.ServiceCategoryId);

            _unitOfWorkMock.Setup(repo => repo.SpecializationRepository.GetAsync(createServiceModel.SpecializationId, CancellationToken.None))
                .ReturnsAsync(specialization);

            _unitOfWorkMock.Setup(repo => repo.ServiceCategoryRepository.GetAsync(createServiceModel.ServiceCategoryId, CancellationToken.None))
                .ReturnsAsync(category);

            // Act and Assert
            await Should.NotThrowAsync(async () =>
            {
                await _serviceManager.Create(createServiceModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Create_RelatedSpecializationDoesntExist_ShouldBeException(CreateServiceModel createServiceModel)
        {
            // Arrange
            var category = CreateServiceCategory(createServiceModel.ServiceCategoryId);

            _unitOfWorkMock.Setup(repo => repo.SpecializationRepository.GetAsync(createServiceModel.SpecializationId, CancellationToken.None))
                .ReturnsAsync((Specialization?)null);

            _unitOfWorkMock.Setup(repo => repo.ServiceCategoryRepository.GetAsync(createServiceModel.ServiceCategoryId, CancellationToken.None))
                .ReturnsAsync(category);

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _serviceManager.Create(createServiceModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Create_RelatedSpecializationIsNotActive_ShouldBeException(CreateServiceModel createServiceModel)
        {
            // Arrange
            var category = CreateServiceCategory(createServiceModel.ServiceCategoryId);
            var specialization = CreateSpecialization(createServiceModel.SpecializationId, IsActive: false);

            _unitOfWorkMock.Setup(repo => repo.SpecializationRepository.GetAsync(createServiceModel.SpecializationId, CancellationToken.None))
                .ReturnsAsync(specialization);

            _unitOfWorkMock.Setup(repo => repo.ServiceCategoryRepository.GetAsync(createServiceModel.ServiceCategoryId, CancellationToken.None))
                .ReturnsAsync(category);

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _serviceManager.Create(createServiceModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Create_RelatedCategoryIsNotFound_ShouldBeException(CreateServiceModel createServiceModel)
        {
            // Arrange
            var specialization = CreateSpecialization(createServiceModel.SpecializationId, IsActive: true);

            _unitOfWorkMock.Setup(repo => repo.SpecializationRepository.GetAsync(createServiceModel.SpecializationId, CancellationToken.None))
                .ReturnsAsync(specialization);

            _unitOfWorkMock.Setup(repo => repo.ServiceCategoryRepository.GetAsync(createServiceModel.ServiceCategoryId, CancellationToken.None))
                .ReturnsAsync((ServiceCategory?) null);

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _serviceManager.Create(createServiceModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Delete_ServiceDoesntExist_ShouldBeException(Guid serviceId)
        {
            // Arrange
            _unitOfWorkMock.Setup(repo => repo.ServiceRepository.GetAsync(serviceId, CancellationToken.None))
                .ReturnsAsync((Service?)null);

            // Act and Assert
            await Should.ThrowAsync<NotFoundException>(async () =>
            {
                await _serviceManager.Delete(serviceId, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Delete_ServiceDoesExist_ShouldBeSuccess(Guid serviceId)
        {
            // Arrange
            _unitOfWorkMock.Setup(repo => repo.ServiceRepository.GetAsync(serviceId, CancellationToken.None))
                .ReturnsAsync(CreateService(Id: serviceId));

            // Act and Assert
            await Should.NotThrowAsync(async () =>
            {
                await _serviceManager.Delete(serviceId, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Update_ServiceDoesntExist_ShouldBeException(UpdateServiceModel updateServiceModel)
        {
            // Arrange
            _unitOfWorkMock.Setup(repo => repo.ServiceRepository.GetAsync(updateServiceModel.Id, CancellationToken.None))
                .ReturnsAsync((Service?)null);

            // Act and Assert
            await Should.ThrowAsync<NotFoundException>(async () =>
            {
                await _serviceManager.Update(updateServiceModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Update_RelatedSpecializationDoesntExist_ShouldBeException(UpdateServiceModel updateServiceModel)
        {
            // Arrange
            var category = CreateServiceCategory(updateServiceModel.ServiceCategoryId);

            _unitOfWorkMock.Setup(repo => repo.SpecializationRepository.GetAsync(updateServiceModel.SpecializationId, CancellationToken.None))
                .ReturnsAsync((Specialization?)null);

            _unitOfWorkMock.Setup(repo => repo.ServiceCategoryRepository.GetAsync(updateServiceModel.ServiceCategoryId, CancellationToken.None))
                .ReturnsAsync(category);

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _serviceManager.Update(updateServiceModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Update_RelatedSpecializationIsNotActive_ShouldBeException(UpdateServiceModel updateServiceModel)
        {
            // Arrange
            var category = CreateServiceCategory(updateServiceModel.ServiceCategoryId);
            var specialization = CreateSpecialization(updateServiceModel.SpecializationId, IsActive: false);

            _unitOfWorkMock.Setup(repo => repo.SpecializationRepository.GetAsync(updateServiceModel.SpecializationId, CancellationToken.None))
                .ReturnsAsync(specialization);

            _unitOfWorkMock.Setup(repo => repo.ServiceCategoryRepository.GetAsync(updateServiceModel.ServiceCategoryId, CancellationToken.None))
                .ReturnsAsync(category);

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _serviceManager.Update(updateServiceModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Update_RelatedCategoryIsNotFound_ShouldBeException(UpdateServiceModel updateServiceModel)
        {
            // Arrange
            var specialization = CreateSpecialization(updateServiceModel.SpecializationId, IsActive: true);

            _unitOfWorkMock.Setup(repo => repo.SpecializationRepository.GetAsync(updateServiceModel.SpecializationId, CancellationToken.None))
                .ReturnsAsync(specialization);

            _unitOfWorkMock.Setup(repo => repo.ServiceCategoryRepository.GetAsync(updateServiceModel.ServiceCategoryId, CancellationToken.None))
                .ReturnsAsync((ServiceCategory?)null);

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _serviceManager.Update(updateServiceModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Update_ShouldBeSuccess(UpdateServiceModel updateServiceModel)
        {
            // Arrange
            _unitOfWorkMock.Setup(repo => repo.DoctorRepository.GetAsync(updateServiceModel.Id, CancellationToken.None))
                .ReturnsAsync(CreateDoctor(Id: updateServiceModel.Id, SpecializationId: updateServiceModel.SpecializationId));

            var specialization = CreateSpecialization(updateServiceModel.SpecializationId, IsActive: true);

            _unitOfWorkMock.Setup(repo => repo.SpecializationRepository.GetAsync(updateServiceModel.SpecializationId, CancellationToken.None))
                .ReturnsAsync(specialization);

            // Act and Assert
            await Should.NotThrowAsync(async () =>
            {
                await _serviceManager.Update(updateServiceModel, CancellationToken.None);
            });
        }

        private Specialization CreateSpecialization(Guid? Id = null, bool IsActive = true)
        {
            return _fixture.Build<Specialization>()
                .Without(x => x.Doctors)
                .Without(x => x.Services)
                .With(x => x.Id, Guid.NewGuid())
                .With(x => x.IsActive, IsActive)
                .Create();
        }

        private ServiceCategory CreateServiceCategory(Guid? Id = null)
        {
            return _fixture.Build<ServiceCategory>()
                .Without(x => x.Services)
                .With(x => x.Id, Guid.NewGuid())
                .Create();
        }

        private Doctor CreateDoctor(Guid? Id = null, Guid? SpecializationId = null)
        {
            return _fixture.Build<Doctor>()
                .With(x => x.Id, Id ?? Guid.NewGuid())
                .With(x => x.SpecializationId, SpecializationId ?? Guid.NewGuid())
                .Without(x => x.Specialization)
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
