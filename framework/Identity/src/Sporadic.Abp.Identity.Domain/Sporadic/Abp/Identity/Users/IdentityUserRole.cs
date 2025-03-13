using System;
using Volo.Abp.Domain.Entities;

namespace Sporadic.Abp.Identity.Users
{
    public class IdentityUserRole : Entity
    {
        /// <summary>
        /// 用户标识 <see cref="IdentityUser"/>"/>
        /// </summary>
        public virtual Guid UserId { get; protected set; }

        /// <summary>
        /// 角色标识 <see cref="IdentityRole"/>
        /// </summary>
        public virtual Guid RoleId { get; protected set; }

        protected IdentityUserRole()
        {

        }

        protected internal IdentityUserRole(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public override object[] GetKeys()
        {
            return [UserId, RoleId];
        }
    }
}
