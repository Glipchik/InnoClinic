using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documents.Domain.Entities
{
    public class Account : BaseEntity
    {
        public required string Email { get; set; }
    }
}
