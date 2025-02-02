using Profiles.API.DTO.Enums;
using System;

namespace Profiles.API.DTOs;

public record CreatePatientDto(
    CreateAccountDto Account,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime DateOfBirth);

public record CreatePatientFromAuthServerDto(
    CreateAccountFromAuthDto CreateAccountFromAuthDto,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime DateOfBirth);

public record PatientDto(
    Guid Id, 
    string FirstName,
    string LastName,
    string? MiddleName,
    Guid AccountId,
    DateTime DateOfBirth);

public record UpdatePatientDto(
    Guid Id, 
    string FirstName, 
    string LastName,
    string? MiddleName,
    DateTime DateOfBirth);

public record UpdatePatientByReceptionistDto(
    Guid Id,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime DateOfBirth);