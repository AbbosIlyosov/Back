using MediatR;
using Servicar.Infrastruture.Services;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;

namespace Servicar.Application.Features.Auth.Commands
{
    public record RegisterUserCommand(UserRegisterDTO Model) : IRequest<Result<string, ErrorDTO>>;

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<string, ErrorDTO>>
    {
        private readonly IAuthService _authService;
        public RegisterUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Result<string, ErrorDTO>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            return await _authService.Register(request.Model);
        }
    }
}
