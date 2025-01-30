namespace Documents.Domain.Entities
{
    public class Specialization : BaseEntity
    {
        public required string SpecializationName { get; set; }
        public required bool IsActive { get; set; } = true;
    }
}