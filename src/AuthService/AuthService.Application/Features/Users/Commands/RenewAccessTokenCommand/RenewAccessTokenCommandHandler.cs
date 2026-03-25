using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Application.Features.Users.Commands.RenewAccessTokenCommand
{
    public class RenewAccessTokenCommandHandler : IRequestHandler<RenewAccessTokenCommandRequest, RenewAccessTokenCommandResponse>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenService tokenService;
        private readonly ILogger<RenewAccessTokenCommandHandler> logger;
        private readonly IConfiguration configuration;


        public RenewAccessTokenCommandHandler(UserManager<AppUser> userManager, ILogger<RenewAccessTokenCommandHandler> logger, ITokenService tokenService)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.tokenService = tokenService;
        }
        public async Task<RenewAccessTokenCommandResponse> Handle(RenewAccessTokenCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken && request.RefreshTokenExpiry > DateTime.UtcNow, cancellationToken: cancellationToken);

            if (user is null || !string.IsNullOrEmpty(user.RefreshToken) || request.RefreshTokenExpiry < DateTime.UtcNow)
            {
                throw new SecurityTokenException("Geçersiz veya süresi dolmuş oturum. Lütfen tekrar giriş yapınız.");
            }

            var roles = await userManager.GetRolesAsync(user);

            var accessToken = tokenService.GenerateAccessToken(user, roles.ToList());
            var refreshToken = tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(Convert.ToDouble(configuration["JWT_REFRESH_EXPIRY_DAYS"]));

            await userManager.UpdateAsync(user);

            return new RenewAccessTokenCommandResponse
            {
                AccessToken = accessToken,
                AccessTokenExpiry = DateTime.UtcNow.AddDays(Convert.ToDouble(configuration["JWT_ACCESS_EXPIRY_MIN"]))
            };
        }
    }
}
