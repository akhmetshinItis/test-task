using Vitacore.Test.Core.Entities;

namespace Vitacore.Test.Core.Interfaces.Lots
{
    public interface ITangerineLotBackgroundGenerator
    {
        IReadOnlyCollection<TangerineLot> Generate(int count, DateTime now);
    }
}
