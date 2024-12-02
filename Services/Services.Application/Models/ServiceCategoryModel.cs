using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Models
{
    public record ServiceCategoryModel(Guid Id, string CategoryName, TimeSpan TimeSlotSize) : BaseModel(Id);
}
