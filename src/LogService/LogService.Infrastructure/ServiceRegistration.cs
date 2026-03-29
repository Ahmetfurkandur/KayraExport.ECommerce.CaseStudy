using LogService.Application.Interfaces;
using LogService.Infrastructure.Repositories.Seq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LogService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<ILogQueryRepository, SeqLogQueryRepository>(client =>
            {
                client.BaseAddress = new Uri(configuration["Seq:ServerUrl"]!);
            });

            services.AddScoped<ILogQueryRepository, SeqLogQueryRepository>();
        }
    }
}
