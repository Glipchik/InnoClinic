using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.Entities
{
    public class Service
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ServiceCategoryId { get; set; }

        [ForeignKey("ServiceCategoryId")]
        public ServiceCategory ServiceCategory { get; set; }

        [Required]
        public string ServiceName { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public double Price { get; set; }

        [Required]
        public Guid SpecializationId { get; set; }

        [ForeignKey("SpecializationId")]
        public Specialization Specialization { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
    }
}
