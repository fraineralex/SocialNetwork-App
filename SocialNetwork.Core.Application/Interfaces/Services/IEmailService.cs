using SocialNetwork.Core.Application.Dtos.Email;

namespace SocialNetwork.Core.Application
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
