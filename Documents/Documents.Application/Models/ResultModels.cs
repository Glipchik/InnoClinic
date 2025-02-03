namespace Documents.Application.Models
{
    public class ResultModel
    {
        public Guid Id { get; set; }
        public required Guid AppointmentId { get; set; }
        public required AppointmentModel Appointment { get; set; }
        public required string Complaints { get; set; }
        public required string Conclusion { get; set; }
        public required string Recommendations { get; set; }
    }

    public class CreateResultModel
    {
        public required Guid AppointmentId { get; set; }
        public required string Complaints { get; set; }
        public required string Conclusion { get; set; }
        public required string Recommendations { get; set; }
    }

    public class UpdateResultModel
    {
        public required Guid Id { get; set; }
        public required string Complaints { get; set; }
        public required string Conclusion { get; set; }
        public required string Recommendations { get; set; }
    }
}
