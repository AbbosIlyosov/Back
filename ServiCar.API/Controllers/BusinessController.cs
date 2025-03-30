using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicar.Application.Features.Business.Commands;
using Servicar.Application.Features.Business.Queries;
using ServiCar.Domain.DTOs;
using System.Threading.Tasks;

namespace ServiCar.API.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class BusinessController : Controller
    {
        private readonly IMediator _mediator;

        public BusinessController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("get-all")]
        public async Task<IActionResult> GetAllBusinesses()
        {
            var result = await _mediator.Send(new GetAllBusinessesQuery());

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            if (!result.Data.Any()) 
            {
                return NotFound("No businesses found");
            }

            return Ok(result.Data);
        }

        [HttpPost, Route("create")]
        public async Task<IActionResult> CreateBusiness([FromBody] BusinessCreateDTO dto)
        {
            var result = await _mediator.Send(new CreateBusinessCommand(dto));

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Data);
        }

        [HttpPut, Route("update")]
        public async Task<IActionResult> UpdateBusiness([FromBody] BusinessUpdateDTO dto)
        {
            var result = await _mediator.Send(new UpdateBusinessCommand(dto));

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Data);
        }

        [HttpPut, Route("update-status")]
        public async Task<IActionResult> UpdateBusinessStatus([FromBody] BusinessStatusUpdateDTO dto)
        {
            var result = await _mediator.Send(new UpdateBusinessStatusCommand(dto)); 
            
            if (!result.IsSuccess) 
            { 
                return BadRequest(result.Error); 
            }

            return Ok(result.Data);
        }
    }
}
