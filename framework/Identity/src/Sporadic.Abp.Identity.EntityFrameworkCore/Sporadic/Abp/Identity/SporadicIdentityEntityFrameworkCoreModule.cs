using Microsoft.Extensions.DependencyInjection;
using Sporadic.Abp.Identity.OrganizationUnits;
using Sporadic.Abp.Identity.Roles;
using Sporadic.Abp.Identity.Users;
using Sporadic.Abp.Users;
using Volo.Abp.Modularity;

namespace Sporadic.Abp.Identity
{
    [DependsOn(
        typeof(SporadicIdentityDomainModule),
        typeof(SporadicUsersEntityFrameworkCoreModule))]
    public class SporadicIdentityEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<IdentityDbContext>(options =>
            {
                options.AddRepository<IdentityUser, EfCoreIdentityUserRepository>();
                options.AddRepository<IdentityRole, EfCoreIdentityRoleRepository>();
                //options.AddRepository<IdentityClaimType, EfCoreIdentityClaimTypeRepository>();
                options.AddRepository<OrganizationUnit, EfCoreOrganizationUnitRepository>();
            });
        }
    }
}
