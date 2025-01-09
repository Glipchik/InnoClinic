using System;
using System.ComponentModel.DataAnnotations;

namespace Authorization.Presentation.DTOs;

public class CreateAccountDto
{
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }
}
