using Authorization.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Authorization.Domain.Entities
{
    public class Account : BaseEntity
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        [Required]
        public string PasswordSalt { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public bool IsEmailVerified { get; set; } = false;

        [Required]
        public Role Role { get; set; }
    }
}
