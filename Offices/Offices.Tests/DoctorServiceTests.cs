using System.Net;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using AutoMapper;
using MongoDB.Driver.Core.Misc;
using Moq;
using Offices.Application.MappingProfiles;
using Offices.Application.Models;
using Offices.Application.Services;
using Offices.Data.Entities;
using Offices.Data.Repositories.Abstractions;
using Offices.Domain.Exceptions;
using Shouldly;

namespace Offices.Tests
{
    public class DoctorServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IOfficeRepository> _officeRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DoctorService _doctorService;

        public DoctorServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _doctorRepositoryMock = _fixture.Freeze<Mock<IDoctorRepository>>();
            _officeRepositoryMock = _fixture.Freeze<Mock<IOfficeRepository>>();
            _doctorService = new DoctorService(_doctorRepositoryMock.Object, _officeRepositoryMock.Object, _mapperMock.Object);
        }

        [Theory]
        [AutoData]
        public async void Create_RelatedOfficeExists_ShouldBeSuccess(CreateDoctorModel createDoctorModel)
        {
            // Arrange
            var office = CreateOffice(createDoctorModel.OfficeId, IsActive: true);

            _officeRepositoryMock.Setup(repo => repo.GetAsync(createDoctorModel.OfficeId, CancellationToken.None))
                .ReturnsAsync(office);

            var doctorEntity = _fixture.Create<Doctor>();
            _mapperMock.Setup(mapper => mapper.Map<Doctor>(createDoctorModel))
                .Returns(doctorEntity);

            // Act and Assert
            await Should.NotThrowAsync(async () =>
            {
                await _doctorService.Create(createDoctorModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Create_RelatedOfficeNotActive_ShouldBeException(CreateDoctorModel createDoctorModel)
        {
            // Arrange
            var inactiveOffice = CreateOffice(createDoctorModel.OfficeId, IsActive: false);

            _officeRepositoryMock.Setup(repo => repo.GetAsync(createDoctorModel.OfficeId, CancellationToken.None))
                .ReturnsAsync(inactiveOffice);

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _doctorService.Create(createDoctorModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Create_RelatedOfficeNotFound_ShouldBeException(CreateDoctorModel createDoctorModel)
        {
            // Arrange
            _officeRepositoryMock
                .Setup(repo => repo.GetAsync(createDoctorModel.OfficeId, CancellationToken.None))
                .ReturnsAsync((Office?)null);

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _doctorService.Create(createDoctorModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Update_OfficeExists_ShouldBeSuccess(UpdateDoctorModel updateDoctorModel)
        {
            // Arrange
            var existingDoctor = CreateDoctor(Id: updateDoctorModel.Id, OfficeId: updateDoctorModel.OfficeId);

            var activeOffice = CreateOffice(updateDoctorModel.OfficeId, IsActive: true);

            _doctorRepositoryMock
                .Setup(repo => repo.GetAsync(updateDoctorModel.Id, CancellationToken.None))
                .ReturnsAsync(existingDoctor);

            _officeRepositoryMock
                .Setup(repo => repo.GetAsync(updateDoctorModel.OfficeId, CancellationToken.None))
                .ReturnsAsync(activeOffice);

            _mapperMock.Setup(mapper => mapper.Map<Doctor>(updateDoctorModel))
                .Returns(existingDoctor);

            // Act and Assert
            await Should.NotThrowAsync(async () =>
            {
                await _doctorService.Update(updateDoctorModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Update_RelatedOfficeNotFound_ShouldBeException(UpdateDoctorModel updateDoctorModel)
        {
            // Arrange
            var existingDoctor = CreateDoctor(Id: updateDoctorModel.Id, OfficeId: updateDoctorModel.OfficeId);

            _doctorRepositoryMock
                .Setup(repo => repo.GetAsync(updateDoctorModel.Id, CancellationToken.None))
                .ReturnsAsync(existingDoctor);

            _officeRepositoryMock
                .Setup(repo => repo.GetAsync(updateDoctorModel.OfficeId, CancellationToken.None))
                .ReturnsAsync((Office?)null);

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _doctorService.Update(updateDoctorModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Update_RelatedOfficeNotActive_ShouldBeException(UpdateDoctorModel updateDoctorModel)
        {
            // Arrange
            var existingDoctor = CreateDoctor(Id: updateDoctorModel.Id, OfficeId: updateDoctorModel.OfficeId);

            var inactiveOffice = CreateOffice(updateDoctorModel.OfficeId, IsActive: false);

            _doctorRepositoryMock
                .Setup(repo => repo.GetAsync(updateDoctorModel.Id, CancellationToken.None))
                .ReturnsAsync(existingDoctor);

            _officeRepositoryMock
                .Setup(repo => repo.GetAsync(updateDoctorModel.OfficeId, CancellationToken.None))
                .ReturnsAsync(inactiveOffice);

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _doctorService.Update(updateDoctorModel, CancellationToken.None);
            });
        }

        private Office CreateOffice(string? Id = null, bool IsActive = true)
        {
            return _fixture.Build<Office>()
                .With(x => x.Id, Id ?? _fixture.Create<string>())
                .With(x => x.IsActive, IsActive)
                .Create();
        }

        private Doctor CreateDoctor(string? Id = null, string? OfficeId = null)
        {
            return _fixture.Build<Doctor>()
                .With(x => x.Id, Id ?? _fixture.Create<string>())
                .With(x => x.OfficeId, OfficeId ?? _fixture.Create<string>())
                .Create();
        }
    }
}