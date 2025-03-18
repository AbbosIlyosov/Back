using MediatR;
using ServiCar.Domain.DTOs;

namespace Servicar.Application.Features.Auth.Commands
{
    public record SwitchToWorkerAccountCommand(string userId) : IRequest<ResponseDTO>;

    public class SwitchToWorkerAccountCommandHandler : IRequestHandler<SwitchToWorkerAccountCommand, ResponseDTO>
    {
        public SwitchToWorkerAccountCommandHandler()
        {
        }
        public Task<ResponseDTO> Handle(SwitchToWorkerAccountCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
