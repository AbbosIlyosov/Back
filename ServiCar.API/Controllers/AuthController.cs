using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicar.Application.Features.Auth.Commands;
using Servicar.Application.Features.Auth.Queries;
using ServiCar.Domain.DTOs;

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
        public async Task<IActionResult> CreateAccount([FromBody] UserRegisterDTO model)
        {
            var result = await _mediator.Send(new RegisterUserCommand(model));

            if (!result.IsSuccess)
            {
                _logger.LogError("Error creating account: {Error}", result.Error.Message);
                return StatusCode((int)result.Error.StatusCode, result.Error.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var result = await _mediator.Send(new LoginUserQuery(model));

            if (!result.IsSuccess)
            {
                _logger.LogError("Error logging in: {Error}", result.Error.Message);
                return StatusCode((int)result.Error.StatusCode, result.Error.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost("switch-to-worker-account"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> SwitchToWorkerAccount(int userId)
        {
            var result = await _mediator.Send(new SwitchToWorkerAccountCommand(userId));

            if (!result.IsSuccess)
            {
                return StatusCode((int)result.Error.StatusCode, result.Error.Message);
            }

            return Ok(result.Data);
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
