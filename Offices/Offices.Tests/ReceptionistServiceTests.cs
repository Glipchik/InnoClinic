using System;
using System.Collections.Generic;
using System.Linq;
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

        [Theory]
        [AutoData]
        public async Task Create_ShouldBeSuccess(CreateReceptionistModel createReceptionistModel)
        {
            // Arrange
            var office = CreateOffice(Id: createReceptionistModel.OfficeId, IsActive: true);

            _officeRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), CancellationToken.None))
                                 .ReturnsAsync(office);

            // Act and Assert
            await Should.NotThrowAsync(async () =>
            {
                await _receptionistService.Create(createReceptionistModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async Task Create_RelatedOfficeNotActive_ShouldBeException(CreateReceptionistModel createReceptionistModel)
        {
            // Arrange
            var office = CreateOffice(Id: createReceptionistModel.OfficeId, IsActive: false);

            _officeRepositoryMock.Setup(repo => repo.GetAsync(createReceptionistModel.OfficeId, CancellationToken.None))
                                 .ReturnsAsync(office);

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _receptionistService.Create(createReceptionistModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async Task Create_RelatedOfficeNotFound_ShouldBeException(CreateReceptionistModel createReceptionistModel)
        {
            // Arrange
            _officeRepositoryMock.Setup(repo => repo.GetAsync(It.Is<Guid>(id => id == createReceptionistModel.OfficeId), CancellationToken.None))
                                 .ReturnsAsync((Office?)null);

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _receptionistService.Create(createReceptionistModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async void Update_ShouldBeSuccess(UpdateReceptionistModel updateReceptionistModel)
        {
            // Arrange
            _officeRepositoryMock
                .Setup(repo => repo.GetAsync(It.Is<Guid>(id => id == updateReceptionistModel.OfficeId), CancellationToken.None))
                .ReturnsAsync(CreateOffice(Id: updateReceptionistModel.OfficeId, IsActive: true));

            _receptionistRepositoryMock.Setup(repo => repo.GetAsync(It.Is<Guid>(id => id == updateReceptionistModel.Id), CancellationToken.None))
                              .ReturnsAsync(CreateReceptionist(Id: updateReceptionistModel.Id, OfficeId: updateReceptionistModel.OfficeId));
            // Act and Assert
            await Should.NotThrowAsync(async () =>
            {
                await _receptionistService.Update(updateReceptionistModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async Task Update_RelatedOfficeNotFound_ShouldBeException(UpdateReceptionistModel updateReceptionistModel)
        {
            // Arrange
            var receptionist = CreateReceptionist(Id: updateReceptionistModel.Id, OfficeId: updateReceptionistModel.OfficeId);

            _officeRepositoryMock.Setup(repo => repo.GetAsync(updateReceptionistModel.OfficeId, CancellationToken.None))
                                 .ReturnsAsync((Office?)null);
            _receptionistRepositoryMock.Setup(repo => repo.GetAsync(updateReceptionistModel.Id, CancellationToken.None))
                                       .ReturnsAsync(receptionist);

            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () => 
            {
                await _receptionistService.Update(updateReceptionistModel, CancellationToken.None);
            });
        }

        [Theory]
        [AutoData]
        public async Task Update_RelatedOfficeIsNotActive_ShouldBeException(UpdateReceptionistModel updateReceptionistModel)
        {
            // Arrange
            var office = CreateOffice(Id: updateReceptionistModel.OfficeId, IsActive: false);

            var receptionist = CreateReceptionist(Id: updateReceptionistModel.Id, OfficeId: updateReceptionistModel.OfficeId);

            _officeRepositoryMock.Setup(repo => repo.GetAsync(updateReceptionistModel.OfficeId, CancellationToken.None))
                                 .ReturnsAsync(office);

            _receptionistRepositoryMock.Setup(repo => repo.GetAsync(updateReceptionistModel.Id, CancellationToken.None))
                                       .ReturnsAsync(receptionist);

            // Act and Assert
            await Should.ThrowAsync<RelatedObjectNotFoundException>(async () =>
            {
                await _receptionistService.Update(updateReceptionistModel, CancellationToken.None);
            });
        }


        private Office CreateOffice(Guid? Id = null, bool IsActive = true)
        {
            return _fixture.Build<Office>()
                .With(x => x.Id, Id ?? Guid.NewGuid())
                .With(x => x.IsActive, IsActive)
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
