using Services.Application.Models;

namespace Services.API.DTOs
{
    public record CreateServiceCategoryDto(string CategoryName, TimeSpan TimeSlotSize);
    public record ServiceCategoryDto(Guid Id, string CategoryName, TimeSpan TimeSlotSize);
    public record UpdateServiceCategoryDto(Guid Id, string CategoryName, TimeSpan TimeSlotSize);
}
