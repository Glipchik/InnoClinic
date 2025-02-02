namespace Offices.Application.Models
{
    public class OfficeModel
    {
        public required Guid Id { get; set; }
        public required string Address { get; set; }
        public required string RegistryPhoneNumber { get; set; }
        public required bool IsActive { get; set; }
        public required IEnumerable<DoctorModel> Doctors { get; set; }
        public required IEnumerable<ReceptionistModel> Receptionists { get; set; }
    }
}
