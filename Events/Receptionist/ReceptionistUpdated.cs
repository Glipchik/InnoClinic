namespace Events.Receptionist
{
    public class ReceptionistUpdated
    {
        public required Guid Id { get; set; }
        public required string FirstName { get; set; } = null!;
        public required string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public required Guid AccountId { get; set; }
        public required Guid OfficeId { get; set; }
    }
}
