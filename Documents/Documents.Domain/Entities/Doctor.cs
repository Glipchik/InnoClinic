﻿using Documents.Domain.Enums;

namespace Documents.Domain.Entities
{
    public class Doctor : BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? MiddleName { get; set; }
        public required Guid AccountId { get; set; }
        public required Account Account { get; set; }
        public required DoctorStatus Status { get; set; } = DoctorStatus.AtWork;
        public required Guid SpecializationId { get; set; }
        public required Specialization Specialization { get; set; }
    }
}