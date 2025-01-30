namespace Events.Office
{
    public class OfficeCreated
    {
        public required string Id { get; set; }
        public required string Address { get; set; }
        public required string RegistryPhoneNumber { get; set; }
        public required bool IsActive { get; set; }
    }
}
