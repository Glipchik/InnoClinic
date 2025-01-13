using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Models
{
    public record CreatePatientModel(
        string FirstName,
        string LastName,
        string MiddleName,
        DateTime DateOfBirth);

    public record PatientModel(
        Guid Id,
        string FirstName,
        string LastName,
        string MiddleName,
        bool IsLinkedToAccount,
        Guid AccountId,
        DateTime DateOfBirth);

    public record UpdatePatientModel(
        Guid Id,
        string FirstName,
        string LastName,
        string MiddleName,
        DateTime DateOfBirth,
        
        Guid AuthorId);
}
