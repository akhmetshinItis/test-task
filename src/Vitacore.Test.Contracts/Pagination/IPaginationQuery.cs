namespace Vitacore.Test.Contracts.Pagination
{
    public interface IPaginationQuery
    {
        int Page { get; }
        int PageSize { get; }
    }
}
