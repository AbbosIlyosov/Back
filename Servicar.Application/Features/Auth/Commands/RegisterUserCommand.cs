using MediatR;
using Servicar.Infrastruture.Services;
using ServiCar.Domain.DTOs;

namespace Servicar.Application.Features.Auth.Commands
{
    public record RegisterUserCommand(RegistrationDTO model) : IRequest<ResponseDTO>;

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ResponseDTO>
    {
        private readonly IAuthService _authService;
        public RegisterUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<ResponseDTO> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            return await _authService.Register(request.model);
        }
    }
}
