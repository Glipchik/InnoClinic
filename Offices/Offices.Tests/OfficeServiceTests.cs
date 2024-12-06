using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class OfficeServiceTests
    {
        private readonly IMapper _mapper;

        public OfficeServiceTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ApplicationMappingProfile>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public async void GetOfficeById_Returns_Office()
        {
            // Arrange
            var fixture = new Fixture();
            var officeRepositoryMock = new Mock<IOfficeRepository>();
            officeRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>(), CancellationToken.None))
                              .ReturnsAsync(new Office
                              {
                                  Id = "1231231-1231-123",
                                  Address = "New york",
                                  PhotoURL = fixture.Create<string>(),
                                  RegistryPhoneNumber = fixture.Create<string>(),
                                  IsActive = true
                              });

            var doctorRepositoryMock = new Mock<IDoctorRepository>();
            var receptionistRepositoryMock = new Mock<IReceptionistRepository>();

            var officeService = new OfficeService(officeRepositoryMock.Object, doctorRepositoryMock.Object, receptionistRepositoryMock.Object, _mapper);

            // Act
            var officeModel = await officeService.Get("1231231-1231-123", CancellationToken.None);

            // Assert
            Assert.NotNull(officeModel);
            Assert.Equal("New york", officeModel.Address);
        }

        [Fact]
        public async void CreateOffice_ShouldBe_Success()
        {
            // Arrange
            var fixture = new Fixture();
            var officeRepositoryMock = new Mock<IOfficeRepository>();

            var doctorRepositoryMock = new Mock<IDoctorRepository>();
            var receptionistRepositoryMock = new Mock<IReceptionistRepository>();

            var officeService = new OfficeService(officeRepositoryMock.Object, doctorRepositoryMock.Object, receptionistRepositoryMock.Object, _mapper);

            var createOfficeModel = new CreateOfficeModel
            (
                Address: fixture.Create<string>(),
                PhotoURL: fixture.Create<string>(),
                RegistryPhoneNumber: fixture.Create<string>(),
                IsActive: true
            );

            // Act
            var exception = await Record.ExceptionAsync(async () =>
            {
                await officeService.Create(createOfficeModel, CancellationToken.None);
            });

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async void DeleteOffice_WhenThereAreDoctors_ShouldBe_Exception()
        {
            // Arrange
            var officeRepositoryMock = new Mock<IOfficeRepository>();
            var receptionistRepositoryMock = new Mock<IReceptionistRepository>();

            var doctorRepositoryMock = new Mock<IDoctorRepository>();
            doctorRepositoryMock.Setup(repo => repo.GetActiveDoctorsFromOffice("officeId", CancellationToken.None)).
                ReturnsAsync(new List<Doctor>
                {
                    new Doctor
                    {
                        Id = "1231231-1231-123",
                        FirstName = "John",
                        LastName = "Doe",
                        MiddleName = "B",
                        OfficeId = "1231231-1231-123",
                        Status = "Active"
                    }
                });


            var office = new OfficeService(officeRepositoryMock.Object, doctorRepositoryMock.Object, receptionistRepositoryMock.Object, _mapper);

            // Act
            var exception = await Record.ExceptionAsync(async () =>
            {
                await office.Delete("officeId", CancellationToken.None);
            });

            // Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public async void DeleteOffice_WhenThereAreReceptionists_ShouldBe_Exception()
        {
            // Arrange
            var officeRepositoryMock = new Mock<IOfficeRepository>();
            var receptionistRepositoryMock = new Mock<IReceptionistRepository>();
            receptionistRepositoryMock.Setup(repo => repo.GetActiveReceptionistsFromOffice("officeId", CancellationToken.None)).
                ReturnsAsync(new List<Receptionist>
                {
                    new Receptionist
                    {
                        Id = "1231231-1231-123",
                        FirstName = "John",
                        LastName = "Doe",
                        MiddleName = "B",
                        OfficeId = "1231231-1231-123"
                    }
                });

            var doctorRepositoryMock = new Mock<IDoctorRepository>();


            var office = new OfficeService(officeRepositoryMock.Object, doctorRepositoryMock.Object, receptionistRepositoryMock.Object, _mapper);

            // Act
            var exception = await Record.ExceptionAsync(async () =>
            {
                await office.Delete("officeId", CancellationToken.None);
            });

            // Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public async void DeleteOffice_ShouldBe_Success()
        {
            // Arrange
            var officeRepositoryMock = new Mock<IOfficeRepository>();
            var receptionistRepositoryMock = new Mock<IReceptionistRepository>();

            var doctorRepositoryMock = new Mock<IDoctorRepository>();
            doctorRepositoryMock.Setup(repo => repo.GetActiveDoctorsFromOffice("officeId", CancellationToken.None)).
                ReturnsAsync(new List<Doctor>());


            var office = new OfficeService(officeRepositoryMock.Object, doctorRepositoryMock.Object, receptionistRepositoryMock.Object, _mapper);

            // Act
            var exception = await Record.ExceptionAsync(async () =>
            {
                await office.Delete("officeId", CancellationToken.None);
            });

            // Assert
            Assert.Null(exception);
        }
    }
}
