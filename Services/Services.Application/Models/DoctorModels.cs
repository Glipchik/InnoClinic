using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Application.Models.Enums;

namespace Services.Application.Models
{
    public record CreateDoctorModel(string FirstName, string LastName, string MiddleName, Guid SpecializationId, DoctorStatusModel Status);
    public record DoctorModel(Guid Id, string FirstName, string LastName, string MiddleName, Guid SpecializationId, DoctorStatusModel Status) : BaseModel(Id);
    public record UpdateDoctorModel(Guid Id, string FirstName, string LastName, string MiddleName, Guid SpecializationId, DoctorStatusModel Status) : BaseModel(Id);

}
