using Microsoft.EntityFrameworkCore;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Persistence;
using System.Net;

namespace ServiCar.Infrastructure.Services
{
    public interface IWorkingTimeService
    {
        Task<Result<IEnumerable<WorkingTimeDTO>, ErrorDTO>> GetAll();
    }
    public class WorkingTimeService : IWorkingTimeService
    {
        private readonly ServiCarApiContext _context;
        public WorkingTimeService(ServiCarApiContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<WorkingTimeDTO>, ErrorDTO>> GetAll()
        {
            try
            {
                var categories = await _context.WorkingTimes.Select(x => new WorkingTimeDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                }).ToListAsync();

                if (categories == null)
                {
                    var error = new ErrorDTO { StatusCode = HttpStatusCode.BadRequest, Message = "Could not get working time." };
                    return Result<IEnumerable<WorkingTimeDTO>, ErrorDTO>.Fail(error);
                }
                return Result<IEnumerable<WorkingTimeDTO>, ErrorDTO>.Success(categories);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO { StatusCode = HttpStatusCode.InternalServerError, Message = "Internal server error occured." };
                return Result<IEnumerable<WorkingTimeDTO>, ErrorDTO>.Fail(error);
            }
        }
    }
}
