using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Sporadic.Abp.Identity.Users;
using Volo.Abp.Modularity;

namespace Sporadic.Abp.Identity.AspNetCore;

[DependsOn( typeof(SporadicIdentityDomainModule))]
public class SporadicIdentityAspNetCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IdentityBuilder>(builder =>
        {
            builder
                .AddDefaultTokenProviders()
                .AddSignInManager<SporadicSignInManager>()
                .AddUserValidator<IdentityUserValidator>();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var options = context.Services.ExecutePreConfiguredActions(new SporadicIdentityAspNetCoreOptions());

        if (options.ConfigureAuthentication)
        {
            context.Services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = IdentityConstants.ApplicationScheme;
                    o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies();
        }
    }
}
