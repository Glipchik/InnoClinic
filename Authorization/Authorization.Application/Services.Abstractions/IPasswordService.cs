using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Application.Services.Abstractions
{
    public interface IPasswordService
    {
        string HashPassword(string password, string salt);
        string GenerateSalt();
        string GeneratePassword();
    }
}
