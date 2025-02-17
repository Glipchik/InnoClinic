using System;

namespace Profiles.API.DTOs;

public record CreateAccountDto(
    string Email,
    string PhoneNumber);

public record CreateAccountFromAuthDto(
    Guid Id,
    string Email,
    string PhoneNumber);

public record AccountDto(
    Guid Id,
    string Email,
    string PhoneNumber,
    string? PhotoFileName);
