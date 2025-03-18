using MediatR;
using Microsoft.AspNetCore.Mvc;
using Servicar.Application.Features.Appointment.Commands;
using Servicar.Application.Features.Appointment.Queries;

namespace ServiCar.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetAppointmentFiltered([FromQuery] GetAppointmentFilteredQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAppointment([FromBody] UpdateAppointmentCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAppointment([FromQuery] DeleteAppointmentCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
