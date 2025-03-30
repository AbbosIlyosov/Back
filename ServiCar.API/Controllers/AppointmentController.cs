using MediatR;
using Microsoft.AspNetCore.Mvc;
using Servicar.Application.Features.Appointment.Commands;
using Servicar.Application.Features.Appointment.Queries;
using ServiCar.Domain.DTOs;

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
        public async Task<IActionResult> GetAppointmentFiltered([FromQuery] AppointmentFilterDTO filter)
        {
            var result = await _mediator.Send(new GetAppointmentFilteredQuery(filter));

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Data);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentCreateDTO dto)
        {
            var result = await _mediator.Send(new CreateAppointmentCommand(dto));

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Error);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAppointment([FromBody] AppointmentUpdateDTO dto)
        {
            await _mediator.Send(new UpdateAppointmentCommand(dto));
            return NoContent();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAppointment([FromQuery] int id)
        {
            var result = await _mediator.Send(new DeleteAppointmentCommand(id));

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok("Appointment deleted successfully.");
        }
    }
}
