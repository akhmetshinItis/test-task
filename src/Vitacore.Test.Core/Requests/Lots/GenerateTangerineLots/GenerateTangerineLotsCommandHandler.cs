using MediatR;
using Vitacore.Test.Contracts.Requests.Lots.GenerateTangerineLots;
using Vitacore.Test.Core.Interfaces.Lots;

namespace Vitacore.Test.Core.Requests.Lots.GenerateTangerineLots
{
    public class GenerateTangerineLotsCommandHandler : IRequestHandler<GenerateTangerineLotsCommand, GenerateTangerineLotsResponse>
    {
        private readonly IAppDbContext _dbContext;
        private readonly ITangerineLotGenerator _tangerineLotGenerator;

        public GenerateTangerineLotsCommandHandler(
            IAppDbContext dbContext,
            ITangerineLotGenerator tangerineLotGenerator)
        {
            _dbContext = dbContext;
            _tangerineLotGenerator = tangerineLotGenerator;
        }

        public async Task<GenerateTangerineLotsResponse> Handle(GenerateTangerineLotsCommand request, CancellationToken cancellationToken)
        {
            var generatedLots = _tangerineLotGenerator.Generate(request.Request, DateTime.UtcNow);

            _dbContext.TangerineLots.AddRange(generatedLots);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new GenerateTangerineLotsResponse
            {
                GeneratedLotsCount = generatedLots.Count,
                Items = generatedLots
                    .Select(x => new GeneratedTangerineLotResponse
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
                        PurchaseType = x.PurchaseType?.ToString(),
                        CreatedAt = x.CreatedAt,
                        ClosedAt = x.ClosedAt
                    })
                    .ToList()
            };
        }
    }
}
