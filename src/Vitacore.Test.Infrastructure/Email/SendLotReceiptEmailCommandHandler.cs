using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vitacore.Test.Core.Interfaces.Email;
using Vitacore.Test.Core.Models.Email;
using Vitacore.Test.Core.Requests.Email.SendLotReceiptEmail;
using Vitacore.Test.Data.Postgres.Identity;
using Vitacore.Test.Core.Enums;
using Vitacore.Test.Core.Exceptions;

namespace Vitacore.Test.Infrastructure.Email
{
    public class SendLotReceiptEmailCommandHandler : IRequestHandler<SendLotReceiptEmailCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public SendLotReceiptEmailCommandHandler(
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task Handle(SendLotReceiptEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken)
                ?? throw new EntityNotFoundException<ApplicationUser>(request.UserId);

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new AppException("У пользователя отсутствует email для отправки чека.");
            }

            var message = new EmailMessage
            {
                To = user.Email,
                Subject = request.PurchaseType == PurchaseType.Buyout
                    ? $"Вы выкупили лот \"{request.LotTitle}\""
                    : $"Вы победили в аукционе \"{request.LotTitle}\"",
                Body = BuildBody(request),
                IsBodyHtml = false
            };

            await _emailSender.SendAsync(message, cancellationToken);
        }

        private static string BuildBody(SendLotReceiptEmailCommand request)
        {
            var purchaseType = request.PurchaseType == PurchaseType.Buyout
                ? "Выкуп"
                : "Победа в аукционе";

            return $"""
                    Спасибо за покупку.

                    Чек:
                    Лот: {request.LotTitle}
                    Идентификатор лота: {request.LotId}
                    Тип покупки: {purchaseType}
                    Сумма: {request.Amount:F2}
                    Дата покупки (UTC): {request.PurchasedAtUtc:yyyy-MM-dd HH:mm:ss}
                    """;
        }
    }
}
