using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Models
{
    public record CreateServiceModel(string ServiceName, Guid ServiceCategoryId, Guid SpecializationId, decimal Price, bool IsActive);
    public record ServiceModel(Guid Id, string ServiceName, ServiceCategoryModel ServiceCategory, SpecializationModel Specialization, decimal Price, bool IsActive) : BaseModel(Id);
    public record UpdateServiceModel(Guid Id, string ServiceName, Guid ServiceCategoryId, Guid SpecializationId, decimal Price, bool IsActive) : BaseModel(Id);
    public record ServiceQueryParametresModel(Guid? ServiceCategoryId, Guid? SpecializationId, bool? IsActive);
}
