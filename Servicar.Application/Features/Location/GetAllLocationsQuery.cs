using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Location
{
    public record GetAllLocationsQuery() : IRequest<Result<IEnumerable<LocationDTO>, ErrorDTO>>;

    public class GetAllLocationsQueryHandler : IRequestHandler<GetAllLocationsQuery, Result<IEnumerable<LocationDTO>, ErrorDTO>>
    {
        private readonly ILocationService _locationService;
        public GetAllLocationsQueryHandler(ILocationService locationService)
        {
            _locationService = locationService;
        }
        public async Task<Result<IEnumerable<LocationDTO>, ErrorDTO>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
        {
            return await _locationService.GetAllLocations();
        }
    }
}
