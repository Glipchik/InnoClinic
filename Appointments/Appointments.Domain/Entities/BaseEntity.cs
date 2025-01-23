using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointments.Domain.Entities
{
    public class BaseEntity
    {
        public required Guid Id { get; set; }
    }
}
