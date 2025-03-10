namespace Sporadic.Abp.Identity.Roles
{
    public class IdentityRoleWithUserCount
    {
        public IdentityRole Role { get; set; }

        public int UserCount { get; set; }

        public IdentityRoleWithUserCount(IdentityRole role, int userCount)
        {
            Role = role;
            UserCount = userCount;
        }
    }
}
