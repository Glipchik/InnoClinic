using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Models
{
    public record CreateServiceCategoryModel(string CategoryName, TimeSpan TimeSlotSize);
}
