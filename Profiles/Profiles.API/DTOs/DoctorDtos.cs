using Profiles.API.DTO.Enums;
using System;

namespace Profiles.API.DTOs;

public record CreateDoctorDto(
    CreateAccountDto Account,
    string FirstName,
    string LastName,
    string? MiddleName,
    Guid SpecializationId,  
    Guid OfficeId,
    DateTime DateOfBirth,
    DateTime CareerStartYear,
    DoctorStatusDto Status);

public record DoctorDto(
    Guid Id, 
    string FirstName,
    string LastName,
    string? MiddleName,
    Guid SpecializationId,
    Guid AccountId,
    DateTime CareerStartYear,
    Guid OfficeId,
    DateTime DateOfBirth,
    DoctorStatusDto Status);

public record UpdateDoctorDto(
    Guid Id, 
    string FirstName, 
    string LastName,
    string? MiddleName,
    DateTime CareerStartYear,
    DateTime DateOfBirth);

public record UpdateDoctorByReceptionistDto(
    Guid Id, 
    string FirstName,
    string LastName,
    string? MiddleName,
    Guid SpecializationId,
    DateTime CareerStartYear,
    Guid OfficeId,
    DateTime DateOfBirth,
    DoctorStatusDto Status);

public record DoctorQueryParametresDto(
    Guid? SpecializationId,
    DoctorStatusDto? Status);