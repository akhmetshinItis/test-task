using MediatR;
using Vitacore.Test.Contracts.Requests.Lots.GetLotById;

namespace Vitacore.Test.Core.Requests.Lots.GetLotById
{
    public class GetLotByIdQuery : IRequest<GetLotByIdResponse>
    {
        public GetLotByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
