﻿namespace Documents.Domain.Entities
{
    public class Result : BaseEntity
    {
        public required Guid AppointmentId { get; set; }
        public required Appointment Appointment { get; set; }
        public required string Complaints { get; set; }
        public required string Conclusion { get; set; }
        public required string Recommendations { get; set; }
    }
}
