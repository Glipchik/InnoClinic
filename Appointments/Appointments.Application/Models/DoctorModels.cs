using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointments.Application.Models
{
    public class DoctorModel
    {
        public required Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? MiddleName { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required DateTime CareerStartYear { get; set; }
        public required Guid AccountId { get; set; }
        public required DoctorStatus Status { get; set; } = DoctorStatus.AtWork;
        public required Guid SpecializationId { get; set; }
    }
}
