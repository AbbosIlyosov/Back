using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Categories.Queries
{
    public record GetCategoriesQuery() : IRequest<Result<IEnumerable<CategoryDTO>, ErrorDTO>>;

    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Result<IEnumerable<CategoryDTO>, ErrorDTO>>
    {
        private readonly ICategoryService _categoryService;
        public GetCategoriesQueryHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<Result<IEnumerable<CategoryDTO>, ErrorDTO>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _categoryService.GetAll();
        }
    }
}
