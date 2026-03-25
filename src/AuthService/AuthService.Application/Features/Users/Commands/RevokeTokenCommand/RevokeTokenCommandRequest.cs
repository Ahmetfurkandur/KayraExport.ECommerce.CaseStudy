using MediatR;

namespace AuthService.Application.Features.Users.Commands.RevokeTokenCommand
{
    public record RevokeTokenCommandRequest : IRequest<RevokeTokenCommandResponse>
    {
        public string RefreshToken { get; init; }
    }
}
