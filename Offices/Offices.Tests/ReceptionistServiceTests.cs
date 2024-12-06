using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using MongoDB.Driver.Core.Misc;
using Moq;
using Offices.Application.MappingProfiles;
using Offices.Application.Models;
using Offices.Application.Services;
using Offices.Data.Entities;
using Offices.Data.Repositories.Abstractions;

namespace Offices.Tests
{
    public class ReceptionistServiceTests
    {
        private readonly IMapper _mapper;

        public ReceptionistServiceTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ApplicationMappingProfile>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public async void GetReceptionistById_Returns_Receptionist()
        {
            // Arrange
            var fixture = new Fixture();
            var receptionistRepositoryMock = new Mock<IReceptionistRepository>();
            receptionistRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>(), CancellationToken.None))
                              .ReturnsAsync(new Receptionist
                              {
                                  Id = "1231231-1231-123",
                                  FirstName = "John",
                                  LastName = fixture.Create<string>(),
                                  MiddleName = fixture.Create<string>(),
                                  OfficeId = fixture.Create<string>()
                              });

            var officeRepositoryMock = new Mock<IOfficeRepository>();

            var receptionistService = new ReceptionistService(receptionistRepositoryMock.Object, officeRepositoryMock.Object, _mapper);

            // Act
            var receptionistModel = await receptionistService.Get("1231231-1231-123", CancellationToken.None);

