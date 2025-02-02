using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using AutoMapper;
using MongoDB.Driver.Core.Misc;
using Moq;
using Offices.Application.MappingProfiles;
using Offices.Application.Models;
using Offices.Application.Services;
using Offices.Application.Services.Abstractions;
using Offices.Data.Entities;
using Offices.Data.Enums;
using Offices.Data.Repositories.Abstractions;
using Offices.Domain.Exceptions;
using Offices.MessageBroking.Producers.Abstractions;
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
        private readonly Mock<IOfficeProducer> _officeProducerMock;
        private readonly OfficeService _officeService;

        public OfficeServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _doctorRepositoryMock = _fixture.Freeze<Mock<IDoctorRepository>>();
            _officeRepositoryMock = _fixture.Freeze<Mock<IOfficeRepository>>();
            _receptionistRepositoryMock = _fixture.Freeze<Mock<IReceptionistRepository>>();
            _officeProducerMock = _fixture.Freeze<Mock<IOfficeProducer>>();

            _officeService = new OfficeService(_officeRepositoryMock.Object, _doctorRepositoryMock.Object, _receptionistRepositoryMock.Object, _mapperMock.Object, _officeProducerMock.Object);
        }

        [Fact]
        public async Task Create_ShouldBeSuccess()
        {
            // Arrange
            var createOfficeModel = _fixture.Build<CreateOfficeModel>()
                .With(x => x.IsActive, true)
                .Create();

            // Act and Assert
            await Should.NotThrowAsync(async () =>
            {
                await _officeService.Create(createOfficeModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async Task Delete_ThereAreDoctors_ShouldBeException(Guid officeId)
        {
            // Arrange
            _doctorRepositoryMock.Setup(repo => repo.GetActiveDoctorsFromOffice(officeId, CancellationToken.None))
                .ReturnsAsync(new List<Doctor>
                {
                    CreateDoctor(OfficeId: officeId, Status: DoctorStatus.AtWork)
                });

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectFoundException>(async () =>
            {
                await _officeService.Delete(officeId, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async Task Delete_ThereAreReceptionists_ShouldBeException(Guid officeId)
        {
            // Arrange
            _receptionistRepositoryMock.Setup(repo => repo.GetReceptionistsFromOffice(officeId, CancellationToken.None))
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

        [Theory]
        [AutoData]
        public async void DeleteOffice_ShouldBe_Success(Guid officeId)
        {
            // Arrange
            _doctorRepositoryMock.Setup(repo => repo.GetActiveDoctorsFromOffice(officeId, CancellationToken.None)).
                ReturnsAsync(new List<Doctor>());
            _receptionistRepositoryMock.Setup(repo => repo.GetReceptionistsFromOffice(officeId, CancellationToken.None)).
                ReturnsAsync(new List<Receptionist>());

            // Act and Assert
            await Should.NotThrowAsync(async () =>
            {
                await _officeService.Delete(officeId, CancellationToken.None);
            });
        }

        private Doctor CreateDoctor(Guid? Id = null, Guid? OfficeId = null, DoctorStatus? Status = null)
        {
            return _fixture.Build<Doctor>()
                .With(x => x.Id, Id ?? Guid.NewGuid())
                .With(x => x.OfficeId, OfficeId ?? Guid.NewGuid())
                .With(x => x.Status, Status ?? DoctorStatus.AtWork)
                .Create();
        }

        private Receptionist CreateReceptionist(Guid? Id = null, Guid? OfficeId = null)
        {
            return _fixture.Build<Receptionist>()
                .With(x => x.Id, Id ?? Guid.NewGuid())
                .With(x => x.OfficeId, OfficeId ?? Guid.NewGuid())
                .Create();
        }
    }
}
