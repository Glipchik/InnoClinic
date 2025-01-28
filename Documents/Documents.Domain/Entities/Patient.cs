namespace Documents.Domain.Entities
{
    public class Patient : BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? MiddleName { get; set; }
        public required Account Account { get; set; }
        public required Guid AccountId { get; set; }
    }
}