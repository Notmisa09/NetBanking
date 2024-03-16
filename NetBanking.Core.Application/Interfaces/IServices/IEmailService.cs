using NetBanking.Core.Application.Dtos.Email;

namespace NetBanking.Core.Application.Interfaces.IServices
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
