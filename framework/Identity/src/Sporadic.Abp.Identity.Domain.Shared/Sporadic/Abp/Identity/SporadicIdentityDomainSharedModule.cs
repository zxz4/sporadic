using Sporadic.Abp.Identity.Localization;
using Sporadic.Abp.Users;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Sporadic.Abp.Identity
{
    [DependsOn(
        typeof(SporadicUsersDomainSharedModule),
        typeof(AbpValidationModule)
    )]
    public class SporadicIdentityDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<SporadicIdentityDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<IdentityResource>("zh-Hans")
                    .AddBaseTypes(
                        typeof(AbpValidationResource)
                    ).AddVirtualJson("/Sporadic/Abp/Identity/Localization");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Sporadic.Abp.Identity", typeof(IdentityResource));
            });
        }
    }
}
