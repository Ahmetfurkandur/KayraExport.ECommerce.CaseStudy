using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Authentication;

namespace AuthService.Application.Features.Users.Commands.LoginCommand
{
    public class LoginCommandHandler : IRequestHandler<LoginCommandRequest, LoginCommandResponse>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenService tokenService;
        private readonly IConfiguration configuration;
        private readonly ILogger<LoginCommandHandler> logger;
        public LoginCommandHandler(UserManager<AppUser> userManager, ITokenService tokenService, IConfiguration configuration, ILogger<LoginCommandHandler> logger)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
        {
            // Mail kontrolü
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                throw new AuthenticationException("Yanlış email adresi!");
            }

            //Parola kontrolü
            var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                throw new AuthenticationException("Yanlış parola!");
            }

            var roles = await userManager.GetRolesAsync(user);

            //Token generation
            var accessToken = tokenService.GenerateAccessToken(user, roles.ToList());
            var refreshToken = tokenService.GenerateRefreshToken();

            //Refresh token'ı db ye kaydediyoruz.
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(Convert.ToDouble(configuration["JWT_REFRESH_EXPIRY_DAYS"]));
            var updateResult = await userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                logger.LogWarning(
                    "Failed to persist refresh token for user {UserId}. Token will not be available for renewal.",
                    user.Id);
            }

            logger.LogInformation(
                    "User {UserId}'s refresh token updated successfully",
                    user.Id);

            var response = new LoginCommandResponse()
            {
                AccessToken = accessToken,
                UserName = user.UserName!,
                RefreshToken = user.RefreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddDays(Convert.ToDouble(configuration["JWT_ACCESS_EXPIRY_MIN"]))
            };

            return response;
        }
    }
}
