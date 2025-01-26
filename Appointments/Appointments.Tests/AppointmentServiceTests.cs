using Appointments.Application.Models;
using Appointments.Application.Services;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Appointments.Domain.Repositories.Abstractions;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using AutoMapper;
using Moq;
using Shouldly;

namespace Appointments.Tests
{
    public class AppointmentServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AppointmentService _appointmentService;

        public AppointmentServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _mapperMock = _fixture.Freeze<Mock<IMapper>>();

            _unitOfWorkMock = _fixture.Freeze<Mock<IUnitOfWork>>();

            _appointmentService = new AppointmentService(_unitOfWorkMock.Object, _mapperMock.Object);

            _fixture.Customize<DateOnly>(o => o.FromFactory(() => DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1))));
        }

        [Theory]
        [AutoData]
        public async void GetDoctorsSchedule_NoAppointments_ShouldBe36Slots()
        //public async void GetDoctorsSchedule_NoAppointments_ShouldBe36Slots(CreateAppointmentModel createAppointmentModel)
        {
            // Arrange
            var createAppointmentModel = _fixture.Create<CreateAppointmentModel>();

            var serviceCategory = CreateServiceCategory(null, 10);

            var specialization = CreateSpecialization();

            var service = CreateService(createAppointmentModel.ServiceId, serviceCategory.Id, specialization.Id, true);

            var doctor = CreateDoctor(null, null, DoctorStatus.AtWork);

            var patient = CreatePatient();

            _unitOfWorkMock.Setup(repo => repo.ServiceCategoryRepository.GetAsync(serviceCategory.Id, CancellationToken.None))
                .ReturnsAsync(serviceCategory);

            _unitOfWorkMock.Setup(repo => repo.SpecializationRepository.GetAsync(specialization.Id, CancellationToken.None))
                .ReturnsAsync(specialization);

            _unitOfWorkMock.Setup(repo => repo.ServiceRepository.GetAsync(service.Id, CancellationToken.None))
               .ReturnsAsync(service);

            _unitOfWorkMock.Setup(repo => repo.DoctorRepository.GetAsync(doctor.Id, CancellationToken.None))
               .ReturnsAsync(doctor);

            _unitOfWorkMock.Setup(repo => repo.PatientRepository.GetAsync(patient.Id, CancellationToken.None))
               .ReturnsAsync(patient);

            _unitOfWorkMock.Setup(repo => repo.AppointmentRepository.GetAllApprovedByDoctorIdAsync(doctor.Id, createAppointmentModel.Date, CancellationToken.None))
               .ReturnsAsync([]);

            // Act
            var schedule = await _appointmentService.GetDoctorsSchedule(doctor.Id, createAppointmentModel.Date, CancellationToken.None);

            //Arrange
            Should.Equals(schedule.Count(), 36);
        }

        [Theory]
        [AutoData]
        public async void Create_NoAppointments_ShouldBeSuccess()
        //public async void Create_NoAppointments_ShouldBeSuccess(CreateAppointmentModel createAppointmentModel)
        {
            // Arrange
            var createAppointmentModel = _fixture.Create<CreateAppointmentModel>();

            var serviceCategory = CreateServiceCategory(null, 30);

            var specialization = CreateSpecialization();

            var service = CreateService(createAppointmentModel.ServiceId, serviceCategory.Id, specialization.Id, true);
            service.ServiceCategory = serviceCategory;

            var doctor = CreateDoctor(null, null, DoctorStatus.AtWork);

            var patient = CreatePatient();

            _unitOfWorkMock.Setup(repo => repo.ServiceCategoryRepository.GetAsync(serviceCategory.Id, CancellationToken.None))
                .ReturnsAsync(serviceCategory);

            _unitOfWorkMock.Setup(repo => repo.SpecializationRepository.GetAsync(specialization.Id, CancellationToken.None))
                .ReturnsAsync(specialization);

            _unitOfWorkMock.Setup(repo => repo.ServiceRepository.GetAsync(service.Id, CancellationToken.None))
               .ReturnsAsync(service);

            _unitOfWorkMock.Setup(repo => repo.DoctorRepository.GetAsync(doctor.Id, CancellationToken.None))
               .ReturnsAsync(doctor);

            _unitOfWorkMock.Setup(repo => repo.PatientRepository.GetAsync(patient.Id, CancellationToken.None))
               .ReturnsAsync(patient);

            _unitOfWorkMock.Setup(repo => repo.AppointmentRepository.GetAllApprovedByDoctorIdAsync(doctor.Id, createAppointmentModel.Date, CancellationToken.None))
               .ReturnsAsync([]);

            // Act and Assert
            var schedule = await _appointmentService.GetDoctorsSchedule(doctor.Id, createAppointmentModel.Date, CancellationToken.None);
            createAppointmentModel.TimeSlotId = schedule.ElementAt((int)Math.Ceiling((double)(schedule.Count() / 2))).Id;
            await Should.NotThrowAsync(async () =>
            {
                await _appointmentService.Create(createAppointmentModel, CancellationToken.None);
            });
        }

        private ServiceCategory CreateServiceCategory(Guid? Id = null, int? TimeSlotSize = null)
        {
            return _fixture.Build<ServiceCategory>()
                .With(sc => sc.TimeSlotSize, TimeSpan.FromMinutes(TimeSlotSize ?? 10))
                .With(sc => sc.Id, Guid.NewGuid())
                .Create();
        }

        private Specialization CreateSpecialization(Guid? Id = null)
        {
            return _fixture.Build<Specialization>()
                .With(sc => sc.Id, Guid.NewGuid())
                .Create();
        }

        private Service CreateService(Guid? Id = null, Guid? ServiceCategoryId = null, Guid? SpecializationId = null, bool IsActive = true)
        {
            return _fixture.Build<Service>()
                .Without(s => s.Specialization)
                .With(s => s.ServiceCategoryId, ServiceCategoryId ?? Guid.NewGuid())
                .With(s => s.Id, Guid.NewGuid())
                .With(s => s.IsActive, IsActive)
                .Create();
        }

        private Doctor CreateDoctor(Guid? Id = null, Guid? SpecializationId = null, DoctorStatus? Status = null)
        {
            return _fixture.Build<Doctor>()
                .With(d => d.Id, Id ?? Guid.NewGuid())
                .With(d => d.SpecializationId, SpecializationId ?? Guid.NewGuid())
                .With(d => d.Status, Status ?? DoctorStatus.AtWork)
                .Without(d => d.Specialization)
                .Create();
        }

        private Patient CreatePatient(Guid? Id = null)
        {
            return _fixture.Build<Patient>()
                .With(x => x.Id, Guid.NewGuid())
                .Create();
        }

        public class DateOnlyFixtureCustomization : ICustomization
        {

            void ICustomization.Customize(IFixture fixture)
            {
                fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));
            }
        }
    }
}