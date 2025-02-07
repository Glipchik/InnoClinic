namespace Appointments.Domain.Entities
{
    public class ServiceCategory : BaseEntity
    {
        public required string CategoryName { get; set; }
        public required TimeSpan TimeSlotSize { get; set; }
    }
}
