using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offices.Domain.Exceptions
{
    public class ValidationException : BadRequestException
    {
        public Dictionary<string, string[]> Errors { get; }

        public ValidationException(string message, Dictionary<string, string[]> errors) : base(message)
        {
            Errors = errors ?? new Dictionary<string, string[]>();
        }

        public ValidationException(Dictionary<string, string[]> errors)
            : this("Validation failed", errors)
        {
        }
    }
}
