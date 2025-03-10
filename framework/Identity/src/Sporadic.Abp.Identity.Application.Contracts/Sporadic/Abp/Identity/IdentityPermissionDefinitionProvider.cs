using Sporadic.Abp.Identity.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Sporadic.Abp.Identity
{
    public class IdentityPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var identityGroup = context.AddGroup(IdentityPermissions.GroupName, L("Permission:IdentityManagement"));

            var rolesPermission = identityGroup.AddPermission(IdentityPermissions.Roles.Default, L("Permission:RoleManagement"));

            var usersPermission = identityGroup.AddPermission(IdentityPermissions.Users.Default, L("Permission:UserManagement"));

            var organizationUnitsPermission = identityGroup.AddPermission(IdentityPermissions.OrganizationUnits.Default, L("Permission:OrganizationUnitManagement"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<IdentityResource>(name);
        }
    }

}
