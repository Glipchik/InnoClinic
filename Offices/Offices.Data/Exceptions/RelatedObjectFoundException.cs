using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offices.Data.Exceptions
{
    public class RelatedObjectFoundException : Exception
    {
        public RelatedObjectFoundException()
            : base("Related object found.")
        {
        }

        public RelatedObjectFoundException(string message)
            : base(message)
        {
        }

        public RelatedObjectFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
