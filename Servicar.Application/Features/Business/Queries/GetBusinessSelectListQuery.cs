using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Business.Queries
{
    public record GetBusinessSelectListQuery : IRequest<Result<IEnumerable<BusinessSelectListDTO>, ErrorDTO>>;

    public class GetBusinessSelectListQueryHandler : IRequestHandler<GetBusinessSelectListQuery, Result<IEnumerable<BusinessSelectListDTO>, ErrorDTO>>
    {
        private readonly IBusinessService _businessService;
        public GetBusinessSelectListQueryHandler(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        public async Task<Result<IEnumerable<BusinessSelectListDTO>, ErrorDTO>> Handle(GetBusinessSelectListQuery request, CancellationToken cancellationToken)
        {
            return await _businessService.GetBusinessesForSelectList();
        }
    }
}
