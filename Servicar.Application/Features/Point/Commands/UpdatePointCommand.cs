using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Point.Commands
{
    public record UpdatePointCommand(UpdatePointDTO Model) : IRequest<Result<string, ErrorDTO>>;

    public class UpdatePointCommandHandler : IRequestHandler<UpdatePointCommand, Result<string, ErrorDTO>>
    {
        private readonly IPointService _pointService;
        public UpdatePointCommandHandler(IPointService pointService)
        {
            _pointService = pointService;
        }
        public async Task<Result<string, ErrorDTO>> Handle(UpdatePointCommand request, CancellationToken cancellationToken)
        {
            return await _pointService.UpdatePoint(request.Model);
        }
    }
}
