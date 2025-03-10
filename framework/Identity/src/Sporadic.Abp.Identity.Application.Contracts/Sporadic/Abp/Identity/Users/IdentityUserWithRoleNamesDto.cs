namespace Sporadic.Abp.Identity.Users
{
    public class IdentityUserWithRoleNamesDto
    {
        public IdentityUserDto User { get;set;}

        public string[] RoleNames { get; set; }
    }
}
