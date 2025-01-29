﻿namespace Events.Specialization
{
    public class SpecializationCreated
    {
        public required Guid Id { get; set; }
        public required string SpecializationName { get; set; }
        public required bool IsActive { get; set; }
    }
}
