using System;
using Volo.Abp.Domain.Entities;

namespace Sporadic.Abp.Identity.Users
{
    public class IdentityUserOrganizationUnit : Entity
    {
        /// <summary>
        /// 用户标识 <see cref="IdentityUser"/>.
        /// </summary>
        public virtual Guid UserId { get; protected set; }

        /// <summary>
        /// 机构标识 <see cref="OrganizationUnits.OrganizationUnit"/>.
        /// </summary>
        public virtual Guid OrganizationUnitId { get; protected set; }

        protected IdentityUserOrganizationUnit()
        {

        }

        public IdentityUserOrganizationUnit(Guid userId, Guid organizationUnitId)
        {
            UserId = userId;
            OrganizationUnitId = organizationUnitId;
        }

        public override object[] GetKeys()
        {
            return [UserId, OrganizationUnitId];
        }
    }
}
