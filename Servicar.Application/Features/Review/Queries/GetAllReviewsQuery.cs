using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Review.Queries
{
    public record GetAllReviewsQuery() : IRequest<Result<List<ReviewDTO>, ErrorDTO>>;

    public class GetAllReviewsQueryHandler : IRequestHandler<GetAllReviewsQuery, Result<List<ReviewDTO>, ErrorDTO>>
    {
        private readonly IReviewService _reviewService;
        public GetAllReviewsQueryHandler(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        public async Task<Result<List<ReviewDTO>, ErrorDTO>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
        {
            return await _reviewService.GetAll();
        }
    }
}
