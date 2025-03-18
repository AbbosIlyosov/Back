using MediatR;
using Servicar.Domain.DTOs;
using ServiCar.Domain.DTOs;

namespace Servicar.Application.Features.Auth.Commands
{
    public record AssignRoleCommand(AssignRoleDTO dto) : IRequest<ResponseDTO>;

    public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand, ResponseDTO>
    {
        public AssignRoleCommandHandler()
        {
        }
        public Task<ResponseDTO> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
