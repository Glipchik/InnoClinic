using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Moq;
using Services.Application.Exceptions;
using Services.Application.Extensions;
using Services.Application.Mapper;
using Services.Application.Models;
using Services.Application.Models.Enums;
using Services.Application.Services;
using Services.Domain.Entities;
using Services.Domain.Enums;
using Services.Domain.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Xunit;

namespace Services.Tests
{
    public class DoctorServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;
        private readonly DoctorService _doctorService;

        public DoctorServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _unitOfWorkMock = _fixture.Freeze<Mock<IUnitOfWork>>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ApplicationMapping>();
            });

            _mapper = configuration.CreateMapper();
            _doctorService = new DoctorService(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task CreateDoctor_ShouldBeSuccess()
        {
            // Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var createModel = _fixture.Build<CreateDoctorModel>()
                .With(d => d.Status, DoctorStatusModel.AtWork)
                .Create();
            var specialization = _fixture.Build<Specialization>()
                .With(s => s.Id, createModel.SpecializationId)
                .With(s => s.Doctors, [])
                .With(s => s.Services, [])
                .With(s => s.IsActive, true)
                .Create();

            _unitOfWorkMock.Setup(u => u.SpecializationRepository.GetAsync(createModel.SpecializationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(specialization);

            // Act
            var exception = await Record.ExceptionAsync(async () =>
            {
                await _doctorService.Create(createModel, CancellationToken.None);
            });

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task CreateDoctor_WhenSpecializationIsNotFound_ShouldBeException()
        {
            // Arrange
            // Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var createModel = _fixture.Build<CreateDoctorModel>()
                .With(d => d.Status, DoctorStatusModel.AtWork)
                .Create();

            _unitOfWorkMock.Setup(u => u.SpecializationRepository.GetAsync(createModel.SpecializationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Specialization)null);

            // Act & Assert
            await Assert.ThrowsAsync<RelatedObjectNotFoundException>(() => _doctorService.Create(createModel, CancellationToken.None));
        }

        [Fact]
        public async Task Delete_ShouldDeleteDoctor_WhenDoctorExists()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var doctor = _fixture.Build<Doctor>()
                .With(d => d.Id, doctorId)
                .Create();

            _unitOfWorkMock.Setup(u => u.DoctorRepository.GetAsync(doctorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            // Act
            await _doctorService.Delete(doctorId, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(u => u.DoctorRepository.DeleteAsync(doctorId, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldThrowException_WhenDoctorNotFound()
        {
            // Arrange
            var doctorId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.DoctorRepository.GetAsync(doctorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _doctorService.Delete(doctorId, CancellationToken.None));
        }

        [Fact]
        public async Task Get_ShouldReturnDoctorModel_WhenDoctorExists()
        {
            // Arrange
            var doctor = _fixture.Create<Doctor>();
            var doctorModel = _fixture.Create<DoctorModel>();

            _unitOfWorkMock.Setup(u => u.DoctorRepository.GetAsync(doctor.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            //_mapperMock.Setup(m => m.Map<DoctorModel>(doctor)).Returns(doctorModel);

            // Act
            var result = await _doctorService.Get(doctor.Id, CancellationToken.None);

            // Assert
            Assert.Equal(doctorModel, result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllDoctors()
        {
            // Arrange
            var doctors = _fixture.CreateMany<Doctor>();
            var doctorModels = _fixture.CreateMany<DoctorModel>();

            _unitOfWorkMock.Setup(u => u.DoctorRepository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctors);

            //_mapperMock.Setup(m => m.Map<IEnumerable<DoctorModel>>(doctors)).Returns(doctorModels);

            // Act
            var result = await _doctorService.GetAll(CancellationToken.None);

            // Assert
            Assert.Equal(doctorModels, result);
        }

        [Fact]
        public async Task Update_ShouldUpdateDoctor_WhenDoctorAndSpecializationExist()
        {
            // Arrange
            var updateModel = _fixture.Create<UpdateDoctorModel>();
            var doctor = _fixture.Build<Doctor>()
                .With(d => d.Id, updateModel.Id)
                .Create();

            var specialization = _fixture.Build<Specialization>()
                .With(s => s.Id, updateModel.SpecializationId)
                .With(s => s.IsActive, true)
                .Create();

            _unitOfWorkMock.Setup(u => u.DoctorRepository.GetAsync(updateModel.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _unitOfWorkMock.Setup(u => u.SpecializationRepository.GetAsync(updateModel.SpecializationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(specialization);

            var updatedDoctor = _fixture.Create<Doctor>();
            //_mapperMock.Setup(m => m.Map(updateModel, doctor)).Returns(updatedDoctor);

            // Act
            await _doctorService.Update(updateModel, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(u => u.DoctorRepository.UpdateAsync(updatedDoctor, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}