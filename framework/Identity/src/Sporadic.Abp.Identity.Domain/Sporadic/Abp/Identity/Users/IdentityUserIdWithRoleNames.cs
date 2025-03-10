using System;

namespace Sporadic.Abp.Identity.Users
{
    public class IdentityUserIdWithRoleNames
    {
        public Guid Id { get; set; }

        public string[] RoleNames { get; set; }
    }
}
