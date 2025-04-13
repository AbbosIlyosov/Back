using MediatR;
using Microsoft.AspNetCore.Mvc;
using Servicar.Application.Features.WorkingTime;

namespace ServiCar.API.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class WorkingTimeController : Controller
    {
        private readonly IMediator _mediator;
        public WorkingTimeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetWorkingTimeQuery());

            if (!result.IsSuccess)
            {
                return StatusCode((int)result.Error.StatusCode, result.Error.Message);
            }

            return Ok(result.Data);
        }
    }
}
