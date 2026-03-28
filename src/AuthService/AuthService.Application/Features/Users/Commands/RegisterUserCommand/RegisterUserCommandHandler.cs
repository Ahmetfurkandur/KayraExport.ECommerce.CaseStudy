using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using ErrorHandling;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace AuthService.Application.Features.Users.Commands.RegisterUserCommand
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommandRequest, RegisterUserCommandResponse>
    {
        private readonly ITokenService tokenService;
        private readonly UserManager<AppUser> userManager;
        private readonly IConfiguration configuration;

        public RegisterUserCommandHandler(ITokenService tokenService, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            this.tokenService = tokenService;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public async Task<RegisterUserCommandResponse> Handle(RegisterUserCommandRequest request, CancellationToken cancellationToken)
        {

            ErrorBuilder.Create(409)
                .WithTitle("Mail Zaten Kullanılıyor")
                .WithDescription($"{request.Email} adresi zaten kullanılıyor!")
                .ThrowIfNotNull(await userManager.FindByEmailAsync(request.Email));

            ErrorBuilder.Create(409)
                .WithTitle("Kullanıcı Adı Zaten Kullanılıyor")
                .WithDescription($"{request.FullName} kullanıcı adı zaten kullanılıyor!")
                .ThrowIfNotNull(await userManager.FindByNameAsync(request.FullName));


            var user = new AppUser
            {
                UserName = request.FullName,
                Email = request.Email,
            };

            var result = await userManager.CreateAsync(user, request.Password);

            ErrorBuilder.Create(400)
                .WithTitle("Hatalı Girdi")
                .WithDescription(result.Errors.First().Description)
                .ThrowIf(!result.Succeeded);

            await userManager.AddToRoleAsync(user, "USER");

            return new RegisterUserCommandResponse()
            {
                UserId = user.Id,
                AccessToken = tokenService.GenerateAccessToken(user, ["User"]),
                ExpiryTime = DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["JWT_ACCESS_EXPIRY_MIN"])),
            };
        }
    }
}
