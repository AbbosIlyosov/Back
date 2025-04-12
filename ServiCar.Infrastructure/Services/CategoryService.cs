using Microsoft.EntityFrameworkCore;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Persistence;
using System.Net;

namespace ServiCar.Infrastructure.Services
{

    public interface ICategoryService
    {
        Task<Result<IEnumerable<CategoryDTO>, ErrorDTO>> GetAll();
    }
    public class CategoryService : ICategoryService
    {
        private readonly ServiCarApiContext _context;
        public CategoryService(ServiCarApiContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<CategoryDTO>, ErrorDTO>> GetAll()
        {
            try
            {
                var categories = await _context.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();

                if (categories == null)
                {
                    var error = new ErrorDTO { StatusCode = HttpStatusCode.BadRequest, Message = "Could not get categories." };
                    return Result<IEnumerable<CategoryDTO>, ErrorDTO>.Fail(error);
                }
                return Result<IEnumerable<CategoryDTO>, ErrorDTO>.Success(categories);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO { StatusCode = HttpStatusCode.InternalServerError, Message = "Internal server error occured." };
                return Result<IEnumerable<CategoryDTO>, ErrorDTO>.Fail(error);
            }
        }
    }
}
