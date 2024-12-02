using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Models
{
    public record UpdateSpecializationModel(Guid Id, string SpecializationName, bool IsActive) : BaseModel(Id);
}
