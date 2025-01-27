using Profiles.Application.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Models
{
    public class CreateDoctorModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public Guid SpecializationId { get; set; }
        public DateTime CareerStartYear { get; set; }
        public Guid OfficeId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DoctorStatusModel Status { get; set; }
    }

    public class DoctorModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public Guid SpecializationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CareerStartYear { get; set; }
        public Guid OfficeId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DoctorStatusModel Status { get; set; }
    }

    public class UpdateDoctorModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public Guid SpecializationId { get; set; }
        public DateTime CareerStartYear { get; set; }
        public Guid OfficeId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DoctorStatusModel Status { get; set; }
        public Guid AuthorId { get; set; }
    }
}
