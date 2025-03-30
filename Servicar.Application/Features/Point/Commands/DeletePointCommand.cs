using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Point.Commands
{
    public record DeletePointCommand(int Id) : IRequest<Result<string, ErrorDTO>>;

    public class DeletePointCommandHandler : IRequestHandler<DeletePointCommand, Result<string, ErrorDTO>>
    {
        private readonly IPointService _pointService;
        public DeletePointCommandHandler(IPointService pointService)
        {
            _pointService = pointService;
        }

        public async Task<Result<string, ErrorDTO>> Handle(DeletePointCommand request, CancellationToken cancellationToken)
        {
            return await _pointService.DeletePoint(request.Id);
        }
    }
}
