using Authorization.Presentation.DTO.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Authorization.Presentation.DTOs;

public class CreateAccountDto
{
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int RoleId { get; set; }
}

public class CreateAccountForProfilesDto
{
    Guid Id { get; set; }
    string Email { get; set; }
    string PhoneNumber { get; set; }
}
