using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offices.Data.Exceptions
{
    public class RelatedObjectNotFoundException : Exception
    {
        public RelatedObjectNotFoundException()
            : base("Related object not found.")
        {
        }

        public RelatedObjectNotFoundException(string message)
            : base(message)
        {
        }

        public RelatedObjectNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
