using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

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
            if (userManager.FindByEmailAsync(request.Email) is null)
                throw new InvalidOperationException($"{request.Email} adresi zaten kullanılıyor!");

            var user = new AppUser
            {
                UserName = request.FullName,
                Email = request.Email,
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new ValidationException(result.Errors.First().Description);

            await userManager.AddToRoleAsync(user, "User");

            return new RegisterUserCommandResponse()
            {
                UserId = user.Id,
                AccessToken = tokenService.GenerateAccessToken(user, ["User"]),
                ExpiryTime = DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["JWT_ACCESS_EXPIRY_MIN"])),
            };
        }
    }
}
