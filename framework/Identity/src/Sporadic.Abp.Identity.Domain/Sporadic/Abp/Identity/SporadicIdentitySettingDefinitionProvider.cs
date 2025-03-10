using Sporadic.Abp.Identity.Localization;
using Sporadic.Abp.Identity.Settings;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Sporadic.Abp.Identity
{
    public class SporadicIdentitySettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    IdentitySettingNames.Password.RequiredLength,
                    6.ToString(),
                    L("DisplayName:Sporadic.Identity.Password.RequiredLength"),
                    L("Description:Sporadic.Identity.Password.RequiredLength"),
                    true),

                new SettingDefinition(
                    IdentitySettingNames.Password.RequiredUniqueChars,
                    0.ToString(),
                    L("DisplayName:Sporadic.Identity.Password.RequiredUniqueChars"),
                    L("Description:Sporadic.Identity.Password.RequiredUniqueChars"),
                    true),

                new SettingDefinition(
                    IdentitySettingNames.Password.RequireNonAlphanumeric,
                    false.ToString(),
                    L("DisplayName:Sporadic.Identity.Password.RequireNonAlphanumeric"),
                    L("Description:Sporadic.Identity.Password.RequireNonAlphanumeric"),
                    true),

                new SettingDefinition(
                    IdentitySettingNames.Password.RequireLowercase,
                    false.ToString(),
                    L("DisplayName:Sporadic.Identity.Password.RequireLowercase"),
                    L("Description:Sporadic.Identity.Password.RequireLowercase"),
                    true),

                new SettingDefinition(
                    IdentitySettingNames.Password.RequireUppercase,
                    false.ToString(),
                    L("DisplayName:Sporadic.Identity.Password.RequireUppercase"),
                    L("Description:Sporadic.Identity.Password.RequireUppercase"),
                    true),

                new SettingDefinition(
                    IdentitySettingNames.Password.RequireDigit,
                    false.ToString(),
                    L("DisplayName:Sporadic.Identity.Password.RequireDigit"),
                    L("Description:Sporadic.Identity.Password.RequireDigit"),
                    true),

                new SettingDefinition(
                    IdentitySettingNames.Password.ForceUsersToPeriodicallyChangePassword,
                    false.ToString(),
                    L("DisplayName:Sporadic.Identity.Password.ForceUsersToPeriodicallyChangePassword"),
                    L("Description:Sporadic.Identity.Password.ForceUsersToPeriodicallyChangePassword"),
                    true),

                new SettingDefinition(
                    IdentitySettingNames.Password.PasswordChangePeriodDays,
                    0.ToString(),
                    L("DisplayName:Sporadic.Identity.Password.PasswordChangePeriodDays"),
                    L("Description:Sporadic.Identity.Password.PasswordChangePeriodDays"),
                    true),

                new SettingDefinition(
                    IdentitySettingNames.Lockout.AllowedForNewUsers,
                    true.ToString(),
                    L("DisplayName:Sporadic.Identity.Lockout.AllowedForNewUsers"),
                    L("Description:Sporadic.Identity.Lockout.AllowedForNewUsers"),
                    true),

                new SettingDefinition(
                    IdentitySettingNames.Lockout.LockoutDuration,
                    (5 * 60).ToString(),
                    L("DisplayName:Sporadic.Identity.Lockout.LockoutDuration"),
                    L("Description:Sporadic.Identity.Lockout.LockoutDuration"),
                    true),

                new SettingDefinition(
                    IdentitySettingNames.Lockout.MaxFailedAccessAttempts,
                    5.ToString(),
                    L("DisplayName:Sporadic.Identity.Lockout.MaxFailedAccessAttempts"),
                    L("Description:Sporadic.Identity.Lockout.MaxFailedAccessAttempts"),
                    true),

                new SettingDefinition(
                    IdentitySettingNames.SignIn.RequireConfirmedEmail,
                    false.ToString(),
                    L("DisplayName:Sporadic.Identity.SignIn.RequireConfirmedEmail"),
                    L("Description:Sporadic.Identity.SignIn.RequireConfirmedEmail"),
                    true),
                new SettingDefinition(
                    IdentitySettingNames.SignIn.EnablePhoneNumberConfirmation,
                    true.ToString(),
                    L("DisplayName:Sporadic.Identity.SignIn.EnablePhoneNumberConfirmation"),
                    L("Description:Sporadic.Identity.SignIn.EnablePhoneNumberConfirmation"),
                    true),
                new SettingDefinition(
                    IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber,
                    false.ToString(),
                    L("DisplayName:Sporadic.Identity.SignIn.RequireConfirmedPhoneNumber"),
                    L("Description:Sporadic.Identity.SignIn.RequireConfirmedPhoneNumber"),
                    true),

                new SettingDefinition(
                    IdentitySettingNames.User.IsUserNameUpdateEnabled,
                    true.ToString(),
                    L("DisplayName:Sporadic.Identity.User.IsUserNameUpdateEnabled"),
                    L("Description:Sporadic.Identity.User.IsUserNameUpdateEnabled"),
                    true),

                new SettingDefinition(
                    IdentitySettingNames.User.IsEmailUpdateEnabled,
                    true.ToString(),
                    L("DisplayName:Sporadic.Identity.User.IsEmailUpdateEnabled"),
                    L("Description:Sporadic.Identity.User.IsEmailUpdateEnabled"),
                    true)
            );
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<IdentityResource>(name);
        }
    }
}
