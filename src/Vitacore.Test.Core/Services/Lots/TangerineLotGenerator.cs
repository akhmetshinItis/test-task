using Vitacore.Test.Contracts.Requests.Lots.GenerateTangerineLots;
using Vitacore.Test.Core.Entities;
using Vitacore.Test.Core.Enums;
using Vitacore.Test.Core.Exceptions;
using Vitacore.Test.Core.Interfaces.Lots;
using Vitacore.Test.Core.Options;

namespace Vitacore.Test.Core.Services.Lots
{
    public class TangerineLotGenerator : ITangerineLotGenerator
    {
        private readonly TangerineLotGenerationOptions _options;

        public TangerineLotGenerator(TangerineLotGenerationOptions options)
        {
            _options = options;
        }

        public IReadOnlyCollection<TangerineLot> Generate(GenerateTangerineLotsRequest request, DateTime now)
        {
            if (_options.MaxGenerateCount <= 0)
            {
                throw new InvalidOperationException("Максимальное количество генерируемых лотов должно быть больше нуля.");
            }

            if (request.Count <= 0)
            {
                throw new AppException("Количество генерируемых лотов должно быть больше нуля.");
            }

            if (request.Count > _options.MaxGenerateCount)
            {
                throw new AppException($"Нельзя сгенерировать больше {_options.MaxGenerateCount} лотов за один запрос.");
            }

            var generatedLots = new List<TangerineLot>(request.Count);

            for (var index = 0; index < request.Count; index++)
            {
                generatedLots.Add(BuildLot(request, now.AddMilliseconds(index)));
            }

            return generatedLots;
        }

        private TangerineLot BuildLot(GenerateTangerineLotsRequest request, DateTime createdAt)
        {
            return new TangerineLot
            {
                Title = request.Title,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                StartPrice = request.StartPrice,
                CurrentPrice = request.StartPrice,
                BuyoutPrice = request.BuyoutPrice,
                AuctionStartAt = request.AuctionStartAt,
                AuctionEndAt = request.AuctionEndAt,
                ExpirationAt = request.ExpirationAt,
                Status = LotStatus.Active,
                CurrentLeaderUserId = null,
                BuyerId = null,
                PurchaseType = null,
                CreatedAt = createdAt,
                ClosedAt = null
            };
        }
    }
}
