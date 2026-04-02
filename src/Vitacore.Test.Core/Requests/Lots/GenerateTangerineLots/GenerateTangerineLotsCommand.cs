using MediatR;
using Vitacore.Test.Contracts.Requests.Lots.GenerateTangerineLots;

namespace Vitacore.Test.Core.Requests.Lots.GenerateTangerineLots
{
    public class GenerateTangerineLotsCommand : IRequest<GenerateTangerineLotsResponse>
    {
        public GenerateTangerineLotsCommand(GenerateTangerineLotsRequest request)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public GenerateTangerineLotsRequest Request { get; }
    }
}
