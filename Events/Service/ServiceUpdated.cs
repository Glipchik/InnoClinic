namespace Events.Service
{
    public class ServiceUpdated
    {
        public required Guid Id { get; set; }
        public required Guid ServiceCategoryId { get; set; }
        public required string ServiceName { get; set; }
        public required decimal Price { get; set; }
        public required Guid SpecializationId { get; set; }
        public required bool IsActive { get; set; }
    }
}
