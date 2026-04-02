using MediatR;
using Vitacore.Test.Core.Enums;

namespace Vitacore.Test.Core.Requests.Email.SendLotReceiptEmail
{
    public class SendLotReceiptEmailCommand : IRequest
    {
        public SendLotReceiptEmailCommand(
            Guid userId,
            Guid lotId,
            string lotTitle,
            decimal amount,
            PurchaseType purchaseType,
            DateTime purchasedAtUtc)
        {
            UserId = userId;
            LotId = lotId;
            LotTitle = lotTitle ?? throw new ArgumentNullException(nameof(lotTitle));
            Amount = amount;
            PurchaseType = purchaseType;
            PurchasedAtUtc = purchasedAtUtc;
        }

        public Guid UserId { get; }
        public Guid LotId { get; }
        public string LotTitle { get; }
        public decimal Amount { get; }
        public PurchaseType PurchaseType { get; }
        public DateTime PurchasedAtUtc { get; }
    }
}
