using MediatR;

namespace AuthService.Application.Features.Users.Commands.RenewAccessTokenCommand
{
    public record RenewAccessTokenCommandRequest : IRequest<RenewAccessTokenCommandResponse>
    {
        public string RefreshToken { get; init; }
        public DateTime RefreshTokenExpiry { get; init; }
    }
}
