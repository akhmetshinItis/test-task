using System.Net;
using System.Net.Mail;
using Vitacore.Test.Core.Interfaces.Email;
using Vitacore.Test.Core.Models.Email;
using Vitacore.Test.Infrastructure.Email.Options;

namespace Vitacore.Test.Infrastructure.Email
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly EmailOptions _options;

        public SmtpEmailSender(EmailOptions options)
        {
            _options = options;
        }

        public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(message);

            using var mailMessage = new MailMessage
            {
                From = string.IsNullOrWhiteSpace(_options.FromName)
                    ? new MailAddress(_options.FromAddress)
                    : new MailAddress(_options.FromAddress, _options.FromName),
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = message.IsBodyHtml
            };

            mailMessage.To.Add(message.To);

            using var smtpClient = new SmtpClient(_options.Host, _options.Port)
            {
                EnableSsl = _options.EnableSsl,
                UseDefaultCredentials = _options.UseDefaultCredentials
            };

            if (!_options.UseDefaultCredentials && !string.IsNullOrWhiteSpace(_options.UserName))
            {
                smtpClient.Credentials = new NetworkCredential(_options.UserName, _options.Password);
            }

            await smtpClient.SendMailAsync(mailMessage, cancellationToken);
        }
    }
}
