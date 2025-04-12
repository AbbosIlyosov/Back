using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Business.Queries
{
    public record GetAllBusinessesQuery : IRequest<Result<List<BusinessDTO>, ErrorDTO>>;

    public class GetAllBusinessesQueryHandler : IRequestHandler<GetAllBusinessesQuery, Result<List<BusinessDTO>, ErrorDTO>>
    {
        private readonly IBusinessService _businessService;
        public GetAllBusinessesQueryHandler(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        public async Task<Result<List<BusinessDTO>, ErrorDTO>> Handle(GetAllBusinessesQuery request, CancellationToken cancellationToken)
        {
            return await _businessService.GetAllBusinesses();
        }
    }
}
