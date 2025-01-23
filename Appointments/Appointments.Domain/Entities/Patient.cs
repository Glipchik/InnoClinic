namespace Appointments.Domain.Entities
{
    public class Patient : BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? MiddleName { get; set; }
        public required DateTime DateOfBirth { get; set; }
    }
}
