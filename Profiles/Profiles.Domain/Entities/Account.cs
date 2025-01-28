using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Domain.Entities
{
    public class Account : BaseEntity
    {
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool IsEmailVerified { get; set; } 
        public string PhotoFileName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
