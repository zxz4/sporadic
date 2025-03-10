using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Sporadic.Abp.Identity.OrganizationUnits;
using Sporadic.Abp.Identity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;
using Volo.Abp.Security.Claims;

namespace Sporadic.Abp.Identity.Roles
{
    public class IdentityRoleManager : RoleManager<IdentityRole>, IDomainService
    {
        protected IIdentityUserRepository UserRepository { get; }

        protected IOrganizationUnitRepository OrganizationUnitRepository { get; }

        protected OrganizationUnitManager OrganizationUnitManager { get; }

        protected IDistributedCache<AbpDynamicClaimCacheItem> DynamicClaimCache { get; }

        public IdentityRoleManager(
            IdentityRoleStore store,
            IEnumerable<IRoleValidator<IdentityRole>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<IdentityRole>> logger,
            IIdentityUserRepository userRepository,
            IDistributedCache<AbpDynamicClaimCacheItem> dynamicClaimCache) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
            UserRepository = userRepository;
            DynamicClaimCache = dynamicClaimCache;
        }


        public virtual async Task<IdentityRole> GetByIdAsync(Guid id)
        {
            var role = await Store.FindByIdAsync(id.ToString(), CancellationToken);

            return role ?? throw new EntityNotFoundException(typeof(IdentityRole), id);
        }

        public async override Task<IdentityResult> SetRoleNameAsync(IdentityRole role, string name)
        {
            if (role.IsStatic)
            {
                throw new BusinessException(IdentityErrorCodes.StaticRoleRenaming);
            }

            var result = await base.SetRoleNameAsync(role, name);

            if (result.Succeeded)
            {
                Logger.LogDebug($"Remove dynamic claims cache for users of role: {role.Id}");
                var userIdList = await UserRepository.GetUserIdListByRoleIdAsync(role.Id, cancellationToken: CancellationToken);
                await DynamicClaimCache.RemoveManyAsync(userIdList.Select(userId => AbpDynamicClaimCacheItem.CalculateCacheKey(userId, null)), token: CancellationToken);
            }
            return result;
        }

        public async override Task<IdentityResult> DeleteAsync(IdentityRole role)
        {
            if (role.IsStatic)
            {
                throw new BusinessException(IdentityErrorCodes.StaticRoleDeletion);
            }

            var result = await base.DeleteAsync(role);

            if (result.Succeeded)
            {
                var userIdList = await UserRepository.GetUserIdListByRoleIdAsync(role.Id, cancellationToken: CancellationToken);
               
                Logger.LogDebug($"Remove dynamic claims cache for users of role: {role.Id}");

                await DynamicClaimCache.RemoveManyAsync(userIdList.Select(userId => AbpDynamicClaimCacheItem.CalculateCacheKey(userId, null)), token: CancellationToken);

                //var orgList = await OrganizationUnitRepository.GetListByRoleIdAsync(role.Id, includeDetails: false, cancellationToken: CancellationToken);

                //foreach (var organizationUnit in orgList)
                //{
                //    await OrganizationUnitManager.RemoveDynamicClaimCacheAsync(organizationUnit);
                //}
            }

            return result;
        }
    }
}
