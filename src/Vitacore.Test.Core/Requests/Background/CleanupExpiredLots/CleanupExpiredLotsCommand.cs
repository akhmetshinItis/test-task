using MediatR;

namespace Vitacore.Test.Core.Requests.Background.CleanupExpiredLots
{
    public class CleanupExpiredLotsCommand : IRequest<CleanupExpiredLotsResult>
    {
    }
}
