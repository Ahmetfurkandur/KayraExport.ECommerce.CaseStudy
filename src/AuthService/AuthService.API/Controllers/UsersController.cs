using AuthService.Application.Features.Users.Commands.RegisterUserCommand;
using MediatR;
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

        [HttpPost("/register")]
        public async Task<IActionResult> RegisterUser(RegisterUserCommandRequest request)
        {
            return Ok(await mediator.Send(request));
        }
    }
}
