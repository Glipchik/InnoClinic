namespace Events.Doctor
{
    public class DoctorCreated
    {
        public required Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? MiddleName { get; set; }
        public required DateTime CareerStartYear { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required DoctorStatus Status { get; set; }
        public required Guid SpecializationId { get; set; }
        public required Guid AccountId { get; set; }
        public required Guid OfficeId { get; set; }
    }
}
