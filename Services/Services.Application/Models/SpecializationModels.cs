using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Models
{
    public record CreateSpecializationModel(string SpecializationName, bool IsActive);
    public record SpecializationModel(Guid Id, string SpecializationName, bool IsActive) : BaseModel(Id);
    public record UpdateSpecializationModel(Guid Id, string SpecializationName, bool IsActive) : BaseModel(Id);

}
