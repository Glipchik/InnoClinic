using Authorization.Domain.Enums;
using IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Domain.Entities
{
    public class Account : BaseEntity
    {
        [Required]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public bool IsEmailVerified { get; set; } = false;

        [Required]
        public Role Role { get; set; }

        public string ProviderName { get; set; }

        public Guid ProviderSubjectId { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Claim> Claims { get; set; } = new HashSet<Claim>(new ClaimComparer());
    }
}
