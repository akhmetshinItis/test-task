using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vitacore.Test.Core.Interfaces.Email;
using Vitacore.Test.Core.Models.Email;
using Vitacore.Test.Core.Requests.Email.SendOutbidEmail;
using Vitacore.Test.Data.Postgres.Identity;
using Vitacore.Test.Core.Exceptions;

namespace Vitacore.Test.Infrastructure.Email
{
    public class SendOutbidEmailCommandHandler : IRequestHandler<SendOutbidEmailCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public SendOutbidEmailCommandHandler(
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task Handle(SendOutbidEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken)
                ?? throw new EntityNotFoundException<ApplicationUser>(request.UserId);

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new AppException("У пользователя отсутствует email для отправки уведомления.");
            }

            var message = new EmailMessage
            {
                To = user.Email,
                Subject = $"Вашу ставку перебили по лоту \"{request.LotTitle}\"",
                Body = $"""
                        Ваша ставка по лоту была перебита.

                        Лот: {request.LotTitle}
                        Идентификатор лота: {request.LotId}
                        Текущая ставка: {request.CurrentPrice:F2}
                        Дата изменения (UTC): {request.OutbidAtUtc:yyyy-MM-dd HH:mm:ss}
                        """,
                IsBodyHtml = false
            };

            await _emailSender.SendAsync(message, cancellationToken);
        }
    }
}
