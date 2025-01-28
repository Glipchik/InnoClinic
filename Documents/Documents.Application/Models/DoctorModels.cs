using Documents.Application.Models.Enums;

namespace Documents.Application.Models
{
    public class DoctorModel
    {
        public required Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? MiddleName { get; set; }
        public required Guid AccountId { get; set; }
        public required DoctorStatusModel Status { get; set; } = DoctorStatusModel.AtWork;
        public required Guid SpecializationId { get; set; }
    }
}
