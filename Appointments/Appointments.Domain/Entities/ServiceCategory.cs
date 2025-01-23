using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointments.Domain.Entities
{
    public class ServiceCategory : BaseEntity
    {
        public required string CategoryName { get; set; }
        public required TimeSpan TimeSlotSize { get; set; }
    }
}
