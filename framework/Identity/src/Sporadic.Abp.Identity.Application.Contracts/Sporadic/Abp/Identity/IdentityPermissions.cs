using Volo.Abp.Reflection;

namespace Sporadic.Abp.Identity
{
    public static class IdentityPermissions
    {
        public const string GroupName = "System";

        public static class Roles
        {
            public const string Default = GroupName + ".Roles";
        }

        public static class Users
        {
            public const string Default = GroupName + ".Users";
        }

        public static class OrganizationUnits
        {
            public const string Default = GroupName + ".OrganizationUnits";
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(IdentityPermissions));
        }
    }

}
