using MediatR;
using ServiCar.Domain.DTOs;

namespace Servicar.Application.Features.Auth.Commands
{
    public record RefreshTokenCommand(TokenApiDTO TokenApiModel) : IRequest<ResponseDTO>;

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ResponseDTO>
    {
        public RefreshTokenCommandHandler()
        {
        }
        public Task<ResponseDTO> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
