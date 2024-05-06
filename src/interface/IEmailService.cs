using MiPortal.Models;

namespace MiPortal.Services
{
    public interface IEmailService
    {
        void SendEmail(EmailDTO request);
    }
}
