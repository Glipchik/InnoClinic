using System.Net;
using AutoFixture;
using AutoFixture.AutoMoq;
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

        [Fact]
        public async void Create_RelatedOfficeExists_ShouldBeSuccess()
        {
            // Arrange
            var officeId = "1231231-1231-123";

            var createDoctorModel = _fixture.Build<CreateDoctorModel>()
                .With(d => d.OfficeId, officeId)
                .Create();

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

        [Fact]
        public async void Create_RelatedOfficeNotActive_ShouldBeException()
        {
            // Arrange
            var officeId = "1231231-1231-123";

            var createDoctorModel = _fixture.Build<CreateDoctorModel>()
                .With(d => d.OfficeId, officeId)
                .Create();

            var inactiveOffice = CreateOffice(createDoctorModel.OfficeId, IsActive: false);

            _officeRepositoryMock.Setup(repo => repo.GetAsync(createDoctorModel.OfficeId, CancellationToken.None))
                .ReturnsAsync(inactiveOffice);

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _doctorService.Create(createDoctorModel, CancellationToken.None);
            });
        }

        [Fact]
        public async void Create_RelatedOfficeNotFound_ShouldBeException()
        {
            // Arrange
            var officeId = "1231231-1231-123";

            var createDoctorModel = _fixture.Build<CreateDoctorModel>()
                .With(d => d.OfficeId, officeId)
                .Create();

            _officeRepositoryMock
                .Setup(repo => repo.GetAsync(createDoctorModel.OfficeId, CancellationToken.None))
                .ReturnsAsync((Office?)null);

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _doctorService.Create(createDoctorModel, CancellationToken.None);
            });
        }

        [Fact]
        public async void Update_OfficeExists_ShouldBeSuccess()
        {
            // Arrange
            var doctorId = "1231231-1231-123";
            var officeId = "1231231-1231-124";

            var updateDoctorModel = _fixture.Build<UpdateDoctorModel>()
                .With(d => d.Id, doctorId)
                .With(d => d.OfficeId, officeId)
                .Create();

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

        [Fact]
        public async void Update_RelatedOfficeNotFound_ShouldBeException()
        {
            // Arrange
            var doctorId = "1231231-1231-123";
            var officeId = "1231231-1231-124";

            var updateDoctorModel = _fixture.Build<UpdateDoctorModel>()
                .With(d => d.Id, doctorId)
                .With(d => d.OfficeId, officeId)
                .Create();

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

        [Fact]
        public async void Update_RelatedOfficeNotActive_ShouldBeException()
        {
            // Arrange
            var doctorId = "1231231-1231-123";
            var officeId = "1231231-1231-124";

            var updateDoctorModel = _fixture.Build<UpdateDoctorModel>()
                .With(d => d.Id, doctorId)
                .With(d => d.OfficeId, officeId)
                .Create();

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

        private Office CreateOffice(string? Id = null, string? Address = null, string? PhotoURL = null, string? RegistryPhoneNumber = null, bool? IsActive = null)
        {
            return _fixture.Build<Office>()
                .With(x => x.Id, Id ?? _fixture.Create<string>())
                .With(x => x.Address, Address ?? _fixture.Create<string>())
                .With(x => x.PhotoURL, PhotoURL ?? _fixture.Create<string>())
                .With(x => x.RegistryPhoneNumber, RegistryPhoneNumber ?? _fixture.Create<string>())
                .With(x => x.IsActive, IsActive ?? _fixture.Create<bool>())
                .Create();
        }

        private Doctor CreateDoctor(string? Id = null, string? FirstName = null, string? LastName = null, string? MiddleName = null, string? OfficeId = null, string? Status = null)
        {
            return _fixture.Build<Doctor>()
                .With(x => x.Id, Id ?? _fixture.Create<string>())
                .With(x => x.FirstName, FirstName ?? _fixture.Create<string>())
                .With(x => x.LastName, LastName ?? _fixture.Create<string>())
                .With(x => x.MiddleName, MiddleName ?? _fixture.Create<string>())
                .With(x => x.OfficeId, OfficeId ?? _fixture.Create<string>())
                .With(x => x.Status, Status ?? _fixture.Create<string>())
                .Create();
        }
    }
}