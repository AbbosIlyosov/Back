using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Business.Queries
{
    public record GetBusinessGridInfoQuery : IRequest<Result<IEnumerable<BusinessGridInfoDTO>, ErrorDTO>>;

    public class GetBusinessGridInfoQueryHandler : IRequestHandler<GetBusinessGridInfoQuery, Result<IEnumerable<BusinessGridInfoDTO>, ErrorDTO>>
    {
        private readonly IBusinessService _businessService;
        public GetBusinessGridInfoQueryHandler(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        public async Task<Result<IEnumerable<BusinessGridInfoDTO>, ErrorDTO>> Handle(GetBusinessGridInfoQuery request, CancellationToken cancellationToken)
        {
            return await _businessService.GetBusinessGridInfo();
        }
    }
}
