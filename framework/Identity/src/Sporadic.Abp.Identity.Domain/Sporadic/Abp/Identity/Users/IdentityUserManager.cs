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
    public class IdentityUserManager : UserManager<IdentityUser>, IDomainService
    {
        protected IIdentityUserRepository IdentityUserRepository { get; }
        public IdentityUserManager(
            IIdentityUserRepository identityUserRepository,
            IdentityUserStore store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<IdentityUser> passwordHasher,
            IEnumerable<IUserValidator<IdentityUser>> userValidators,
            IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            IServiceProvider services, 
            ILogger<UserManager<IdentityUser>> logger)
            : base(
                  store,
                  optionsAccessor,
                  passwordHasher,
                  userValidators,
                  passwordValidators,
                  keyNormalizer,
                  errors,
                  services,
                  logger)
        {
            IdentityUserRepository = identityUserRepository;
        }



        public virtual async Task<IdentityResult> CreateAsync(IdentityUser user, string password, bool validatePassword)
        {
            var result = await UpdatePasswordHash(user, password, validatePassword);
            if (!result.Succeeded)
            {
                return result;
            }

            return await CreateAsync(user);
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

            return await IdentityUserRepository.FindByPhoneNumberAsync(phoneNumber, CancellationToken);
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
