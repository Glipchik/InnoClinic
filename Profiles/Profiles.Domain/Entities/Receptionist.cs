using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Domain.Entities
{
    public class Receptionist : BaseEntity
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }

        public Guid AccountId { get; set; }
        public Account Account { get; set; } = null!;

        public Guid OfficeId { get; set; }
        public Office Office { get; set; } = null!;
    }
}
