using ErrorHandling.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ErrorHandling
{
    public static class ServiceRegistration
    {
        public static void AddExceptionMiddleware(this IServiceCollection services)
        {
            services.AddSingleton<ExceptionMiddleware>();
        }

        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
