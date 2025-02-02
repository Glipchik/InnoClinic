﻿namespace Events.Result
{
    public class ResultUpdated
    {
        public required Guid Id { get; set; }
        public required Guid AppointmentId { get; set; }
        public required string Complaints { get; set; }
        public required string Conclusion { get; set; }
        public required string Recommendations { get; set; }
    }
}
