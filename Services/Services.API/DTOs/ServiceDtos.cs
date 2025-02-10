using Services.Application.Models;

namespace Services.API.DTOs
{
    public record CreateServiceDto(string ServiceName, Guid ServiceCategoryId, Guid SpecializationId, decimal Price, bool IsActive);
    public record ServiceDto(Guid Id, string ServiceName, Guid ServiceCategoryId, Guid SpecializationId, decimal Price, bool IsActive);
    public record UpdateServiceDto(Guid Id, string ServiceName, Guid ServiceCategoryId, Guid SpecializationId, decimal Price, bool IsActive);
    public record ServiceQueryParametresDto(Guid? ServiceCategoryId, Guid? SpecializationId, bool? IsActive);
}
