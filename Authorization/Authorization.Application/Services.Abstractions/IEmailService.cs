namespace Authorization.Application.Services.Abstractions
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message, CancellationToken cancellationToken);
    }
}
