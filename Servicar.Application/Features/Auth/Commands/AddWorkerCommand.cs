using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Auth.Commands
{
    public record AddWorkerCommand(UserRegisterDTO Model): IRequest<Result<string, ErrorDTO>>;

    public class AddWorkerCommandHandler : IRequestHandler<AddWorkerCommand, Result<string, ErrorDTO>>
    {
        private readonly IUserService _userService;
        public AddWorkerCommandHandler(IUserService userService)
        {
           _userService = userService; 
        }
        public Task<Result<string, ErrorDTO>> Handle(AddWorkerCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
