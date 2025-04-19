using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Auth.Commands
{
    public record AddWorkerCommand(AddWorkerDTO Model): IRequest<Result<string, ErrorDTO>>;

    public class AddWorkerCommandHandler : IRequestHandler<AddWorkerCommand, Result<string, ErrorDTO>>
    {
        private readonly IUserService _userService;
        public AddWorkerCommandHandler(IUserService userService)
        {
           _userService = userService; 
        }
        public async Task<Result<string, ErrorDTO>> Handle(AddWorkerCommand request, CancellationToken cancellationToken)
        {
            return await _userService.AddWorkerAccount(request.Model);
        }
    }
}
