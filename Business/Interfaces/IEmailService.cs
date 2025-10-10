using Domain.Entities;

namespace Business.Services
{
    public interface IEmailService
    {
        Task SendVerificationEmailAsync(User user, string verificationLink);
    }
}