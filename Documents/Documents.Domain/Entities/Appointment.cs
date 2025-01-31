namespace Documents.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public required Guid PatientId { get; set; }
        public required Patient Patient { get; set; }
        public required Guid DoctorId { get; set; }
        public required Doctor Doctor { get; set; }
        public required Guid ServiceId { get; set; }
        public required Service Service { get; set; }
        public required DateOnly Date { get; set; }
        public required TimeOnly Time { get; set; }
        public required bool IsApproved { get; set; } = false;
    }
}