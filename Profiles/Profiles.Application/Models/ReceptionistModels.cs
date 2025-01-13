using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Models
{
    public record CreateReceptionistModel(
        string FirstName,
        string LastName,
        string MiddleName,
        Guid OfficeId);

    public record ReceptionistModel(
        Guid Id,
        string FirstName,
        string LastName,
        string MiddleName,
        Guid AccountId,
        Guid OfficeId);

    public record UpdateReceptionistModel(
        Guid Id,
        string FirstName,
        string LastName,
        string MiddleName,
        Guid OfficeId,
        
        Guid AuthorId);
}
