using MediatR;
using Microsoft.AspNetCore.Mvc;
using Servicar.Application.Features.Location;

namespace ServiCar.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : Controller
    {
        private readonly IMediator _mediator;
        public LocationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllLocationsQuery());

            if (!result.IsSuccess)
            {
                return StatusCode((int)result.Error.StatusCode, result.Error.Message);
            }

            return Ok(result.Data);
        }
    }
}
