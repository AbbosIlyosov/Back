using MediatR;
using ServiCar.Domain.DTOs;

namespace Servicar.Application.Features.Auth.Commands
{
    public record DeleteUserCommand(string userId) : IRequest<ResponseDTO>;

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ResponseDTO>
    {
        public DeleteUserCommandHandler()
        {
        }
        public Task<ResponseDTO> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
