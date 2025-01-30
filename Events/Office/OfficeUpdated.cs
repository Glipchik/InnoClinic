namespace Events.Office
{
    public class OfficeUpdated
    {
        public required string Id { get; set; }
        public required string Address { get; set; }
        public required string RegistryPhoneNumber { get; set; }
        public required bool IsActive { get; set; }
    }
}
