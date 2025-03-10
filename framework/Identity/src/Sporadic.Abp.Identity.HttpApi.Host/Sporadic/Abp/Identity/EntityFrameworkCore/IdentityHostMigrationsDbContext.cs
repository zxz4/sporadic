using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;

namespace Sporadic.Abp.Identity.EntityFrameworkCore
{
    public class IdentityHostMigrationsDbContext : AbpDbContext<IdentityHostMigrationsDbContext>
    {
        public IdentityHostMigrationsDbContext(DbContextOptions<IdentityHostMigrationsDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigurePermissionManagement();
            modelBuilder.ConfigureIdentity();
        }

    }
}
