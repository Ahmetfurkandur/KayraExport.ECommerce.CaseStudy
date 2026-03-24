using Microsoft.AspNetCore.Identity;

namespace AuthService.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        //Refresh token mekanizmasında kullanmak için gerekli alanları tanımlıyoruz.
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
    }
}
