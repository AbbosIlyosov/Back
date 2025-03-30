using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Business.Commands
{
    public record CreateBusinessCommand(BusinessCreateDTO Model) : IRequest<Result<BusinessDTO, ErrorDTO>>;

    public class CreateBusinessCommandHandler : IRequestHandler<CreateBusinessCommand, Result<BusinessDTO, ErrorDTO>>
    {
        private readonly IBusinessService _businessService;
        public CreateBusinessCommandHandler(IBusinessService businessService)
        {
            _businessService = businessService;
        }
        public async Task<Result<BusinessDTO, ErrorDTO>> Handle(CreateBusinessCommand request, CancellationToken cancellationToken)
        {
            return await _businessService.CreateBusiness(request.Model);
        }
    }
}
