using Authorization.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Domain.Entities
{
    public class Account : BaseEntity
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public bool IsEmailVerified { get; set; } = false;

        [Required]
        public Roles Role { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        [Required]
        public Guid UpdatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}
