using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Sporadic.Abp.Identity.Settings;
using System;
using System.Threading.Tasks;
using Volo.Abp.Options;
using Volo.Abp.Settings;

namespace Sporadic.Abp.Identity
{
    /// <summary>
    /// 动态配置IdentityOptions 
    /// https://abp.io/docs/latest/Modules/Setting-Management
    /// </summary>
    public class SporadicIdentityOptionsManager(IOptionsFactory<IdentityOptions> factory,
        ISettingProvider settingProvider) : AbpDynamicOptionsManager<IdentityOptions>(factory)
    {
        protected ISettingProvider SettingProvider { get; } = settingProvider;

        protected override async Task OverrideOptionsAsync(string name, IdentityOptions options)
        {
            options.Password.RequiredLength = await SettingProvider.GetAsync(IdentitySettingNames.Password.RequiredLength, options.Password.RequiredLength);
            options.Password.RequiredUniqueChars = await SettingProvider.GetAsync(IdentitySettingNames.Password.RequiredUniqueChars, options.Password.RequiredUniqueChars);
            options.Password.RequireNonAlphanumeric = await SettingProvider.GetAsync(IdentitySettingNames.Password.RequireNonAlphanumeric, options.Password.RequireNonAlphanumeric);
            options.Password.RequireLowercase = await SettingProvider.GetAsync(IdentitySettingNames.Password.RequireLowercase, options.Password.RequireLowercase);
            options.Password.RequireUppercase = await SettingProvider.GetAsync(IdentitySettingNames.Password.RequireUppercase, options.Password.RequireUppercase);
            options.Password.RequireDigit = await SettingProvider.GetAsync(IdentitySettingNames.Password.RequireDigit, options.Password.RequireDigit);

            options.Lockout.AllowedForNewUsers = await SettingProvider.GetAsync(IdentitySettingNames.Lockout.AllowedForNewUsers, options.Lockout.AllowedForNewUsers);
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(await SettingProvider.GetAsync(IdentitySettingNames.Lockout.LockoutDuration, options.Lockout.DefaultLockoutTimeSpan.TotalSeconds.To<int>()));
            options.Lockout.MaxFailedAccessAttempts = await SettingProvider.GetAsync(IdentitySettingNames.Lockout.MaxFailedAccessAttempts, options.Lockout.MaxFailedAccessAttempts);

            options.SignIn.RequireConfirmedEmail = await SettingProvider.GetAsync(IdentitySettingNames.SignIn.RequireConfirmedEmail, options.SignIn.RequireConfirmedEmail);
            options.SignIn.RequireConfirmedPhoneNumber = await SettingProvider.GetAsync(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber, options.SignIn.RequireConfirmedPhoneNumber);
        }
    }
}
