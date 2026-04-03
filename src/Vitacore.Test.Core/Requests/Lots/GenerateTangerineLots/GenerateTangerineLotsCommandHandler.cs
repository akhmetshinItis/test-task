using MediatR;
using Vitacore.Test.Contracts.Requests.Lots.GenerateTangerineLots;
using Vitacore.Test.Core.Entities;
using Vitacore.Test.Core.Enums;

namespace Vitacore.Test.Core.Requests.Lots.GenerateTangerineLots
{
    public class GenerateTangerineLotsCommandHandler : IRequestHandler<GenerateTangerineLotsCommand, GenerateTangerineLotsResponse>
    {
        private readonly IAppDbContext _dbContext;

        public GenerateTangerineLotsCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GenerateTangerineLotsResponse> Handle(GenerateTangerineLotsCommand request, CancellationToken cancellationToken)
        {
            var generatedLot = new TangerineLot
            {
                Title = request.Request.Title,
                Description = request.Request.Description,
                ImageUrl = request.Request.ImageUrl,
                StartPrice = request.Request.StartPrice,
                CurrentPrice = request.Request.StartPrice,
                BuyoutPrice = request.Request.BuyoutPrice,
                AuctionStartAt = request.Request.AuctionStartAt,
                AuctionEndAt = request.Request.AuctionEndAt,
                ExpirationAt = request.Request.ExpirationAt,
                Status = LotStatus.Active,
                CurrentLeaderUserId = null,
                BuyerId = null,
                PurchaseType = null,
                CreatedAt = DateTime.UtcNow,
                ClosedAt = null
            };

            _dbContext.TangerineLots.Add(generatedLot);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new GenerateTangerineLotsResponse
            {
                Id = generatedLot.Id,
                Title = generatedLot.Title,
                Description = generatedLot.Description,
                ImageUrl = generatedLot.ImageUrl,
                StartPrice = generatedLot.StartPrice,
                CurrentPrice = generatedLot.CurrentPrice,
                BuyoutPrice = generatedLot.BuyoutPrice,
                AuctionStartAt = generatedLot.AuctionStartAt,
                AuctionEndAt = generatedLot.AuctionEndAt,
                ExpirationAt = generatedLot.ExpirationAt,
                Status = generatedLot.Status.ToString(),
                CurrentLeaderUserId = generatedLot.CurrentLeaderUserId,
                BuyerId = generatedLot.BuyerId,
                PurchaseType = generatedLot.PurchaseType?.ToString(),
                CreatedAt = generatedLot.CreatedAt,
                ClosedAt = generatedLot.ClosedAt
            };
        }
    }
}
