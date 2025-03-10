using System;
using Volo.Abp.Domain.Entities;

namespace Sporadic.Abp.Identity.Users
{
    public class IdentityUserRole : Entity
    {
        public virtual Guid UserId { get; protected set; }

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
