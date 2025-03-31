using MediatR;
using Servicar.Domain.DTOs;
using Servicar.Infrastruture.Services;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Auth.Commands
{
    public record AssignRoleCommand(AssignRoleDTO Model) : IRequest<Result<string, ErrorDTO>>;

    public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand, Result<string, ErrorDTO>>
    {
        private readonly IUserService _userService;
        public AssignRoleCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<Result<string, ErrorDTO>> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
        {
            return await _userService.AssignRole(request.Model);
        }
    }
}
