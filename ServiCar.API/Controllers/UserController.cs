using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicar.Application.DTOs;
using Servicar.Application.Features.Auth.Commands;
using Servicar.Domain.DTOs;
using ServiCar.Domain.DTOs;
using System.Security.Claims;

namespace ServiCar.API.Controllers
{
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("assign-role"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDTO model)
        {
            var result = await _mediator.Send(new AssignRoleCommand(model));

            if (!result.IsSuccess)
            {
                return StatusCode((int)result.Error.StatusCode, result.Error.Message);
            }

            return Ok(result.Data);
        }

        [HttpPut("update-profile"), Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _mediator.Send(new UpdateProfileCommand(userId, model));

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete-user"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser([FromBody] int id)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id));
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("add-worker"), Authorize]
        public async Task<IActionResult> AddWorker([FromBody] UserRegisterDTO model)
        {
            var result = await _mediator.Send(new AddWorkerCommand(model));

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

    }
}
