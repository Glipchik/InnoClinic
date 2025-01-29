using Offices.Application.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offices.Application.Models
{
    public record UpdateDoctorModel(Guid Id, string FirstName, string LastName, string? MiddleName, Guid OfficeId, DoctorStatusModel Status);
}
