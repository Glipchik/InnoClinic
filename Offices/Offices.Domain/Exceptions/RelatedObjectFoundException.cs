using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Offices.Domain.Exceptions;

namespace Offices.Domain.Exceptions
{
    public class RelatedObjectFoundException : ValidationException
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
