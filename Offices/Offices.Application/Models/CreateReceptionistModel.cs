using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offices.Application.Models
{
    public record CreateReceptionistModel(string FirstName, string LastName, string? MiddleName, Guid OfficeId);
}
