using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Models
{
    public record CreateServiceModel(string ServiceName, Guid ServiceCategoryId, Guid SpecializationId, decimal Price, bool IsActive);
}
