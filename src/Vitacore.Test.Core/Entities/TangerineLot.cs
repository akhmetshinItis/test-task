using Vitacore.Test.Core.Abstractions;
using Vitacore.Test.Core.Enums;
using Vitacore.Test.Core.Exceptions;

namespace Vitacore.Test.Core.Entities
{
    public class TangerineLot : BaseEntity
    {
        private readonly decimal _startPrice;
        private decimal _currentPrice;
        private readonly decimal? _buyoutPrice;
        private readonly DateTime _auctionStartAt;
        private readonly DateTime _auctionEndAt;
        private readonly DateTime _expirationAt;

        public string Title
        {
            get;
            init => field = string.IsNullOrWhiteSpace(value)
                ? throw new RequiredFieldNotSpecifiedException(nameof(Title))
                : value.Trim();
        } = null!;

        public string? Description { get; set; }

        public string ImageUrl
        {
            get;
            init => field = string.IsNullOrWhiteSpace(value)
                ? throw new RequiredFieldNotSpecifiedException(nameof(ImageUrl))
                : value.Trim();
        } = null!;

        public decimal StartPrice
        {
            get => _startPrice;
            init
            {
                if (value <= 0)
                {
                    throw new AppException($"{nameof(StartPrice)} должен быть больше нуля.");
                }

                if (_currentPrice > 0 && _currentPrice < value)
                {
                    throw new AppException($"{nameof(CurrentPrice)} не может быть меньше {nameof(StartPrice)}.");
                }

                if (_buyoutPrice.HasValue && _buyoutPrice.Value < value)
                {
                    throw new AppException($"{nameof(BuyoutPrice)} не может быть меньше {nameof(StartPrice)}.");
                }

                _startPrice = value;
            }
        }

        public decimal CurrentPrice
        {
            get => _currentPrice;
            set
            {
                if (value <= 0)
                {
                    throw new AppException($"{nameof(CurrentPrice)} должен быть больше нуля.");
                }

                if (_startPrice > 0 && value < _startPrice)
                {
                    throw new AppException($"{nameof(CurrentPrice)} не может быть меньше {nameof(StartPrice)}.");
                }

                _currentPrice = value;
            }
        }

        public decimal? BuyoutPrice
        {
            get => _buyoutPrice;
            init
            {
                if (value.HasValue && _startPrice > 0 && value.Value < _startPrice)
                {
                    throw new AppException($"{nameof(BuyoutPrice)} не может быть меньше {nameof(StartPrice)}.");
                }

                _buyoutPrice = value;
            }
        }

        public DateTime AuctionStartAt
        {
            get => _auctionStartAt;
            init
            {
                if (value == default)
                {
                    throw new RequiredFieldNotSpecifiedException(nameof(AuctionStartAt));
                }

                if (_auctionEndAt != default && _auctionEndAt <= value)
                {
                    throw new AppException($"{nameof(AuctionEndAt)} должен быть больше {nameof(AuctionStartAt)}.");
                }

                _auctionStartAt = value;
            }
        }

        public DateTime AuctionEndAt
        {
            get => _auctionEndAt;
            init
            {
                if (value == default)
                {
                    throw new RequiredFieldNotSpecifiedException(nameof(AuctionEndAt));
                }

                if (_auctionStartAt != default && value <= _auctionStartAt)
                {
                    throw new AppException($"{nameof(AuctionEndAt)} должен быть больше {nameof(AuctionStartAt)}.");
                }

                if (_expirationAt != default && _expirationAt < value)
                {
                    throw new AppException($"{nameof(ExpirationAt)} не может быть раньше {nameof(AuctionEndAt)}.");
                }

                _auctionEndAt = value;
            }
        }

        public DateTime ExpirationAt
        {
            get => _expirationAt;
            init
            {
                if (value == default)
                {
                    throw new RequiredFieldNotSpecifiedException(nameof(ExpirationAt));
                }

                if (_auctionEndAt != default && value < _auctionEndAt)
                {
                    throw new AppException($"{nameof(ExpirationAt)} не может быть раньше {nameof(AuctionEndAt)}.");
                }

                _expirationAt = value;
            }
        }

        public LotStatus Status { get; set; }

        public Guid? CurrentLeaderUserId { get; set; }
        public Guid? BuyerId { get; set; }
        public PurchaseType? PurchaseType { get; set; }

        public DateTime CreatedAt { get; init; }
        public DateTime? ClosedAt { get; set; }

        public uint Version { get; init; }

        public ICollection<Bid> Bids { get; init; } = new List<Bid>();
    }
}
