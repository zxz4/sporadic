using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Sporadic.Abp.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Sporadic.Abp.Identity.Users
{
    public class IdentityUser : BasicAggregateRoot<Guid>, IUser, IHasConcurrencyStamp
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; protected internal set; }

        /// <summary>
        /// 标识是否已确认邮箱
        /// </summary>
        public bool EmailConfirmed { get; protected internal set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; protected internal set; }

        /// <summary>
        /// 标识是否已确认电话号码
        /// </summary>
        public bool PhoneNumberConfirmed { get; protected internal set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; protected internal set; }

        /// <summary>
        /// 密码哈希
        /// </summary>
        [DisableAuditing]
        public virtual string PasswordHash { get; protected internal set; }

        /// <summary>
        /// 指示用户是否应该在下次登录时更改密码
        /// </summary>
        public virtual bool ShouldChangePasswordOnNextLogin { get; set; }


        /// <summary>
        /// 安全戳
        /// </summary>
        public string SecurityStamp { get; internal set; }

        /// <summary>
        /// 标记是否是外部用户
        /// </summary>
        public bool IsExternal { get; set; }

        /// <summary>
        /// 锁定结束时间
        /// </summary>
        public virtual DateTimeOffset? LockoutEnd { get; protected internal set; }

        /// <summary>
        /// 是否启用账户锁定
        /// </summary>
        public virtual bool LockoutEnabled { get; protected internal set; }

        /// <summary>
        /// 尝试失败次数
        /// </summary>
        public virtual int AccessFailedCount { get; protected internal set; }


        public virtual ICollection<IdentityUserLogin> Logins { get; protected set; }

        public virtual ICollection<IdentityUserRole> Roles { get; protected set; }

        public virtual ICollection<IdentityUserOrganizationUnit> OrganizationUnits { get; protected set; }

        /// <summary>
        /// 使用手机号，用户名，邮箱创建用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="email"></param>
        /// <param name="userName"></param>
        public IdentityUser(
            Guid id,
            [CanBeNull] string phoneNumber,
            [CanBeNull] string email,
            [CanBeNull] string userName)
            : base(id)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) && 
                string.IsNullOrWhiteSpace(email) &&
                string.IsNullOrWhiteSpace(userName))
            {
                Check.NotNull(userName, nameof(userName));
                Check.NotNull(email, nameof(email));
                Check.NotNull(phoneNumber, nameof(phoneNumber));
            }
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;

            ConcurrencyStamp = Guid.NewGuid().ToString("N");
            SecurityStamp = Guid.NewGuid().ToString();
            IsActive = true;

            Roles = new Collection<IdentityUserRole>();
            Logins = new Collection<IdentityUserLogin>();
            OrganizationUnits = new Collection<IdentityUserOrganizationUnit>();
        }

        public string ConcurrencyStamp { get; set; }

        protected IdentityUser()
        {
        }

        public virtual void AddLogin([NotNull] UserLoginInfo login)
        {
            Check.NotNull(login, nameof(login));

            Logins.Add(new IdentityUserLogin(Id, login));
        }

        public void RemoveLogin(string loginProvider, string providerKey)
        {
            Check.NotNull(loginProvider, nameof(loginProvider));
            Check.NotNull(providerKey, nameof(providerKey));

            Logins.RemoveAll(userLogin =>
                userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey);
        }

        public virtual void AddRole(Guid roleId)
        {
            Check.NotNull(roleId, nameof(roleId));

            if (IsInRole(roleId))
            {
                return;
            }

            Roles.Add(new IdentityUserRole(Id, roleId));
        }

        public virtual void RemoveRole(Guid roleId)
        {
            Check.NotNull(roleId, nameof(roleId));

            if (!IsInRole(roleId))
            {
                return;
            }

            Roles.RemoveAll(r => r.RoleId == roleId);
        }

        public virtual bool IsInRole(Guid roleId)
        {
            Check.NotNull(roleId, nameof(roleId));

            return Roles.Any(r => r.RoleId == roleId);
        }

        /// <summary>
        /// 设置下次登录时是否需要更改密码
        /// </summary>
        /// <param name="shouldChangePasswordOnNextLogin"></param>
        public virtual void SetShouldChangePasswordOnNextLogin(bool shouldChangePasswordOnNextLogin)
        {
            ShouldChangePasswordOnNextLogin = shouldChangePasswordOnNextLogin;
        }
    }
}
