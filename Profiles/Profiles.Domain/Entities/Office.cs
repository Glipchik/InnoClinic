using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Domain.Entities
{
    public class Office : BaseEntity
    {
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
