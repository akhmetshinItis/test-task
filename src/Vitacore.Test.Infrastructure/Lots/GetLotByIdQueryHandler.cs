using MediatR;
using Microsoft.EntityFrameworkCore;
using Vitacore.Test.Contracts.Requests.Lots.GetLotById;
using Vitacore.Test.Core.Exceptions;
using Vitacore.Test.Core.Requests.Lots.GetLotById;
using Vitacore.Test.Data.Postgres;
using Vitacore.Test.Data.Postgres.Entities;

namespace Vitacore.Test.Infrastructure.Lots
{
    public class GetLotByIdQueryHandler : IRequestHandler<GetLotByIdQuery, GetLotByIdResponse>
    {
        private readonly IAppDbContext _dbContext;

        public GetLotByIdQueryHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetLotByIdResponse> Handle(GetLotByIdQuery request, CancellationToken cancellationToken)
        {
            var lot = await _dbContext.TangerineLots
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .Select(x => new GetLotByIdResponse
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
                .FirstOrDefaultAsync(cancellationToken);

            return lot ?? throw new EntityNotFoundException<TangerineLot>(request.Id);
        }
    }
}
