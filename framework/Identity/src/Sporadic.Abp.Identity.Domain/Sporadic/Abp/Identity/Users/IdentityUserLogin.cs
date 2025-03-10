﻿using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Sporadic.Abp.Identity.Users
{
    public class IdentityUserLogin : Entity
    {
        /// <summary>
        /// Gets or sets the of the primary key of the user associated with this login.
        /// </summary>
        public virtual Guid UserId { get; protected set; }

        /// <summary>
        /// Gets or sets the login provider for the login (e.g. facebook, google)
        /// </summary>
        public virtual string LoginProvider { get; protected set; }

        /// <summary>
        /// Gets or sets the unique provider identifier for this login.
        /// </summary>
        public virtual string ProviderKey { get; protected set; }

        /// <summary>
        /// Gets or sets the friendly name used in a UI for this login.
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
