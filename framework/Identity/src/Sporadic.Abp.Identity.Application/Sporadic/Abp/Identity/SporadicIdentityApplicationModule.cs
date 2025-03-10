using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace Sporadic.Abp.Identity
{
    [DependsOn(
        typeof(SporadicIdentityDomainModule),
        typeof(SporadicIdentityApplicationContractsModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpPermissionManagementApplicationModule)
        )]
    public class SporadicIdentityApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<SporadicIdentityApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<SporadicIdentityApplicationModuleAutoMapperProfile>(validate: true);
            });
        }
    }
}
