using AuthService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.Features.Users.Commands.RevokeTokenCommand
{
    public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommandRequest, RevokeTokenCommandResponse>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ILogger<RevokeTokenCommandHandler> logger;

        public RevokeTokenCommandHandler(UserManager<AppUser> userManager, ILogger<RevokeTokenCommandHandler> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        public async Task<RevokeTokenCommandResponse> Handle(RevokeTokenCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken && request.RefreshTokenExpiry > DateTime.UtcNow, cancellationToken);

            if (user == null)
            {
                throw new KeyNotFoundException("Geçersiz veya süresi dolmuş oturum. Lütfen tekrar giriş yapınız.");
            }

            user.RevokeRefreshToken();

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Çıkış yapma işlemi başarısız. Lütfen daha sonra tekrar deneyiniz.");
            }

            logger.LogInformation(
                    "Refresh token revoked successfully for user {UserId}.",
                    user.Id);

            return new() { Message = "Çıkış işlemi başarılı" };
        }
    }
}
