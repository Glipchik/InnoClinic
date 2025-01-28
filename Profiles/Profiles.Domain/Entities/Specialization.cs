using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Domain.Entities
{
    public class Specialization : BaseEntity
    {
        public string SpecializationName { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
