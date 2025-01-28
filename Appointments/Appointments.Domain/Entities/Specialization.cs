namespace Appointments.Domain.Entities
{
    public class Specialization : BaseEntity
    {
        public required string SpecializationName { get; set; }
        public bool IsActive { get; set; }
    }
}
