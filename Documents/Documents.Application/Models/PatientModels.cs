using Documents.Domain.Entities;

namespace Documents.Application.Models
{
    public class PatientModel
    {
        public required Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? MiddleName { get; set; }
        public required Guid AccountId { get; set; }
    }
}
