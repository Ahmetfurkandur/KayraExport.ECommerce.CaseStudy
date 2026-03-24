namespace AuthService.Application.Features.Users.Commands.RegisterUserCommand
{
    public class RegisterUserCommandResponse
    {
        public string UserId { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiryTime { get; set; }

    }
}