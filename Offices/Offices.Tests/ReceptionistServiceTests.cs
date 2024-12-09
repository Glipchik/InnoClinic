using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ReceptionistServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IOfficeRepository> _officeRepositoryMock;
        private readonly Mock<IReceptionistRepository> _receptionistRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ReceptionistService _receptionistService;

        public ReceptionistServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _officeRepositoryMock = _fixture.Freeze<Mock<IOfficeRepository>>();
            _receptionistRepositoryMock = _fixture.Freeze<Mock<IReceptionistRepository>>();
            _receptionistService = new ReceptionistService(_receptionistRepositoryMock.Object, _officeRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Create_ShouldBeSuccess()
        {
            // Arrange
            var officeId = "1231231-1231-123";
            var office = CreateOffice(Id: officeId, IsActive: true);

            _officeRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>(), CancellationToken.None))
                                 .ReturnsAsync(office);

            var createReceptionistModel = _fixture.Build<CreateReceptionistModel>()
                                                  .With(r => r.OfficeId, officeId)
                                                  .Create();

            // Act and Assert
            await Should.NotThrowAsync(async () =>
            {
                await _receptionistService.Create(createReceptionistModel, CancellationToken.None);
            });
        }

        [Fact]
        public async Task Create_RelatedOfficeNotActive_ShouldBeException()
        {
            // Arrange
            var officeId = "1231231-1231-123";
            var office = CreateOffice(Id: officeId, IsActive: false);

            _officeRepositoryMock.Setup(repo => repo.GetAsync(officeId, CancellationToken.None))
                                 .ReturnsAsync(office);

            var createReceptionistModel = _fixture.Build<CreateReceptionistModel>()
                                                  .With(r => r.OfficeId, officeId)
                                                  .Create();

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _receptionistService.Create(createReceptionistModel, CancellationToken.None);
            });
        }

        [Fact]
        public async Task Create_RelatedOfficeNotFound_ShouldBeException()
        {
            // Arrange
            var officeId = "1231231-1231-123";
            _officeRepositoryMock.Setup(repo => repo.GetAsync(It.Is<string>(id => id == officeId), CancellationToken.None))
                                 .ReturnsAsync((Office?)null);

            var createReceptionistModel = _fixture.Build<CreateReceptionistModel>()
                                                  .With(r => r.OfficeId, officeId)
                                                  .Create();

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _receptionistService.Create(createReceptionistModel, CancellationToken.None);
            });
        }

        [Fact]
        public async void Update_ShouldBeSuccess()
        {
            // Arrange
            var officeId = "1231231-1231-124";
            var receptionistId = "1231231-1231-123";

            _officeRepositoryMock
                .Setup(repo => repo.GetAsync(It.Is<string>(id => id == officeId), CancellationToken.None))
                .ReturnsAsync(CreateOffice(Id: officeId, IsActive: true));

            _receptionistRepositoryMock.Setup(repo => repo.GetAsync(It.Is<string>(id => id == receptionistId), CancellationToken.None))
                              .ReturnsAsync(CreateReceptionist(Id: receptionistId, OfficeId: officeId));

            var updateReceptionistModel = _fixture.Build<UpdateReceptionistModel>()
                                                  .With(r => r.Id, receptionistId)
                                                  .With(r => r.OfficeId, officeId)
                                                  .Create();

            // Act and Assert
            await Should.NotThrowAsync(async () =>
            {
                await _receptionistService.Update(updateReceptionistModel, CancellationToken.None);
            });
        }

        [Fact]
        public async Task Update_RelatedOfficeNotFound_ShouldBeException()
        {
            // Arrange
            var officeId = "1231231-1231-124";
            var receptionistId = "1231231-1231-123";

            var receptionist = _fixture.Build<Receptionist>()
                                       .With(r => r.Id, receptionistId)
                                       .With(r => r.OfficeId, officeId)
                                       .With(r => r.FirstName, "John")
                                       .With(r => r.LastName, _fixture.Create<string>())
                                       .With(r => r.MiddleName, _fixture.Create<string>())
                                       .Create();

            _officeRepositoryMock.Setup(repo => repo.GetAsync(officeId, CancellationToken.None))
                                 .ReturnsAsync((Office?)null);
            _receptionistRepositoryMock.Setup(repo => repo.GetAsync(receptionistId, CancellationToken.None))
                                       .ReturnsAsync(receptionist);

            var updateReceptionistModel = _fixture.Build<UpdateReceptionistModel>()
                                                  .With(m => m.Id, receptionistId)
                                                  .With(m => m.OfficeId, officeId)
                                                  .With(m => m.FirstName, "New Joe")
                                                  .Create();

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () => 
            {
                await _receptionistService.Update(updateReceptionistModel, CancellationToken.None);
            });
        }

        [Fact]
        public async Task Update_RelatedOfficeIsNotActive_ShouldBeException()
        {
            // Arrange
            var officeId = "1231231-1231-124";
            var receptionistId = "1231231-1231-123";

            var office = _fixture.Build<Office>()
                                 .With(o => o.Id, officeId)
                                 .With(o => o.IsActive, false)
                                 .Create();

            var receptionist = _fixture.Build<Receptionist>()
                                       .With(r => r.Id, receptionistId)
                                       .With(r => r.OfficeId, officeId)
                                       .With(r => r.FirstName, "John")
                                       .With(r => r.LastName, _fixture.Create<string>())
                                       .With(r => r.MiddleName, _fixture.Create<string>())
                                       .Create();

            _officeRepositoryMock.Setup(repo => repo.GetAsync(officeId, CancellationToken.None))
                                 .ReturnsAsync(office);

            _receptionistRepositoryMock.Setup(repo => repo.GetAsync(receptionistId, CancellationToken.None))
                                       .ReturnsAsync(receptionist);

            var updateReceptionistModel = _fixture.Build<UpdateReceptionistModel>()
                                                  .With(m => m.Id, receptionistId)
                                                  .With(m => m.OfficeId, officeId)
                                                  .With(m => m.FirstName, "New Joe")
                                                  .Create();

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _receptionistService.Update(updateReceptionistModel, CancellationToken.None);
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
