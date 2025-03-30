using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Business.Commands
{
    public record UpdateBusinessCommand(BusinessUpdateDTO Model): IRequest<Result<string, ErrorDTO>>;

    public class UpdateBusinessCommandHandler : IRequestHandler<UpdateBusinessCommand, Result<string, ErrorDTO>>
    {
        private readonly IBusinessService _businessService;
        public UpdateBusinessCommandHandler(IBusinessService businessService)
        {
            _businessService = businessService;
        }
        public async Task<Result<string, ErrorDTO>> Handle(UpdateBusinessCommand request, CancellationToken cancellationToken)
        {
            return await _businessService.UpdateBusiness(request.Model);
        }
    }
}
