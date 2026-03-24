using AuthService.Domain.Entities;

namespace AuthService.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(AppUser user, List<string> roles);
        string GenerateRefreshToken();
    }
}
