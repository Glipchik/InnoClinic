using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointments.Domain.Entities
{
    public class Service : BaseEntity
    {
        public required string ServiceName { get; set; }
        public required Guid SpecializationId { get; set; }
        public required Specialization Specialization { get; set; }
        public required Guid ServiceCategoryId { get; set; }
        public required ServiceCategory ServiceCategory { get; set; }
        public 
    }
}
