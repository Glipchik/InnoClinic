using Authorization.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Authorization.Domain.Entities
{
    public class Account : BaseEntity
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public bool IsEmailVerified { get; set; } = false;

        [Required]
        public Role Role { get; set; }
    }
}
