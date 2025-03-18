using MediatR;
using Servicar.Infrastruture.Services;
using ServiCar.Domain.DTOs;

namespace Servicar.Application.Features.Auth.Queries
{
    public record LoginUserQuery(LoginDTO model) : IRequest<ResponseDTO>;

    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, ResponseDTO>
    {
        private readonly IAuthService _authService;
        public LoginUserQueryHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<ResponseDTO> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            return await _authService.Login(request.model);
        }
    }
}
