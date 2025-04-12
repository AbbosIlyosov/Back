using Microsoft.EntityFrameworkCore;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Persistence;
using System.Net;

namespace ServiCar.Infrastructure.Services
{
    public interface ILocationService
    {
        Task<Result<IEnumerable<LocationDTO>, ErrorDTO>> GetAllLocations();
    }

    public class LocationService : ILocationService
    {
        private readonly ServiCarApiContext _context;
        public LocationService(ServiCarApiContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<LocationDTO>, ErrorDTO>> GetAllLocations()
        {
            try
            {
                var locations = await _context.Locations
                    .Select(location => new LocationDTO
                    {
                        Id = location.Id,
                        City = location.City,
                        District = location.District,
                        Longitude = location.Longitude,
                        Latitude = location.Latitude
                    }).ToListAsync();

                return Result<IEnumerable<LocationDTO>, ErrorDTO>.Success(locations);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "An error occurred while retrieving locations.",
                    Details = ex.Message
                };

                return Result<IEnumerable<LocationDTO>, ErrorDTO>.Fail(error);
            }
        }
    }
}
