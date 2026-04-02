using MediatR;

namespace Vitacore.Test.Core.Requests.Background.GenerateTangerineLots
{
    public class GenerateTangerineLotsBackgroundCommand : IRequest<GenerateTangerineLotsBackgroundResult>
    {
        public GenerateTangerineLotsBackgroundCommand(int count)
        {
            Count = count;
        }

        public int Count { get; }
    }
}
