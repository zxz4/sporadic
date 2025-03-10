using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Sporadic.Abp.Identity.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Sporadic.Abp.Identity
{
    [DependsOn(
        typeof(SporadicIdentityApplicationContractsModule), 
        typeof(AbpAspNetCoreMvcModule))]
    public class SporadicIdentityHttpApiModule:AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(SporadicIdentityHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<IdentityResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );
            });
        }
    }
}
