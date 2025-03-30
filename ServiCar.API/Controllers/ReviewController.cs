using MediatR;
using Microsoft.AspNetCore.Mvc;
using Servicar.Application.Features.Review.Commands;
using Servicar.Application.Features.Review.Queries;
using ServiCar.Domain.DTOs;

namespace ServiCar.API.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ReviewController : Controller
    {
        private readonly IMediator _mediator;

        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("filtered")]
        public async Task<IActionResult> GetFiltered([FromQuery] ReviewFilterDTO dto)
        {
            var result = await _mediator.Send(new GetFilteredReviewQuery(dto));
            
            if (!result.IsSuccess)
            {
                return StatusCode((int)result.Error.StatusCode, result.Error.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost, Route("create")]
        public async Task<IActionResult> CreateReview([FromBody] ReviewCreateDTO dto)
        {
            var result = await _mediator.Send(new CreateReviewCommand(dto));

            if (!result.IsSuccess)
            {
                return StatusCode((int)result.Error.StatusCode, result.Error.Message);
            }

            return Ok(result.Data);
        }
    }
}
