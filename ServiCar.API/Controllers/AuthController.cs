using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicar.Application.DTOs;
using Servicar.Application.Features.Auth.Commands;
using Servicar.Application.Features.Auth.Queries;
using Servicar.Domain.DTOs;
using ServiCar.Domain.DTOs;
using System.Security.Claims;

namespace ServiCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("create-account")]
        public async Task<IActionResult> CreateAccount([FromBody] RegistrationDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid payload");

            var result = await _mediator.Send(new RegisterUserCommand(model));
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid payload");

            var result = await _mediator.Send(new LoginUserQuery(model));
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("assign-role"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDTO model)
        {
            var result = await _mediator.Send(new AssignRoleCommand(model));
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("switch-to-worker-account"), Authorize]
        public async Task<IActionResult> SwitchToWorkerAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _mediator.Send(new SwitchToWorkerAccountCommand(userId));

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update-profile"), Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _mediator.Send(new UpdateProfileCommand(userId, model));

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("delete-user"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser([FromBody] string id)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id));
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        //[HttpPost("refresh"), Authorize]
        //public async Task<IActionResult> Refresh([FromBody] TokenApiDTO tokenApiModel)
        //{
        //    var result = await _mediator.Send(new RefreshTokenCommand(tokenApiModel));
        //    return result.IsSuccess ? Ok(result) : BadRequest(result);
        //}

        //[HttpPost("revoke-refresh-token"), Authorize]
        //public async Task<IActionResult> RevokeRefreshToken()
        //{
        //    var username = User.Identity.Name;
        //    var result = await _mediator.Send(new RevokeTokenCommand(username));

        //    return result ? NoContent() : BadRequest();
        //}

        //[HttpPost("change-password"), Authorize]
        //public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var result = await _mediator.Send(new ChangePasswordCommand(userId, model));

        //    return result.IsSuccess ? Ok(result) : BadRequest(result);
        //}
    }
}
