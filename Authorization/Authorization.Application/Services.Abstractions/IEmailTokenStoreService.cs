namespace Authorization.Application.Services.Abstractions
{
    public interface IEmailTokenStoreService
    {
        public void StoreToken(string email, string token);
        public bool ValidateToken(string email, string token);
        public void RemoveToken(string email);
    }
}
