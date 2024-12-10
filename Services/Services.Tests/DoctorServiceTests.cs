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
using Services.Infrastructure.Contexts;
using Services.Infrastructure.Repositories;
using Shouldly;

namespace Services.Tests
{
    public class DoctorServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DoctorService _doctorService;

        public DoctorServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _unitOfWorkMock = _fixture.Freeze<Mock<IUnitOfWork>>();

            _doctorService = new DoctorService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Theory]
        [AutoData]
        public async void Create_RelatedSpecializationExists_ShouldBeSuccess(CreateDoctorModel createDoctorModel)
        {
            // Arrange
            var specialization = CreateSpecialization(createDoctorModel.SpecializationId, IsActive: true);

            _unitOfWorkMock.Setup(repo => repo.SpecializationRepository.GetAsync(createDoctorModel.SpecializationId, CancellationToken.None))
                .ReturnsAsync(specialization);

            // Act and Assert
            await Should.NotThrowAsync(async () =>
            {
                await _doctorService.Create(createDoctorModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Create_RelatedSpecializationDoesntExist_ShouldBeException(CreateDoctorModel createDoctorModel)
        {
            // Arrange
            _unitOfWorkMock.Setup(repo => repo.SpecializationRepository.GetAsync(createDoctorModel.SpecializationId, CancellationToken.None))
                .ReturnsAsync((Specialization?) null);

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _doctorService.Create(createDoctorModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Create_RelatedSpecializationIsNotActive_ShouldBeException(CreateDoctorModel createDoctorModel)
        {
            // Arrange
            var specialization = CreateSpecialization(createDoctorModel.SpecializationId, IsActive: false);

            _unitOfWorkMock.Setup(repo => repo.SpecializationRepository.GetAsync(createDoctorModel.SpecializationId, CancellationToken.None))
                .ReturnsAsync(specialization);

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _doctorService.Create(createDoctorModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Delete_DoctorDoesntExist_ShouldBeException(Guid doctorId)
        {
            // Arrange
            _unitOfWorkMock.Setup(repo => repo.DoctorRepository.GetAsync(doctorId, CancellationToken.None))
                .ReturnsAsync((Doctor?)null);

            // Act and Assert
            await Should.ThrowAsync<NotFoundException>(async () =>
            {
                await _doctorService.Delete(doctorId, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Delete_DoctorDoesExist_ShouldBeSuccess(Guid doctorId)
        {
            // Arrange
            _unitOfWorkMock.Setup(repo => repo.DoctorRepository.GetAsync(doctorId, CancellationToken.None))
                .ReturnsAsync(CreateDoctor(Id: doctorId));

            // Act and Assert
            await Should.NotThrowAsync(async () =>
            {
                await _doctorService.Delete(doctorId, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Update_DoctorDoesntExist_ShouldBeException(UpdateDoctorModel updateDoctorModel)
        {
            // Arrange
            _unitOfWorkMock.Setup(repo => repo.DoctorRepository.GetAsync(updateDoctorModel.Id, CancellationToken.None))
                .ReturnsAsync((Doctor?)null);

            // Act and Assert
            await Should.ThrowAsync<NotFoundException>(async () =>
            {
                await _doctorService.Update(updateDoctorModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Update_RelatedSpecializationDoesntExist_ShouldBeException(UpdateDoctorModel updateDoctorModel)
        {
            // Arrange
            _unitOfWorkMock.Setup(repo => repo.DoctorRepository.GetAsync(updateDoctorModel.Id, CancellationToken.None))
                .ReturnsAsync(CreateDoctor(Id: updateDoctorModel.Id, SpecializationId: updateDoctorModel.SpecializationId));

            _unitOfWorkMock.Setup(repo => repo.SpecializationRepository.GetAsync(updateDoctorModel.SpecializationId, CancellationToken.None))
                .ReturnsAsync((Specialization?) null);

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _doctorService.Update(updateDoctorModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Update_RelatedSpecializationIsNotActive_ShouldBeException(UpdateDoctorModel updateDoctorModel)
        {
            // Arrange
            _unitOfWorkMock.Setup(repo => repo.DoctorRepository.GetAsync(updateDoctorModel.Id, CancellationToken.None))
                .ReturnsAsync(CreateDoctor(Id: updateDoctorModel.Id, SpecializationId: updateDoctorModel.SpecializationId));

            var specialization = CreateSpecialization(updateDoctorModel.SpecializationId, IsActive: false);

            _unitOfWorkMock.Setup(repo => repo.SpecializationRepository.GetAsync(updateDoctorModel.SpecializationId, CancellationToken.None))
                .ReturnsAsync(specialization);

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _doctorService.Update(updateDoctorModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Update_ShouldBeSuccess(UpdateDoctorModel updateDoctorModel)
        {
            // Arrange
            _unitOfWorkMock.Setup(repo => repo.DoctorRepository.GetAsync(updateDoctorModel.Id, CancellationToken.None))
                .ReturnsAsync(CreateDoctor(Id: updateDoctorModel.Id, SpecializationId: updateDoctorModel.SpecializationId));

            var specialization = CreateSpecialization(updateDoctorModel.SpecializationId, IsActive: true);

            _unitOfWorkMock.Setup(repo => repo.SpecializationRepository.GetAsync(updateDoctorModel.SpecializationId, CancellationToken.None))
                .ReturnsAsync(specialization);

            // Act and Assert
            await Should.NotThrowAsync(async () =>
            {
                await _doctorService.Update(updateDoctorModel, CancellationToken.None);
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

        private Doctor CreateDoctor(Guid? Id = null, Guid? SpecializationId = null)
        {
            return _fixture.Build<Doctor>()
                .With(x => x.Id, Id ?? Guid.NewGuid())
                .With(x => x.SpecializationId, SpecializationId ?? Guid.NewGuid())
                .Without(x => x.Specialization)
                .Create();
        }
    }
}