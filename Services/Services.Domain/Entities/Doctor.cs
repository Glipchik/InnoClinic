using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.Entities
{
    public class Doctor
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public Guid SpecializationId { get; set; }

        [ForeignKey("SpecializationId")]
        public Specialization Specialization { get; set; }

        [Required]
        public string Status { get; set; }
    }
}
