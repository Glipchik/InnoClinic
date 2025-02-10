namespace Events.Patient
{
    public class PatientCreated
    {
        public required Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? MiddleName { get; set; }
        public required bool IsLinkedToAccount { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required Guid AccountId { get; set; }
    }
}
