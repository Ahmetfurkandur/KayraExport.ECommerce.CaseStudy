using AuthService.Application.Interfaces;
using AuthService.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AuthService.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
