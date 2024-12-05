using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Models
{
    public record CreateServiceCategoryModel(string CategoryName, TimeSpan TimeSlotSize);
    public record ServiceCategoryModel(Guid Id, string CategoryName, TimeSpan TimeSlotSize) : BaseModel(Id);
    public record UpdateServiceCategoryModel(Guid Id, string CategoryName, TimeSpan TimeSlotSize) : BaseModel(Id);

}
