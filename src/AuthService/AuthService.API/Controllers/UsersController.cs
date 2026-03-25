using AuthService.Application.Features.Users.Commands.LoginCommand;
using AuthService.Application.Features.Users.Commands.RegisterUserCommand;
using AuthService.Application.Features.Users.Commands.RenewAccessTokenCommand;
using AuthService.Application.Features.Users.Commands.RevokeTokenCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser(RegisterUserCommandRequest request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(LoginCommandRequest request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout(RevokeTokenCommandRequest request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpPost("refresh")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken(RenewAccessTokenCommandRequest request)
        {
            return Ok(await mediator.Send(request));
        }
    }
}
