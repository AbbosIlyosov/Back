using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Point.Queries
{
    public record GetPointFilteredQuery(PointFilterDTO Model) : IRequest<Result<List<PointGridInfoDTO>, ErrorDTO>>;

    public class GetPointFilteredQueryHandler : IRequestHandler<GetPointFilteredQuery, Result<List<PointGridInfoDTO>, ErrorDTO>>
    {
        private readonly IPointService _pointService;
        public GetPointFilteredQueryHandler(IPointService pointService)
        {
            _pointService = pointService;
        }
        public async Task<Result<List<PointGridInfoDTO>, ErrorDTO>> Handle(GetPointFilteredQuery request, CancellationToken cancellationToken)
        {
            return await _pointService.GetPoints(request.Model);
        }
    }
}
