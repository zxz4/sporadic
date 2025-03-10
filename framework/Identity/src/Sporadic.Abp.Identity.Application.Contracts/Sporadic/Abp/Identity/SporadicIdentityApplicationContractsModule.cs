using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace Sporadic.Abp.Identity
{
    [DependsOn(
        typeof(SporadicIdentityDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpPermissionManagementApplicationContractsModule))]
    public class SporadicIdentityApplicationContractsModule : AbpModule
    {
    }
}
