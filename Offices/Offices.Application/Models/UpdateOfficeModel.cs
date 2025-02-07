using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offices.Application.Models
{
    public record UpdateOfficeModel(Guid Id, string Address, string RegistryPhoneNumber, bool IsActive);
}
