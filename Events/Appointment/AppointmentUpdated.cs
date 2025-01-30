namespace Events.Appointment
{
    public class AppointmentUpdated
    {
        public required Guid Id { get; set; }
        public required Guid PatientId { get; set; }
        public required Guid DoctorId { get; set; }
        public required Guid ServiceId { get; set; }
        public required DateOnly Date { get; set; }
        public required TimeOnly Time { get; set; }
        public required bool IsApproved { get; set; }
    }
}
