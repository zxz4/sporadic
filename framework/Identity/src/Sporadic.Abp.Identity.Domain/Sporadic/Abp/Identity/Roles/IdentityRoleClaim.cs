using JetBrains.Annotations;
using System;
using System.Security.Claims;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Sporadic.Abp.Identity.Roles
{
    public class IdentityRoleClaim : Entity<Guid>
    {
        public virtual Guid RoleId { get; protected set; }

        public virtual string ClaimType { get; protected set; }

        public virtual string ClaimValue { get; protected set; }

        public virtual Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }

        public virtual void SetClaim([NotNull] Claim claim)
        {
            Check.NotNull(claim, nameof(claim));

            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }

        public IdentityRoleClaim(
            Guid id,
            Guid roleId,
            [NotNull] Claim claim)
        {
            Id = id;
            RoleId = roleId;
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }

        public IdentityRoleClaim(
            Guid id,
            Guid roleId,
            [NotNull] string claimType,
            string claimValue)
        {
            Id = id;
            RoleId = roleId;
            ClaimType = claimType;
            ClaimValue = claimValue;
        }
    }
}
