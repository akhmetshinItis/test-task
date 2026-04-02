using Vitacore.Test.Core.Entities;
using Vitacore.Test.Core.Enums;
using Vitacore.Test.Core.Exceptions;
using Vitacore.Test.Core.Interfaces.Lots;
using Vitacore.Test.Core.Options;

namespace Vitacore.Test.Core.Services.Lots
{
    public class TangerineLotBackgroundGenerator : ITangerineLotBackgroundGenerator
    {
        private readonly TangerineLotGenerationOptions _options;

        public TangerineLotBackgroundGenerator(TangerineLotGenerationOptions options)
        {
            _options = options;
        }

        public IReadOnlyCollection<TangerineLot> Generate(int count, DateTime now)
        {
            ValidateOptions();

            if (count <= 0)
            {
                throw new AppException("Количество генерируемых лотов должно быть больше нуля.");
            }

            if (count > _options.MaxGenerateCount)
            {
                throw new AppException($"Нельзя сгенерировать больше {_options.MaxGenerateCount} лотов за один запрос.");
            }

            var generatedLots = new List<TangerineLot>(count);

            for (var index = 0; index < count; index++)
            {
                generatedLots.Add(BuildLot(now.AddMilliseconds(index)));
            }

            return generatedLots;
        }

        private TangerineLot BuildLot(DateTime now)
        {
            var startPrice = Random.Shared.Next(_options.StartPriceMin, _options.StartPriceMax + 1);
            var currentPrice = startPrice;
            var auctionStartAt = now;
            var auctionEndAt = auctionStartAt.AddHours(Random.Shared.Next(_options.AuctionDurationHoursMin, _options.AuctionDurationHoursMax + 1));
            var expirationAt = auctionEndAt.AddDays(Random.Shared.Next(_options.ExpirationDaysMin, _options.ExpirationDaysMax + 1));
            var title = BuildGeneratedTitle(now);

            return new TangerineLot
            {
                Title = title,
                Description = _options.DescriptionTemplates[Random.Shared.Next(_options.DescriptionTemplates.Length)],
                ImageUrl = BuildImageUrl(title),
                StartPrice = startPrice,
                CurrentPrice = currentPrice,
                BuyoutPrice = BuildGeneratedBuyoutPrice(startPrice),
                AuctionStartAt = auctionStartAt,
                AuctionEndAt = auctionEndAt,
                ExpirationAt = expirationAt,
                Status = LotStatus.Active,
                CurrentLeaderUserId = null,
                BuyerId = null,
                PurchaseType = null,
                CreatedAt = now,
                ClosedAt = null
            };
        }

        private void ValidateOptions()
        {
            if (_options.MaxGenerateCount <= 0)
            {
                throw new InvalidOperationException("Максимальное количество генерируемых лотов должно быть больше нуля.");
            }

            if (_options.JobGenerateCount <= 0)
            {
                throw new InvalidOperationException("Количество генерируемых лотов в фоновой задаче должно быть больше нуля.");
            }

            if (_options.StartPriceMin <= 0 || _options.StartPriceMax < _options.StartPriceMin)
            {
                throw new InvalidOperationException("Некорректно задан диапазон стартовой цены для генерации лотов.");
            }

            if (_options.BuyoutMultiplierMin < 0 || _options.BuyoutMultiplierMax < _options.BuyoutMultiplierMin)
            {
                throw new InvalidOperationException("Некорректно задан диапазон множителя цены выкупа для генерации лотов.");
            }

            if (_options.AuctionDurationHoursMin <= 0 || _options.AuctionDurationHoursMax < _options.AuctionDurationHoursMin)
            {
                throw new InvalidOperationException("Некорректно задан диапазон длительности аукциона для генерации лотов.");
            }

            if (_options.ExpirationDaysMin < 0 || _options.ExpirationDaysMax < _options.ExpirationDaysMin)
            {
                throw new InvalidOperationException("Некорректно задан диапазон срока истечения лота для генерации.");
            }

            if (_options.TitlePrefixes.Length == 0)
            {
                throw new InvalidOperationException("Не настроены префиксы заголовков для генерации лотов.");
            }

            if (_options.DescriptionTemplates.Length == 0)
            {
                throw new InvalidOperationException("Не настроены шаблоны описаний для генерации лотов.");
            }
        }

        private string BuildGeneratedTitle(DateTime now)
        {
            var prefix = _options.TitlePrefixes[Random.Shared.Next(_options.TitlePrefixes.Length)];
            var suffix = now.ToString("yyyyMMddHHmmssfff");
            return $"{prefix} Mandarin #{suffix}";
        }

        private decimal BuildGeneratedBuyoutPrice(decimal startPrice)
        {
            var multiplier = _options.BuyoutMultiplierMin
                + (decimal)Random.Shared.NextDouble() * (_options.BuyoutMultiplierMax - _options.BuyoutMultiplierMin);

            return decimal.Round(startPrice * multiplier, 2);
        }

        private string BuildImageUrl(string title)
        {
            var svg = $"""
                       <svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 600 400'>
                           <defs>
                               <linearGradient id='bg' x1='0%' y1='0%' x2='100%' y2='100%'>
                                   <stop offset='0%' stop-color='{_options.SvgBackgroundStartColor}' />
                                   <stop offset='100%' stop-color='{_options.SvgBackgroundEndColor}' />
                               </linearGradient>
                           </defs>
                           <rect width='600' height='400' fill='url(#bg)' rx='32' />
                           <circle cx='300' cy='200' r='118' fill='{_options.SvgMandarinColor}' />
                           <circle cx='250' cy='165' r='20' fill='{_options.SvgHighlightColor}' opacity='0.45' />
                           <ellipse cx='355' cy='112' rx='36' ry='18' fill='{_options.SvgLeafColor}' transform='rotate(-18 355 112)' />
                           <text x='300' y='352' text-anchor='middle' font-family='{_options.SvgFontFamily}' font-size='24' fill='{_options.SvgTitleColor}'>{title}</text>
                       </svg>
                       """;

            return $"data:image/svg+xml;utf8,{Uri.EscapeDataString(svg)}";
        }
    }
}
