using AuthService.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AuthService.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
    {

        public AuthDbContext CreateDbContext(string[] args)
        {
            // 1. Mevcut dizini al
            string path = Path.Combine(Directory.GetCurrentDirectory(), "../AuthService.API");

            // 2. Yapılandırma oluştur (appsettings + environment variables)
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables() // Docker Compose'dan gelen değerleri okur
                .Build();

            // 3. DbContextOptionsBuilder oluştur
            var builder = new DbContextOptionsBuilder<AuthDbContext>();

            // Docker-compose içindeki isimlendirmeye dikkat: ConnectionStrings__DefaultConnection
            // GetConnectionString metodu "DefaultConnection" anahtarını arar.
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new AuthDbContext(builder.Options);
        }
    }
}
