using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Application.Services.Abstractions
{
    public interface IHashService
    {
        public string HashString(string message);
    }
}
