using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Sporadic.Abp.Users
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule),
        typeof(SporadicUsersDomainModule))]
    public class SporadicUsersEntityFrameworkCoreModule : AbpModule
    {
    }
}
