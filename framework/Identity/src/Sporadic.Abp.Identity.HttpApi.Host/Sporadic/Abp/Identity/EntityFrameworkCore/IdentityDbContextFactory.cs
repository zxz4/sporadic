using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Sporadic.Abp.Identity.EntityFrameworkCore
{
    public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityHostMigrationsDbContext>
    {
        public IdentityHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<IdentityHostMigrationsDbContext>()
                .UseMySql(configuration.GetConnectionString("Default"), MySqlServerVersion.LatestSupportedServerVersion);

            return new IdentityHostMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Sporadic.Abp.Identity.HttpApi.Host/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
