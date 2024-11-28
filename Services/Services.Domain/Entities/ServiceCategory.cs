using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.Entities
{
    public class ServiceCategory
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "TimeSlotSize must be greater than 0.")]
        public int TimeSlotSize { get; set; }
    }
}
