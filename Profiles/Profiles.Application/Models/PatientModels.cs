using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Models
{
    public class CreatePatientModel
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class PatientModel
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string MiddleName { get; set; }
        public bool IsLinkedToAccount { get; set; }
        public Guid AccountId { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class UpdatePatientModel
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Guid AuthorId { get; set; }
    }
}
