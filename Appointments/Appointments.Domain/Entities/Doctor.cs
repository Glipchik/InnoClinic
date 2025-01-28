using Appointments.Domain.Enums;

namespace Appointments.Domain.Entities
{
    public class Doctor : BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? MiddleName { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required DateTime CareerStartYear { get; set; }
        public required Guid AccountId { get; set; }
        public required DoctorStatus Status { get; set; } = DoctorStatus.AtWork;
        public required Guid SpecializationId { get; set; }
        public required Specialization Specialization { get; set; }
    }
}
