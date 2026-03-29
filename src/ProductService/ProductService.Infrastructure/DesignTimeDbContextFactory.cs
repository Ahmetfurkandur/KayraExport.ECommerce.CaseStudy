using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProductService.Infrastructure.Contexts;

namespace ProductService.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
    {

        public ProductDbContext CreateDbContext(string[] args)
        {
            // 1. Mevcut dizini al
            string path = Path.Combine(Directory.GetCurrentDirectory(), "../ProductService.API");

            // 2. Yapılandırma oluştur (appsettings + environment variables)
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // 3. DbContextOptionsBuilder oluştur
            var builder = new DbContextOptionsBuilder<ProductDbContext>();


            // GetConnectionString metodu "DefaultConnection" anahtarını arar.
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new ProductDbContext(builder.Options);
        }
    }
}
