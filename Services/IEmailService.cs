namespace VegEaseBackend.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string recipientName, string recipientEmail, string subject);
    }
}
