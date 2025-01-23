using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointments.Domain.Entities
{
    public class Specialization : BaseEntity
    {
        public required string SpecializationName { get; set; }
        public bool IsActive { get; set; }
    }
}
