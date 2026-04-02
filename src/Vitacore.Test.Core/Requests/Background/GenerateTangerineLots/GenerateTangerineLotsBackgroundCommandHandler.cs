using MediatR;
using Vitacore.Test.Core.Interfaces.Lots;

namespace Vitacore.Test.Core.Requests.Background.GenerateTangerineLots
{
    public class GenerateTangerineLotsBackgroundCommandHandler : IRequestHandler<GenerateTangerineLotsBackgroundCommand, GenerateTangerineLotsBackgroundResult>
    {
        private readonly IAppDbContext _dbContext;
        private readonly ITangerineLotBackgroundGenerator _backgroundGenerator;

        public GenerateTangerineLotsBackgroundCommandHandler(
            IAppDbContext dbContext,
            ITangerineLotBackgroundGenerator backgroundGenerator)
        {
            _dbContext = dbContext;
            _backgroundGenerator = backgroundGenerator;
        }

        public async Task<GenerateTangerineLotsBackgroundResult> Handle(GenerateTangerineLotsBackgroundCommand request, CancellationToken cancellationToken)
        {
            var generatedLots = _backgroundGenerator.Generate(request.Count, DateTime.UtcNow);

            _dbContext.TangerineLots.AddRange(generatedLots);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new GenerateTangerineLotsBackgroundResult
            {
                GeneratedLotsCount = generatedLots.Count
            };
        }
    }
}
