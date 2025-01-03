using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Domain.Entities
{
    public class Specialization : BaseEntity
    {
        public string SpecializationName { get; set; }
        public bool IsActive { get; set; }
    }
}
