using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.Entities
{
    public class Specialization : BaseEntity
    {
        [Required]
        public string SpecializationName { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public ICollection<Doctor> Doctors { get; set; } = [];
    }
}
