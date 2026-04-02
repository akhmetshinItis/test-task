using MediatR;

namespace Vitacore.Test.Core.Requests.Email.SendOutbidEmail
{
    public class SendOutbidEmailCommand : IRequest
    {
        public SendOutbidEmailCommand(
            Guid userId,
            Guid lotId,
            string lotTitle,
            decimal currentPrice,
            DateTime outbidAtUtc)
        {
            UserId = userId;
            LotId = lotId;
            LotTitle = lotTitle ?? throw new ArgumentNullException(nameof(lotTitle));
            CurrentPrice = currentPrice;
            OutbidAtUtc = outbidAtUtc;
        }

        public Guid UserId { get; }
        public Guid LotId { get; }
        public string LotTitle { get; }
        public decimal CurrentPrice { get; }
        public DateTime OutbidAtUtc { get; }
    }
}
