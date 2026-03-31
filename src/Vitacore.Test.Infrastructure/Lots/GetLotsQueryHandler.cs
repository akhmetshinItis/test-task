using MediatR;
using Microsoft.EntityFrameworkCore;
using Vitacore.Test.Contracts.Requests.Lots.GetLots;
using Vitacore.Test.Core.Requests.Lots.GetLots;
using Vitacore.Test.Data.Postgres;
using Vitacore.Test.Infrastructure.Extensions;

namespace Vitacore.Test.Infrastructure.Lots
{
    public class GetLotsQueryHandler : IRequestHandler<GetLotsQuery, GetLotsResponse>
    {
        private readonly IAppDbContext _dbContext;

        public GetLotsQueryHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetLotsResponse> Handle(GetLotsQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.TangerineLots
                .AsNoTracking()
                .Where(x => !request.Request.Id.HasValue || x.Id == request.Request.Id)
                .Filter(request.Request.Title, x => x.Title)
                .Filter(request.Request.Description, x => x.Description)
                .Filter(request.Request.ImageUrl, x => x.ImageUrl)
                .Where(x => !request.Request.StartPrice.HasValue || x.StartPrice == request.Request.StartPrice)
                .Where(x => !request.Request.CurrentPrice.HasValue || x.CurrentPrice == request.Request.CurrentPrice)
                .Where(x => !request.Request.BuyoutPrice.HasValue || x.BuyoutPrice == request.Request.BuyoutPrice)
                .Where(x => !request.Request.AuctionStartAt.HasValue || x.AuctionStartAt == request.Request.AuctionStartAt)
                .Where(x => !request.Request.AuctionEndAt.HasValue || x.AuctionEndAt == request.Request.AuctionEndAt)
                .Where(x => !request.Request.ExpirationAt.HasValue || x.ExpirationAt == request.Request.ExpirationAt)
                .Where(x => !request.Request.Status.HasValue || (int)x.Status == request.Request.Status)
                .Where(x => !request.Request.CurrentLeaderUserId.HasValue || x.CurrentLeaderUserId == request.Request.CurrentLeaderUserId)
                .Where(x => !request.Request.BuyerId.HasValue || x.BuyerId == request.Request.BuyerId)
                .Where(x => !request.Request.PurchaseType.HasValue || (x.PurchaseType.HasValue && (int)x.PurchaseType.Value == request.Request.PurchaseType))
                .Where(x => !request.Request.CreatedAt.HasValue || x.CreatedAt == request.Request.CreatedAt)
                .Where(x => !request.Request.ClosedAt.HasValue || x.ClosedAt == request.Request.ClosedAt);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .ApplyOrdering(request.Request)
                .ApplyPagination(request.Request)
                .Select(x => new LotListItemResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                    StartPrice = x.StartPrice,
                    CurrentPrice = x.CurrentPrice,
                    BuyoutPrice = x.BuyoutPrice,
                    AuctionStartAt = x.AuctionStartAt,
                    AuctionEndAt = x.AuctionEndAt,
                    ExpirationAt = x.ExpirationAt,
                    Status = x.Status.ToString(),
                    CurrentLeaderUserId = x.CurrentLeaderUserId,
                    BuyerId = x.BuyerId,
                    PurchaseType = x.PurchaseType.HasValue ? x.PurchaseType.Value.ToString() : null,
                    CreatedAt = x.CreatedAt,
                    ClosedAt = x.ClosedAt
                })
                .ToArrayAsync(cancellationToken);

            return new GetLotsResponse
            {
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}
