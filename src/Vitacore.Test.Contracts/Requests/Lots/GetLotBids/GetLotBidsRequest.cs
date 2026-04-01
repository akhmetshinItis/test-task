using Vitacore.Test.Contracts.Pagination;

namespace Vitacore.Test.Contracts.Requests.Lots.GetLotBids
{
    public class GetLotBidsRequest : IPaginationQuery, IOrderByQuery
    {
        public int Page { get; set; } = PaginationDefaults.Page;
        public int PageSize { get; set; } = PaginationDefaults.PageSize;
        public string? OrderBy { get; set; } = "CreatedAt";
        public bool IsAsc { get; set; }
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
