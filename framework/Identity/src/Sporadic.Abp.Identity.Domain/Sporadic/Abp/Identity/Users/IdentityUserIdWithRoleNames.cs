using System;

namespace Sporadic.Abp.Identity.Users
{
    public class IdentityUserIdWithRoleNames
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string[] RoleNames { get; set; }
    }
}
