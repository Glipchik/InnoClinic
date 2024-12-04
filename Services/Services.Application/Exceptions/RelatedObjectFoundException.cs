using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Exceptions
{
    public class RelatedObjectFoundException : BadRequestException
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
