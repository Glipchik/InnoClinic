using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using MongoDB.Driver.Core.Misc;
using Moq;
using Offices.Application.MappingProfiles;
using Offices.Application.Models;
using Offices.Application.Services;
using Offices.Application.Services.Abstractions;
using Offices.Data.Entities;
using Offices.Data.Repositories.Abstractions;
using Offices.Domain.Exceptions;
using Shouldly;

namespace Offices.Tests
{
    public class OfficeServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IOfficeRepository> _officeRepositoryMock;
        private readonly Mock<IReceptionistRepository> _receptionistRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly OfficeService _officeService;

        public OfficeServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _doctorRepositoryMock = _fixture.Freeze<Mock<IDoctorRepository>>();
            _officeRepositoryMock = _fixture.Freeze<Mock<IOfficeRepository>>();
            _receptionistRepositoryMock = _fixture.Freeze<Mock<IReceptionistRepository>>();
            _officeService = new OfficeService(_officeRepositoryMock.Object, _doctorRepositoryMock.Object, _receptionistRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Create_ShouldBeSuccess()
        {
            // Arrange
            var createOfficeModel = _fixture.Build<CreateOfficeModel>()
                .With(x => x.Address, _fixture.Create<string>())
                .With(x => x.PhotoURL, _fixture.Create<string>())
                .With(x => x.RegistryPhoneNumber, _fixture.Create<string>())
                .With(x => x.IsActive, true)
                .Create();

            // Act and Assert
            await Should.NotThrowAsync(async () =>
            {
                await _officeService.Create(createOfficeModel, CancellationToken.None);
            });
        }

        [Fact]
        public async Task Delete_ThereAreDoctors_ShouldBeException()
        {
            // Arrange
            var officeId = "12312312312";

            _doctorRepositoryMock.Setup(repo => repo.GetActiveDoctorsFromOffice(officeId, CancellationToken.None))
                .ReturnsAsync(new List<Doctor>
                {
                    CreateDoctor(OfficeId: officeId, Status: "Active")
                });

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectFoundException>(async () =>
            {
                await _officeService.Delete(officeId, CancellationToken.None);
            });
        }

        [Fact]
        public async Task Delete_ThereAreReceptionists_ShouldBeException()
        {
            // Arrange
            var officeId = "12312312312";

            _receptionistRepositoryMock.Setup(repo => repo.GetActiveReceptionistsFromOffice(officeId, CancellationToken.None))
                .ReturnsAsync(new List<Receptionist>
                {
                    CreateReceptionist(OfficeId: officeId)
                });

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectFoundException>(async () =>
            {
                await _officeService.Delete(officeId, CancellationToken.None);
            });
        }

        [Fact]
        public async void DeleteOffice_ShouldBe_Success()
        {
            // Arrange
            var officeId = "21312312";
            _doctorRepositoryMock.Setup(repo => repo.GetActiveDoctorsFromOffice(officeId, CancellationToken.None)).
                ReturnsAsync(new List<Doctor>());
            _receptionistRepositoryMock.Setup(repo => repo.GetActiveReceptionistsFromOffice(officeId, CancellationToken.None)).
                ReturnsAsync(new List<Receptionist>());

            // Act and Assert
            await Should.NotThrowAsync(async () =>
            {
                await _officeService.Delete(officeId, CancellationToken.None);
            });
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

        private Receptionist CreateReceptionist(string? Id = null, string? FirstName = null, string? LastName = null, string? MiddleName = null, string? OfficeId = null)
        {
            return _fixture.Build<Receptionist>()
                .With(x => x.Id, Id ?? _fixture.Create<string>())
                .With(x => x.FirstName, FirstName ?? _fixture.Create<string>())
                .With(x => x.LastName, LastName ?? _fixture.Create<string>())
                .With(x => x.MiddleName, MiddleName ?? _fixture.Create<string>())
                .With(x => x.OfficeId, OfficeId ?? _fixture.Create<string>())
                .Create();
        }
    }
}
