using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Point.Queries
{
    public record GetPointByIdQuery(int Id) : IRequest<Result<PointDTO, ErrorDTO>>;

    public class GetPointByIdQueryHandler : IRequestHandler<GetPointByIdQuery, Result<PointDTO, ErrorDTO>>
    {
        private readonly IPointService _pointService;
        public GetPointByIdQueryHandler(IPointService pointService)
        {
            _pointService = pointService;
        }
        public async Task<Result<PointDTO, ErrorDTO>> Handle(GetPointByIdQuery request, CancellationToken cancellationToken)
        {
            return await _pointService.GetById(request.Id);
        }
    }
}
