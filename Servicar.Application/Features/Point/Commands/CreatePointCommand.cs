using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Point.Commands
{
    public record CreatePointCommand(CreatePointDTO Model): IRequest<Result<PointDTO, ErrorDTO>>;

    public class CreatePointCommandHandler : IRequestHandler<CreatePointCommand, Result<PointDTO, ErrorDTO>>
    {
        private readonly IPointService _pointService;
        public CreatePointCommandHandler(IPointService pointService)
        {
            _pointService = pointService;
        }
        public async Task<Result<PointDTO, ErrorDTO>> Handle(CreatePointCommand request, CancellationToken cancellationToken)
        {
            return await _pointService.CreatePoint(request.Model);
        }
    }
}
