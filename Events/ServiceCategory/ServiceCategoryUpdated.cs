namespace Events.ServiceCategory
{
    public class ServiceCategoryUpdated
    {
        public required Guid Id { get; set; }
        public required string CategoryName { get; set; }
        public required TimeSpan TimeSlotSize { get; set; }
    }
}
