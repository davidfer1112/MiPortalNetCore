using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Microsoft.Extensions.Configuration;
using MiPortal.Models;

namespace MiPortal.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
            // Validar configuraciones al iniciar el servicio
            ValidateEmailConfiguration();
        }

        public void SendEmail(EmailDTO request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["Email:UserName"]));
            email.To.Add(MailboxAddress.Parse(request.Para));
            email.Subject = request.Asunto;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Contenido };

            using var smtp = new SmtpClient();
            smtp.Connect(_config["Email:Host"], GetPort(), SecureSocketOptions.StartTls);
            smtp.Authenticate(_config["Email:UserName"], _config["Email:Password"]);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        private void ValidateEmailConfiguration()
        {
            if (string.IsNullOrEmpty(_config["Email:Host"]))
                throw new InvalidOperationException("Email host configuration is required.");
            if (string.IsNullOrEmpty(_config["Email:UserName"]))
                throw new InvalidOperationException("Email username configuration is required.");
            if (string.IsNullOrEmpty(_config["Email:Password"]))
                throw new InvalidOperationException("Email password configuration is required.");
            if (string.IsNullOrEmpty(_config["Email:Port"]))
                throw new InvalidOperationException("Email port configuration is required.");
        }

        private int GetPort()
        {
            if (!int.TryParse(_config["Email:Port"], out int port))
                throw new InvalidOperationException("The SMTP port configuration is invalid.");

            return port;
        }
    }
}
