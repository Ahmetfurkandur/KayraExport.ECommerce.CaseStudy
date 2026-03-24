using MediatR;

namespace AuthService.Application.Features.Users.Commands.RegisterUserCommand
{
    public record RegisterUserCommandRequest : IRequest<RegisterUserCommandResponse>
    {
        public string FullName { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
