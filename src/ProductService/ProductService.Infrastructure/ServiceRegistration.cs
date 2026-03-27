using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Interfaces.Repositories.Products;
using ProductService.Infrastructure.Contexts;
using ProductService.Infrastructure.Repositories.Products;

namespace ProductService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProductDbContext>(c => c.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IProductCommandRepository, ProductCommandRepository>();
            services.AddScoped<IProductQueryRepository, ProductQueryRepository>();
        }
    }
}
