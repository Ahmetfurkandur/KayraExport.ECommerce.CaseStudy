namespace AuthService.Application.Features.Users.Commands.RenewAccessTokenCommand
{
    public class RenewAccessTokenCommandResponse
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiry { get; set; }
    }
}