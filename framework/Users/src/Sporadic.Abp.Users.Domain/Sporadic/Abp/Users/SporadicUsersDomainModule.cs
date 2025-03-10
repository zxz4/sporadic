using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Sporadic.Abp.Users
{
    [DependsOn(
        typeof(SporadicUsersDomainSharedModule),
        typeof(AbpDddDomainModule)
    )]
    public class SporadicUsersDomainModule : AbpModule
    {
    }
}
