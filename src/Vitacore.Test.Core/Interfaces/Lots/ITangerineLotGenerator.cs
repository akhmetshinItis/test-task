using Vitacore.Test.Contracts.Requests.Lots.GenerateTangerineLots;
using Vitacore.Test.Core.Entities;

namespace Vitacore.Test.Core.Interfaces.Lots
{
    public interface ITangerineLotGenerator
    {
        IReadOnlyCollection<TangerineLot> Generate(GenerateTangerineLotsRequest request, DateTime now);
    }
}
