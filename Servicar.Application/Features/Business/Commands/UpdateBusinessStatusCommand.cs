using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Business.Commands
{
    public record UpdateBusinessStatusCommand(BusinessStatusUpdateDTO Model) : IRequest<Result<string, ErrorDTO>>;

    public class UpdateBusinessStatusCommandHandler : IRequestHandler<UpdateBusinessStatusCommand, Result<string, ErrorDTO>>
    {
        private readonly IBusinessService _businessService;
        public UpdateBusinessStatusCommandHandler(IBusinessService businessService)
        {
            _businessService = businessService;
        }
        public async Task<Result<string, ErrorDTO>> Handle(UpdateBusinessStatusCommand request, CancellationToken cancellationToken)
        {
            return await _businessService.UpdateBusinessStatus(request.Model);
        }
    }
}
