using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offices.Data.Entities
{
    public class Doctor: BaseEntity
    {
        public string FirstName { get; set; }
        public int LastName { get; set; }
        public int MiddleName { get; set; }
        public string OfficeId { get; set; }
        public string Status { get; set; }

    }
}
