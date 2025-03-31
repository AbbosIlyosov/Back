using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Auth.Commands
{
    public record DeleteUserCommand(int userId) : IRequest<Result<string, ErrorDTO>>;

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<string, ErrorDTO>>
    {
        private readonly IUserService _userService;
        public DeleteUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<Result<string, ErrorDTO>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.DeleteUser(request.userId);
        }
    }
}
