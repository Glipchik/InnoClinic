using Profiles.Application.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Models
{
    public record CreateDoctorModel(
        string FirstName,
        string LastName,
        string MiddleName,
        Guid SpecializationId,
        DateTime CareerStartYear,
        Guid OfficeId,
        DateTime DateOfBirth,
        DoctorStatusModel Status);

    public record DoctorModel(
        Guid Id,
        string FirstName,
        string LastName,
        string MiddleName,
        Guid SpecializationId,
        Guid AccountId,
        DateTime CareerStartYear,
        Guid OfficeId,
        DateTime DateOfBirth,
        DoctorStatusModel Status);

    public record UpdateDoctorModel(
        Guid Id,
        string FirstName,
        string LastName,
        string MiddleName,
        Guid SpecializationId,
        DateTime CareerStartYear,
        Guid OfficeId,
        DateTime DateOfBirth,
        DoctorStatusModel Status,
        
        Guid AuthorId);
}
