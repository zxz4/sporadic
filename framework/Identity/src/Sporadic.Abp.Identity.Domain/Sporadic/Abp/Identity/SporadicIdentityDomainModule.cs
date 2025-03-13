using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Sporadic.Abp.Users;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;

namespace Sporadic.Abp.Identity
{

    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(SporadicIdentityDomainSharedModule),
        typeof(SporadicUsersDomainModule)
        )]
    public class SporadicIdentityDomainModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AbpClaimsPrincipalFactoryOptions>(options =>
            {
                options.RemoteRefreshUrl = "/api/account/dynamic-claims/refresh";
                options.IsRemoteRefreshEnabled = false;
            });
        }
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var identityBuilder = context.Services.AddSporadicIdentity();

            context.Services.AddObjectAccessor(identityBuilder);
            context.Services.ExecutePreConfiguredActions(identityBuilder);

            Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserIdClaimType = AbpClaimTypes.UserId;
                options.ClaimsIdentity.UserNameClaimType = AbpClaimTypes.UserName;
                options.ClaimsIdentity.RoleClaimType = AbpClaimTypes.Role;
                options.ClaimsIdentity.EmailClaimType = AbpClaimTypes.Email;
            });

            context.Services.AddAbpDynamicOptions<IdentityOptions, SporadicIdentityOptionsManager>();
        }
    }
}
