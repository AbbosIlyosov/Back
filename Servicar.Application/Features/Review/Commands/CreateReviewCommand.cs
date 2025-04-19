using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Review.Commands
{
    public record CreateReviewCommand(ReviewCreateDTO Model) : IRequest<Result<bool, ErrorDTO>>;
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Result<bool, ErrorDTO>>
    {
        private readonly IReviewService _reviewService;
        public CreateReviewCommandHandler(IReviewService reviewService)
        {
           _reviewService = reviewService; 
        }
        public async Task<Result<bool, ErrorDTO>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            return await _reviewService.CreateReview(request.Model);
        }
    }
}
