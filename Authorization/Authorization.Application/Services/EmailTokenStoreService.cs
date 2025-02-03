using Authorization.Application.Services.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Authorization.Application.Services
{
    public class EmailTokenStoreService : IEmailTokenStoreService
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _tokenLifetime = TimeSpan.FromMinutes(15);

        public EmailTokenStoreService(IMemoryCache cache, IConfiguration configuration)
        {
            _cache = cache;
            _tokenLifetime = TimeSpan.FromMinutes(Double.Parse(configuration["EmailTokenLifetime:Duration"]
                ?? throw new ArgumentNullException("EmailTokenLifetime is null")));
        }

        public void StoreToken(string email, string token)
        {
            _cache.Set(email, token, _tokenLifetime);
        }

        public bool ValidateToken(string email, string token)
        {
            _cache.TryGetValue(email, out string? storedToken);

            if (storedToken == null)
            {
                return false;
            }

            return storedToken == token;
        }

        public void RemoveToken(string email)
        {
            _cache.Remove(email);
        }
    }
}
