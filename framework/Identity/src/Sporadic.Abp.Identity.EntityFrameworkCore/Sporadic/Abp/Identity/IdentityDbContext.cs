using Microsoft.EntityFrameworkCore;
using Sporadic.Abp.Identity.OrganizationUnits;
using Sporadic.Abp.Identity.Roles;
using Sporadic.Abp.Identity.Users;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Sporadic.Abp.Identity
{
    [ConnectionStringName(SporadicIdentityDbProperties.ConnectionStringName)]

    public class IdentityDbContext : AbpDbContext<IdentityDbContext>, IIdentityDbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }

        public DbSet<IdentityUser> Users { get; set; }

        public DbSet<IdentityRole> Roles { get; set; }

        public DbSet<OrganizationUnit> OrganizationUnits { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureIdentity();
        }
    }
}
