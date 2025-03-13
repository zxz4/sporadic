using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;

namespace Sporadic.Abp.Identity.Users
{
    public class IdentityUserManager(
        IIdentityUserRepository identityUserRepository,
        IdentityUserStore store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<IdentityUser> passwordHasher,
        IEnumerable<IUserValidator<IdentityUser>> userValidators,
        IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators,
        ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<IdentityUser>> logger) : UserManager<IdentityUser>(
              store,
              optionsAccessor,
              passwordHasher,
              userValidators,
              passwordValidators,
              keyNormalizer,
              errors,
              services,
              logger), IDomainService
    {
        public const string ConfirmPhoneNumberTokenPurpose = "PhoneNumberConfirmation";

        public const string SignInWithPhoneTokenPurpose = "PhoneCodeSignIn";

        protected IIdentityUserRepository IdentityUserRepository { get; } = identityUserRepository;

        public virtual async Task<IdentityResult> CreateAsync(IdentityUser user, string password, bool validatePassword)
        {
            var result = await UpdatePasswordHash(user, password, validatePassword);
            if (!result.Succeeded)
            {
                return result;
            }

            return await CreateAsync(user);
        }


        /// <summary>
        /// 确认手机号码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual async Task<IdentityResult> ConfirmPhoneNumberAsync(IdentityUser user, string token)
        {
            ThrowIfDisposed();

            Check.NotNull(user, nameof(user));

            if (!await VerifyUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, ConfirmPhoneNumberTokenPurpose, token).ConfigureAwait(false))
            {
                return IdentityResult.Failed(ErrorDescriber.InvalidToken());
            }
            await ((IdentityUserStore)Store).SetPhoneNumberConfirmedAsync(user, true, CancellationToken).ConfigureAwait(false);
          
            return await base.UpdateAsync(user);
        }

        /// <summary>
        /// 生成确认手机短信验证码
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual async Task<string> GeneratePhoneNumberConfirmationTokenAsync(IdentityUser user)
        {
            ThrowIfDisposed();

            Check.NotNull(user, nameof(user));

            return await GenerateUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, ConfirmPhoneNumberTokenPurpose);
        }

        /// <summary>
        /// 生成手机登录验证码
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual async Task<string> GeneratePhoneSignInTokenAsync(IdentityUser user)
        {
            ThrowIfDisposed();

            Check.NotNull(user, nameof(user));

            return await GenerateUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, SignInWithPhoneTokenPurpose);
        }

        /// <summary>
        /// 验证手机登录验证码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual async Task<bool> VertifyPhoneSignInToken(IdentityUser user,string token)
        {
            var result = await VerifyUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, SignInWithPhoneTokenPurpose, token);

            if (result && !user.PhoneNumberConfirmed)
            {
                await ((IdentityUserStore)Store).SetPhoneNumberConfirmedAsync(user, true, CancellationToken).ConfigureAwait(false);

                await base.UpdateAsync(user);
            }

            return result; 
        }


        public async override Task<IdentityResult> DeleteAsync(IdentityUser user)
        {
            return await base.DeleteAsync(user);
        }

        protected async override Task<IdentityResult> UpdateUserAsync(IdentityUser user)
        {
            var result = await base.UpdateUserAsync(user);

            return result;
        }

        public virtual async Task<IdentityUser> GetByIdAsync(Guid id)
        {
            var user = await Store.FindByIdAsync(id.ToString(), CancellationToken);

            return user ?? throw new EntityNotFoundException(typeof(IdentityUser), id);
        }


        public virtual async Task<IdentityUser> FindByPhoneNumberAsync(string phoneNumber)
        {
            ThrowIfDisposed();

            Check.NotNull(phoneNumber, nameof(phoneNumber));

            return await IdentityUserRepository.FindByPhoneNumberAsync(phoneNumber, cancellationToken:CancellationToken);
        }

        public virtual async Task<IdentityResult> SetRolesAsync(
            [NotNull] IdentityUser user, 
            [NotNull] IEnumerable<string> roleNames)
        {
            Check.NotNull(user, nameof(user));
            Check.NotNull(roleNames, nameof(roleNames));

            var currentRoleNames = await GetRolesAsync(user);

            var result = await RemoveFromRolesAsync(user, currentRoleNames.Except(roleNames).Distinct());
            if (!result.Succeeded)
            {
                return result;
            }

            result = await AddToRolesAsync(user, roleNames.Except(currentRoleNames).Distinct());
            if (!result.Succeeded)
            {
                return result;
            }

            return IdentityResult.Success;
        }
    }
}
