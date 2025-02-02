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
using Services.Application.Services.Abstractions;
using Services.Domain.Entities;
using Services.Domain.Repositories.Abstractions;
using Services.MessageBroking.Producers.Abstractions;
using Shouldly;

namespace Services.Tests
{

    public class SpecializationServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IServiceProducer> _serviceProducerMock;
        private readonly Mock<IDoctorProducer> _doctorProducerMock;
        private readonly Mock<ISpecializationProducer> _specializationProducerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly SpecializationService _specializationService;

        public SpecializationServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _unitOfWorkMock = _fixture.Freeze<Mock<IUnitOfWork>>();
            _serviceProducerMock = _fixture.Freeze<Mock<IServiceProducer>>();
            _doctorProducerMock = _fixture.Freeze<Mock<IDoctorProducer>>();
            _specializationProducerMock = _fixture.Freeze<Mock<ISpecializationProducer>>();

            _specializationService = new SpecializationService(_unitOfWorkMock.Object, _mapperMock.Object, _serviceProducerMock.Object, _specializationProducerMock.Object, _doctorProducerMock.Object);
        }

        [Theory]
        [AutoData]
        public async void Delete_SpecializationDoesntExist_ShouldBeException(Guid specializationId)
        {
            // Arrange
            _unitOfWorkMock.Setup(repo => repo.SpecializationRepository.GetAsync(specializationId, CancellationToken.None))
                .ReturnsAsync((Specialization?)null);

            // Act and Assert
            await Should.ThrowAsync<NotFoundException>(async () =>
            {
                await _specializationService.Delete(specializationId, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Delete_SpecializationDoesExist_ShouldBeSuccess(Guid specializationId)
        {
            // Arrange
            _unitOfWorkMock.Setup(repo => repo.SpecializationRepository.GetAsync(specializationId, CancellationToken.None))
                .ReturnsAsync(CreateSpecialization(Id: specializationId));

            // Act and Assert
            await Should.NotThrowAsync(async () =>
            {
                await _specializationService.Delete(specializationId, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Update_SpecializationDoesntExist_ShouldBeException(UpdateSpecializationModel updateSpecializationModel)
        {
            // Arrange
            _unitOfWorkMock.Setup(repo => repo.SpecializationRepository.GetAsync(updateSpecializationModel.Id, CancellationToken.None))
                .ReturnsAsync((Specialization?)null);

            // Act and Assert
            await Should.ThrowAsync<NotFoundException>(async () =>
            {
                await _specializationService.Update(updateSpecializationModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Update_SpecializationDoesExist_ShouldBeSuccess(UpdateSpecializationModel updateSpecializationModel)
        {
            // Arrange
            _unitOfWorkMock.Setup(repo => repo.SpecializationRepository.GetAsync(updateSpecializationModel.Id, CancellationToken.None))
                .ReturnsAsync(CreateSpecialization(Id: updateSpecializationModel.Id));

            // Act and Assert
            await Should.NotThrowAsync(async () =>
            {
                await _specializationService.Update(updateSpecializationModel, CancellationToken.None);
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
    }
}
