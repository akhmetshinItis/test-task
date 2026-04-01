using MediatR;
using Microsoft.EntityFrameworkCore;
using Vitacore.Test.Contracts.Requests.Lots.GetLotBids;
using Vitacore.Test.Core.Exceptions;
using Vitacore.Test.Core.Requests.Lots.GetLotBids;
using Vitacore.Test.Data.Postgres;
using Vitacore.Test.Data.Postgres.Entities;
using Vitacore.Test.Infrastructure.Extensions;

namespace Vitacore.Test.Infrastructure.Lots
{
    public class GetLotBidsQueryHandler : IRequestHandler<GetLotBidsQuery, GetLotBidsResponse>
    {
        private readonly IAppDbContext _dbContext;

        public GetLotBidsQueryHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetLotBidsResponse> Handle(GetLotBidsQuery request, CancellationToken cancellationToken)
        {
            var lotExists = await _dbContext.TangerineLots
                .AsNoTracking()
                .AnyAsync(x => x.Id == request.LotId, cancellationToken);

            if (!lotExists)
            {
                throw new EntityNotFoundException<TangerineLot>(request.LotId);
            }

            var query = _dbContext.Bids
                .AsNoTracking()
                .Where(x => x.LotId == request.LotId)
                .Where(x => !request.Request.Id.HasValue || x.Id == request.Request.Id)
                .Where(x => !request.Request.UserId.HasValue || x.UserId == request.Request.UserId)
                .Where(x => !request.Request.Amount.HasValue || x.Amount == request.Request.Amount)
                .Where(x => !request.Request.CreatedAt.HasValue || x.CreatedAt == request.Request.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .ApplyOrdering(request.Request)
                .ApplyPagination(request.Request)
                .Select(x => new BidListItemResponse
                {
                    Id = x.Id,
                    LotId = x.LotId,
                    UserId = x.UserId,
                    Amount = x.Amount,
                    CreatedAt = x.CreatedAt
                })
                .ToArrayAsync(cancellationToken);

            return new GetLotBidsResponse
            {
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}
