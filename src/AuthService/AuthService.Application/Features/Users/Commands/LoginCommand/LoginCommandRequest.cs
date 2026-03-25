using MediatR;

namespace AuthService.Application.Features.Users.Commands.LoginCommand
{
    public record LoginCommandRequest : IRequest<LoginCommandResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
