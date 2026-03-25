using System.Text.Json.Serialization;

namespace AuthService.Application.Features.Users.Commands.LoginCommand
{
    public class LoginCommandResponse
    {
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime AccessTokenExpiry { get; set; }
    }
}