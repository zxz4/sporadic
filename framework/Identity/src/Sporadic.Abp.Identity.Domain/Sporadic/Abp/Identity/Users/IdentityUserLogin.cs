using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Sporadic.Abp.Identity.Users
{
    public class IdentityUserLogin : Entity
    {
        /// <summary>
        /// 用户标识 <see cref="IdentityUser"/>
        /// </summary>
        public virtual Guid UserId { get; protected set; }

        /// <summary>
        /// 登录提供程序
        /// </summary>
        public virtual string LoginProvider { get; protected set; }

        /// <summary>
        /// 登录提供程序密钥
        /// </summary>
        public virtual string ProviderKey { get; protected set; }

        /// <summary>
        /// 登录提供程序显示名称
        /// </summary>
        public virtual string ProviderDisplayName { get; protected set; }

        protected IdentityUserLogin()
        {

        }

        protected internal IdentityUserLogin(
            Guid userId,
            [NotNull] string loginProvider,
            [NotNull] string providerKey,
            string providerDisplayName)
        {
            Check.NotNull(loginProvider, nameof(loginProvider));
            Check.NotNull(providerKey, nameof(providerKey));

            UserId = userId;
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
            ProviderDisplayName = providerDisplayName;
        }


        protected internal IdentityUserLogin(
            Guid userId,
            [NotNull] UserLoginInfo login)
            : this(
                  userId,
                  login.LoginProvider,
                  login.ProviderKey,
                  login.ProviderDisplayName)
        {
        }


        public virtual UserLoginInfo ToUserLoginInfo()
        {
            return new UserLoginInfo(LoginProvider, ProviderKey, ProviderDisplayName);
        }

        public override object[] GetKeys()
        {
            return [UserId, LoginProvider];
        }
    }
}
