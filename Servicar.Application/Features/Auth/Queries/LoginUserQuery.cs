using MediatR;
using Servicar.Infrastruture.Services;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;

namespace Servicar.Application.Features.Auth.Queries
{
    public record LoginUserQuery(LoginDTO Model) : IRequest<Result<TokenDTO, ErrorDTO>>;

    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, Result<TokenDTO, ErrorDTO>>
    {
        private readonly IAuthService _authService;
        public LoginUserQueryHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Result<TokenDTO, ErrorDTO>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            return await _authService.Login(request.Model);
        }
    }
}
