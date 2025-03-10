using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;

namespace Sporadic.Abp.Identity.Roles
{
    public class IdentityRole : Entity<Guid> , IHasConcurrencyStamp
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public virtual string Name { get; protected internal set; }

        /// <summary>
        /// Gets or sets the normalized name for this role.
        /// </summary>
        [DisableAuditing]
        public virtual string NormalizedName { get; protected internal set; }

        /// <summary>
        /// 默认角色自动被添加到新用户
        /// </summary>
        public virtual bool IsDefault { get; set; }

        /// <summary>
        /// 静态角色不能被修改
        /// </summary>
        public virtual bool IsStatic { get; set; }

        /// <summary>
        /// 非公共角色无法被其他看见
        /// </summary>
        public virtual bool IsPublic { get; set; }


        public string ConcurrencyStamp { get; set; }

        public virtual ICollection<IdentityRoleClaim> Claims { get; protected set; }



        protected IdentityRole() { }

        public IdentityRole(Guid id, [NotNull] string name)
        {
            Check.NotNull(name, nameof(name));
            Id = id;
            Name = name;
            NormalizedName = name.ToUpperInvariant();
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
            Claims = new Collection<IdentityRoleClaim>();
        }

        public virtual void AddClaim([NotNull] IGuidGenerator guidGenerator, [NotNull] Claim claim)
        {
            Check.NotNull(guidGenerator, nameof(guidGenerator));

            Check.NotNull(claim, nameof(claim));

            Claims.Add(new IdentityRoleClaim(guidGenerator.Create(), Id, claim));
        }

        public virtual void RemoveClaim([NotNull] Claim claim)
        {
            Check.NotNull(claim, nameof(claim));

            Claims.RemoveAll(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value);
        }


    }
}
