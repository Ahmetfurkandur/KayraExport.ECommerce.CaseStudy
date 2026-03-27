using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ProductService.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        }
    }
}
