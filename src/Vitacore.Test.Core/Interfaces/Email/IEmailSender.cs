using Vitacore.Test.Core.Models.Email;

namespace Vitacore.Test.Core.Interfaces.Email
{
    public interface IEmailSender
    {
        Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
    }
}
