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
    public class DoctorServiceTests
    {
        private readonly IMapper _mapper;

        public DoctorServiceTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ApplicationMappingProfile>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public async void GetDoctorById_Returns_Doctor()
        {
            // Arrange
            var fixture = new Fixture();
            var doctorRepositoryMock = new Mock<IDoctorRepository>();
            doctorRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>(), CancellationToken.None))
                              .ReturnsAsync(new Doctor
                              {
                                  Id = "1231231-1231-123",
                                  FirstName = "John",
                                  LastName = fixture.Create<string>(),
                                  MiddleName = fixture.Create<string>(),
                                  OfficeId = fixture.Create<string>(),
                                  Status = fixture.Create<string>()
                              });

            var officeRepositoryMock = new Mock<IOfficeRepository>();

            var doctorService = new DoctorService(doctorRepositoryMock.Object, officeRepositoryMock.Object, _mapper);

            // Act
            var doctorModel = await doctorService.Get("1231231-1231-123", CancellationToken.None);

            // Assert
            Assert.NotNull(doctorModel);
            Assert.Equal("John", doctorModel.FirstName);
        }

        [Fact]
        public async void CreateDoctor_ShouldBe_Success()
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

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            var doctorService = new DoctorService(doctorRepositoryMock.Object, officeRepositoryMock.Object, _mapper);

            var createDoctorModel = new CreateDoctorModel
            (
                FirstName: fixture.Create<string>(),
                LastName: fixture.Create<string>(),
                MiddleName: fixture.Create<string>(),
                OfficeId: "1231231-1231-123",
                Status: fixture.Create<string>()
            );

            // Act
            var exception = await Record.ExceptionAsync(async () =>
            {
                await doctorService.Create(createDoctorModel, CancellationToken.None);
            });

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async void CreateDoctor_WhenRelatedOfficeNotActive_ShouldBe_Exception()
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

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            var doctorService = new DoctorService(doctorRepositoryMock.Object, officeRepositoryMock.Object, _mapper);

            var createDoctorModel = new CreateDoctorModel
            (
                FirstName: "John",
                LastName: fixture.Create<string>(),
                MiddleName: fixture.Create<string>(),
                OfficeId: "1231231-1231-123",
                Status: fixture.Create<string>()
            );

            // Act
            var exception = await Record.ExceptionAsync(async () =>
            {
                await doctorService.Create(createDoctorModel, CancellationToken.None);
            });

            // Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public async void CreateDoctor_WhenRelatedOfficeNotFound_ShouldBe_Exception()
        {
            // Arrange
            var fixture = new Fixture();
            var officeRepositoryMock = new Mock<IOfficeRepository>();
            officeRepositoryMock
                .Setup(repo => repo.GetAsync(It.Is<string>(id => id == "1231231-1231-123"), CancellationToken.None))
                .ReturnsAsync((Office?)null);


            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            var doctorService = new DoctorService(doctorRepositoryMock.Object, officeRepositoryMock.Object, _mapper);

            var createDoctorModel = new CreateDoctorModel
            (
                FirstName: "John",
                LastName: fixture.Create<string>(),
                MiddleName: fixture.Create<string>(),
                OfficeId: "1231231-1231-123",
                Status: fixture.Create<string>()
            );

            // Act
            var exception = await Record.ExceptionAsync(async () =>
            {
                await doctorService.Create(createDoctorModel, CancellationToken.None);
            });

            // Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public async void UpdateDoctor_ShouldBe_Success()
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

            var doctorRepositoryMock = new Mock<IDoctorRepository>();
            doctorRepositoryMock.Setup(repo => repo.GetAsync(It.Is<string>(id => id == "1231231-1231-123"), CancellationToken.None))
                              .ReturnsAsync(new Doctor
                              {
                                  Id = "1231231-1231-123",
                                  FirstName = "John",
                                  LastName = fixture.Create<string>(),
                                  MiddleName = fixture.Create<string>(),
                                  OfficeId = "1231231-1231-124",
                                  Status = fixture.Create<string>()
                              });

            var doctorService = new DoctorService(doctorRepositoryMock.Object, officeRepositoryMock.Object, _mapper);

            var updateDoctorModel = new UpdateDoctorModel
            (
                Id: "1231231-1231-123",
                FirstName: "New Joe",
                LastName: fixture.Create<string>(),
                MiddleName: fixture.Create<string>(),
                OfficeId: "1231231-1231-124",
                Status: fixture.Create<string>()
            );

            // Act
            var exception = await Record.ExceptionAsync(async () =>
            {
                await doctorService.Update(updateDoctorModel, CancellationToken.None);
            });

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async void UpdateDoctor_WhenRelatedOfficeNotFound_ShouldBe_Exception()
        {
            // Arrange
            var fixture = new Fixture();
            var officeRepositoryMock = new Mock<IOfficeRepository>();
            officeRepositoryMock
                .Setup(repo => repo.GetAsync(It.Is<string>(id => id == "1231231-1231-124"), CancellationToken.None))
                .ReturnsAsync((Office?)null);

            var doctorRepositoryMock = new Mock<IDoctorRepository>();
            doctorRepositoryMock.Setup(repo => repo.GetAsync(It.Is<string>(id => id == "1231231-1231-123"), CancellationToken.None))
                              .ReturnsAsync(new Doctor
                              {
                                  Id = "1231231-1231-123",
                                  FirstName = "John",
                                  LastName = fixture.Create<string>(),
                                  MiddleName = fixture.Create<string>(),
                                  OfficeId = "1231231-1231-124",
                                  Status = fixture.Create<string>()
                              });

            var doctorService = new DoctorService(doctorRepositoryMock.Object, officeRepositoryMock.Object, _mapper);

            var updateDoctorModel = new UpdateDoctorModel
            (
                Id: "1231231-1231-123",
                FirstName: "New Joe",
                LastName: fixture.Create<string>(),
                MiddleName: fixture.Create<string>(),
                OfficeId: "1231231-1231-124",
                Status: fixture.Create<string>()
            );

            // Act
            var exception = await Record.ExceptionAsync(async () =>
            {
                await doctorService.Update(updateDoctorModel, CancellationToken.None);
            });

            // Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public async void UpdateDoctor_WhenRelatedOfficeIsNotActive_ShouldBe_Exception()
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

            var doctorRepositoryMock = new Mock<IDoctorRepository>();
            doctorRepositoryMock.Setup(repo => repo.GetAsync(It.Is<string>(id => id == "1231231-1231-123"), CancellationToken.None))
                              .ReturnsAsync(new Doctor
                              {
                                  Id = "1231231-1231-123",
                                  FirstName = "John",
                                  LastName = fixture.Create<string>(),
                                  MiddleName = fixture.Create<string>(),
                                  OfficeId = "1231231-1231-124",
                                  Status = fixture.Create<string>()
                              });

            var doctorService = new DoctorService(doctorRepositoryMock.Object, officeRepositoryMock.Object, _mapper);

            var updateDoctorModel = new UpdateDoctorModel
            (
                Id: "1231231-1231-123",
                FirstName: "New Joe",
                LastName: fixture.Create<string>(),
                MiddleName: fixture.Create<string>(),
                OfficeId: "1231231-1231-124",
                Status: fixture.Create<string>()
            );

            // Act
            var exception = await Record.ExceptionAsync(async () =>
            {
                await doctorService.Update(updateDoctorModel, CancellationToken.None);
            });

            // Assert
            Assert.NotNull(exception);
        }
    }
}