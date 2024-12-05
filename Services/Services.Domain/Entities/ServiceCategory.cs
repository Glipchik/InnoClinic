using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.Entities
{
    public class ServiceCategory : BaseEntity
    {
        [Required]
        public string CategoryName { get; set; }

        [Required]
        public TimeSpan TimeSlotSize { get; set; }

        public ICollection<Service> Services { get; set; } = [];
        }
}