            // Assert
            Assert.NotNull(receptionistModel);
            Assert.Equal("John", receptionistModel.FirstName);
        }

        [Fact]
        public async void CreateReceptionist_ShouldBe_Success()
        {
            // Arrange
            var fixture = new Fixture();
            var officeRepositoryMock = new Mock<IOfficeRepository>();
            officeRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>(), CancellationToken.None))
                              .ReturnsAsync(new Office
                              {
                                  Id = "1231231-1231-123",
                                  Address = fixture.Create<string>(),
                                  PhotoURL = fixture.Create<string>(),
                                  RegistryPhoneNumber = fixture.Create<string>(),
                                  IsActive = true
                              });

            var receptionistRepositoryMock = new Mock<IReceptionistRepository>();

            var receptionistService = new ReceptionistService(receptionistRepositoryMock.Object, officeRepositoryMock.Object, _mapper);

            var createDoctorModel = new CreateReceptionistModel
            (
                FirstName: fixture.Create<string>(),
                LastName: fixture.Create<string>(),
                MiddleName: fixture.Create<string>(),
                OfficeId: "1231231-1231-123"
            );

            // Act
            var exception = await Record.ExceptionAsync(async () =>
            {
                await receptionistService.Create(createDoctorModel, CancellationToken.None);
            });

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async void CreateReceptionist_WhenRelatedOfficeNotActive_ShouldBe_Exception()
        {
            // Arrange
            var fixture = new Fixture();
            var officeRepositoryMock = new Mock<IOfficeRepository>();
            officeRepositoryMock.Setup(repo => repo.GetAsync("1231231-1231-123", CancellationToken.None))
                              .ReturnsAsync(new Office
                              {
                                  Id = "1231231-1231-123",
                                  Address = fixture.Create<string>(),
                                  PhotoURL = fixture.Create<string>(),
                                  RegistryPhoneNumber = fixture.Create<string>(),
                                  IsActive = false
                              });

            var receptionistRepositoryMock = new Mock<IReceptionistRepository>();

            var receptionistService = new ReceptionistService(receptionistRepositoryMock.Object, officeRepositoryMock.Object, _mapper);

            var createReceptionistModel = new CreateReceptionistModel
            (
                FirstName: "John",
                LastName: fixture.Create<string>(),
                MiddleName: fixture.Create<string>(),
                OfficeId: "1231231-1231-123"
            );

            // Act
            var exception = await Record.ExceptionAsync(async () =>
            {
                await receptionistService.Create(createReceptionistModel, CancellationToken.None);
            });

            // Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public async void CreateReceptionist_WhenRelatedOfficeNotFound_ShouldBe_Exception()
        {
            // Arrange
            var fixture = new Fixture();
            var officeRepositoryMock = new Mock<IOfficeRepository>();
            officeRepositoryMock
                .Setup(repo => repo.GetAsync(It.Is<string>(id => id == "1231231-1231-123"), CancellationToken.None))
                .ReturnsAsync((Office?)null);


            var receptionistRepositoryMock = new Mock<IReceptionistRepository>();

            var receptionistService = new ReceptionistService(receptionistRepositoryMock.Object, officeRepositoryMock.Object, _mapper);

            var createReceptionistModel = new CreateReceptionistModel
            (
                FirstName: "John",
                LastName: fixture.Create<string>(),
                MiddleName: fixture.Create<string>(),
                OfficeId: "1231231-1231-123"
            );

            // Act
            var exception = await Record.ExceptionAsync(async () =>
            {
                await receptionistService.Create(createReceptionistModel, CancellationToken.None);
            });

            // Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public async void UpdateReceptionist_ShouldBe_Success()
        {
            // Arrange
            var fixture = new Fixture();
            var officeRepositoryMock = new Mock<IOfficeRepository>();
            officeRepositoryMock
                .Setup(repo => repo.GetAsync(It.Is<string>(id => id == "1231231-1231-124"), CancellationToken.None))
                .ReturnsAsync(new Office
                {
                    Id = "1231231-1231-124",
                    Address = fixture.Create<string>(),
                    PhotoURL = fixture.Create<string>(),
                    RegistryPhoneNumber = fixture.Create<string>(),
                    IsActive = true
                });

            var receptionistRepositoryMock = new Mock<IReceptionistRepository>();
            receptionistRepositoryMock.Setup(repo => repo.GetAsync(It.Is<string>(id => id == "1231231-1231-123"), CancellationToken.None))
                              .ReturnsAsync(new Receptionist
                              {
                                  Id = "1231231-1231-123",
                                  FirstName = "John",
                                  LastName = fixture.Create<string>(),
                                  MiddleName = fixture.Create<string>(),
                                  OfficeId = "1231231-1231-124"
                              });

            var receptionistService = new ReceptionistService(receptionistRepositoryMock.Object, officeRepositoryMock.Object, _mapper);

            var updateReceptionistModel = new UpdateReceptionistModel
            (
                Id: "1231231-1231-123",
                FirstName: "New Joe",
                LastName: fixture.Create<string>(),
                MiddleName: fixture.Create<string>(),
                OfficeId: "1231231-1231-124"
            );

            // Act
            var exception = await Record.ExceptionAsync(async () =>
            {
                await receptionistService.Update(updateReceptionistModel, CancellationToken.None);
            });

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async void UpdateReceptionist_WhenRelatedOfficeNotFound_ShouldBe_Exception()
        {
            // Arrange
            var fixture = new Fixture();
            var officeRepositoryMock = new Mock<IOfficeRepository>();
            officeRepositoryMock
                .Setup(repo => repo.GetAsync(It.Is<string>(id => id == "1231231-1231-124"), CancellationToken.None))
                .ReturnsAsync((Office?)null);

            var receptionistRepositoryMock = new Mock<IReceptionistRepository>();
            receptionistRepositoryMock.Setup(repo => repo.GetAsync(It.Is<string>(id => id == "1231231-1231-123"), CancellationToken.None))
                              .ReturnsAsync(new Receptionist
                              {
                                  Id = "1231231-1231-123",
                                  FirstName = "John",
                                  LastName = fixture.Create<string>(),
                                  MiddleName = fixture.Create<string>(),
                                  OfficeId = "1231231-1231-124"
                              });

            var receptionistService = new ReceptionistService(receptionistRepositoryMock.Object, officeRepositoryMock.Object, _mapper);

            var updateReceptionistModel = new UpdateReceptionistModel
            (
                Id: "1231231-1231-123",
                FirstName: "New Joe",
                LastName: fixture.Create<string>(),
                MiddleName: fixture.Create<string>(),
                OfficeId: "1231231-1231-124"
            );

            // Act
            var exception = await Record.ExceptionAsync(async () =>
            {
                await receptionistService.Update(updateReceptionistModel, CancellationToken.None);
            });

            // Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public async void UpdateReceptionist_WhenRelatedOfficeIsNotActive_ShouldBe_Exception()
        {
            // Arrange
            var fixture = new Fixture();
            var officeRepositoryMock = new Mock<IOfficeRepository>();
            officeRepositoryMock
                .Setup(repo => repo.GetAsync(It.Is<string>(id => id == "1231231-1231-124"), CancellationToken.None))
                .ReturnsAsync(new Office
                {
                    Id = "1231231-1231-124",
                    Address = fixture.Create<string>(),
                    PhotoURL = fixture.Create<string>(),
                    RegistryPhoneNumber = fixture.Create<string>(),
                    IsActive = false
                });

            var receptionistRepositoryMock = new Mock<IReceptionistRepository>();
            receptionistRepositoryMock.Setup(repo => repo.GetAsync(It.Is<string>(id => id == "1231231-1231-123"), CancellationToken.None))
                              .ReturnsAsync(new Receptionist
                              {
                                  Id = "1231231-1231-123",
                                  FirstName = "John",
                                  LastName = fixture.Create<string>(),
                                  MiddleName = fixture.Create<string>(),
                                  OfficeId = "1231231-1231-124"
                              });

            var receptionistService = new ReceptionistService(receptionistRepositoryMock.Object, officeRepositoryMock.Object, _mapper);

            var updateReceptionistModel = new UpdateReceptionistModel
            (
                Id: "1231231-1231-123",
                FirstName: "New Joe",
                LastName: fixture.Create<string>(),
                MiddleName: fixture.Create<string>(),
                OfficeId: "1231231-1231-124"
            );

            // Act
            var exception = await Record.ExceptionAsync(async () =>
            {
                await receptionistService.Update(updateReceptionistModel, CancellationToken.None);
            });

            // Assert
            Assert.NotNull(exception);
        }
    }
}
