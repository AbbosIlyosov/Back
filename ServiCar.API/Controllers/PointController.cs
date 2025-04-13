using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicar.Application.Features.Point.Commands;
using Servicar.Application.Features.Point.Queries;
using ServiCar.Domain.DTOs;
using System.Net;
using System.Threading.Tasks;

namespace ServiCar.API.Controllers
{
    //[Authorize]
    [ApiController, Route("api/[controller]")]
    public class PointController : Controller
    {
        private readonly IMediator _mediator;
        public PointController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("get-by-id")]
        public async Task<IActionResult> GetPointById(int id)
        {
            var result = await _mediator.Send(new GetPointByIdQuery(id));

            if (!result.IsSuccess)
            {
                return StatusCode((int)result.Error.StatusCode, result.Error.Message);
            }

            return Ok(result.Data);
        }

        [HttpGet, Route("filtered")]
        public async Task<IActionResult> GetPointsFiltered([FromQuery] PointFilterDTO dto)
        {
            var result = await _mediator.Send(new GetPointFilteredQuery(dto));

            if (!result.IsSuccess)
            {
                return StatusCode((int)result.Error.StatusCode, result.Error.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost, Route("create")]
        public async Task<IActionResult> CreatePoint([FromBody] CreatePointDTO dto)
        {
            var result = await _mediator.Send(new CreatePointCommand(dto));

            if (!result.IsSuccess)
            {
                return StatusCode((int)result.Error.StatusCode, result.Error.Message);
            }

            return Ok(result.Data);
        }

        [HttpPut, Route("update")]
        public async Task<IActionResult> UpdatePoint([FromBody] UpdatePointDTO dto)
        {
            var result = await _mediator.Send(new UpdatePointCommand(dto));

            if (!result.IsSuccess)
            {
                return StatusCode((int)result.Error.StatusCode, result.Error.Message);
            }

            return Ok(result.Data);
        }

        [HttpDelete, Route("delete")]
        public async Task<IActionResult> DeletePoint(int id)
        {
            var result = await _mediator.Send(new DeletePointCommand(id));

            if (!result.IsSuccess)
            {
                return StatusCode((int)result.Error.StatusCode, result.Error.Message);
            }

            return Ok(result.Data);
        }
    }
}
