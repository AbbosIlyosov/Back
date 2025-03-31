using MediatR;
using Servicar.Infrastruture.Services;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;

namespace Servicar.Application.Features.Auth.Commands
{
    public record SwitchToWorkerAccountCommand(int UserId) : IRequest<Result<string, ErrorDTO>>;

    public class SwitchToWorkerAccountCommandHandler : IRequestHandler<SwitchToWorkerAccountCommand, Result<string, ErrorDTO>>
    {
        private readonly IAuthService _authService;
        public SwitchToWorkerAccountCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<Result<string, ErrorDTO>> Handle(SwitchToWorkerAccountCommand request, CancellationToken cancellationToken)
        {
            return await _authService.SwitchToWorkerAccount(request.UserId);
        }
    }
}
