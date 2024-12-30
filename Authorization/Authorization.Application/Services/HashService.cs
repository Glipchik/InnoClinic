using Authorization.Application.Services.Abstractions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Application.Services
{
    public class HashService : IHashService
    {
        private readonly byte[] _salt;

        public HashService(IConfiguration configuration)
        {
            _salt = Encoding.UTF8.GetBytes(configuration.GetSection("Hash")["Salt"]);
        }

        public string HashString(string message)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: message,
                salt: _salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }
    }
}
