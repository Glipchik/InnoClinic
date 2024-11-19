using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offices.Data.Entities
{
    public class Office: BaseEntity
    {
        public string Address { get; set; }
        public string Photo {  get; set; }
        public string RegistryPhoneNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
