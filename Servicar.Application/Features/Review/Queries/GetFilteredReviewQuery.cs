using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Review.Queries
{
    public record GetFilteredReviewQuery(ReviewFilterDTO Model) : IRequest<Result<List<ReviewDTO>, ErrorDTO>>;

    public class GetFilteredReviewQueryHandler : IRequestHandler<GetFilteredReviewQuery, Result<List<ReviewDTO>, ErrorDTO>>
    {
        private readonly IReviewService _reviewService;
        public GetFilteredReviewQueryHandler(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        public async Task<Result<List<ReviewDTO>, ErrorDTO>> Handle(GetFilteredReviewQuery request, CancellationToken cancellationToken)
        {
            return await _reviewService.GetFiltered(request.Model);
        }
    }
}
