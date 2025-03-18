using MediatR;
using Servicar.Application.DTOs;
using ServiCar.Domain.DTOs;

namespace Servicar.Application.Features.Auth.Commands
{
    public record UpdateProfileCommand(string userId, UpdateProfileDTO model) : IRequest<ResponseDTO>;

    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, ResponseDTO>
    {
        public UpdateProfileCommandHandler()
        {
        }
        public Task<ResponseDTO> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
