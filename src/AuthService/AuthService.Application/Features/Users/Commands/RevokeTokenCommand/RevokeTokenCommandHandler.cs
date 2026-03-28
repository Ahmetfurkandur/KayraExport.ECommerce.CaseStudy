using AuthService.Domain.Entities;
using ErrorHandling;
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

            //user nullsa veya refresh token yoksa
            ErrorBuilder.Create(401)
                    .WithTitle("Geçersiz Oturum")
                    .WithDescription("Geçersiz veya süresi dolmuş oturum. Lütfen tekrar giriş yapınız.")
                    .ThrowIf(user is null || string.IsNullOrEmpty(user.RefreshToken));


            user!.RevokeRefreshToken();

            var result = await userManager.UpdateAsync(user);

            //!result.Succeeded durumunda
            ErrorBuilder.Create(401)
                    .WithTitle("Çıkış Başarısız")
                    .WithDescription("Çıkış yapma işlemi başarısız. Lütfen daha sonra tekrar deneyiniz.")
                    .ThrowIf(!result.Succeeded);

            logger.LogInformation(
                    "Refresh token revoked successfully for user {UserId}.",
                    user.Id);

            return new() { Message = "Çıkış işlemi başarılı" };
        }
    }
}
