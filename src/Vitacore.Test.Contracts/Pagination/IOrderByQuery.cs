namespace Vitacore.Test.Contracts.Pagination
{
    public interface IOrderByQuery
    {
        string? OrderBy { get; }
        bool IsAsc { get; }
    }
}
