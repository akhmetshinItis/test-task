using Vitacore.Test.Contracts.Pagination;

namespace Vitacore.Test.Contracts.Requests.Lots.GetLots
{
    public class GetLotsRequest : IPaginationQuery, IOrderByQuery
    {
        public int Page { get; set; } = PaginationDefaults.Page;
        public int PageSize { get; set; } = PaginationDefaults.PageSize;
        public string? OrderBy { get; set; } = "CreatedAt";
        public bool IsAsc { get; set; }
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public decimal? StartPrice { get; set; }
        public decimal? CurrentPrice { get; set; }
        public decimal? BuyoutPrice { get; set; }
        public DateTime? AuctionStartAt { get; set; }
        public DateTime? AuctionEndAt { get; set; }
        public DateTime? ExpirationAt { get; set; }
        public int? Status { get; set; }
        public Guid? CurrentLeaderUserId { get; set; }
        public Guid? BuyerId { get; set; }
        public int? PurchaseType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
    }
}
