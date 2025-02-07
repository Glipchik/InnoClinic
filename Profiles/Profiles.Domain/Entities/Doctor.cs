using Profiles.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Domain.Entities
{
    public class Doctor : BaseEntity
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public DateTime CareerStartYear { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DoctorStatus Status { get; set; }

        public Guid SpecializationId { get; set; }
        public Specialization Specialization { get; set; } = null!;

        public Guid AccountId { get; set; }
        public Account Account { get; set; } = null!;

        public Guid OfficeId { get; set; }
        public Office Office { get; set; } = null!;
    }
}
