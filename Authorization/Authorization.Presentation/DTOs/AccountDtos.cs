using Authorization.Presentation.DTO.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Authorization.Presentation.DTOs;

public class CreateAccountDto
{
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public int RoleId { get; set; }
}

public class CreateAccountForProfilesDto
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
}

public class ResetPasswordDto
{
    public required string Email { get; set; }
    public required string Token { get; set; }
    public required string NewPassword { get; set; }
}