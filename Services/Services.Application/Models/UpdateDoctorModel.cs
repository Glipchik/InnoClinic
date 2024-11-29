using Services.Application.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Models
{
    public record UpdateDoctorModel(Guid Id, string FirstName, string LastName, string MiddleName, Guid SpecializationId, DoctorStatusModel Status) : BaseModel(Id);
}
